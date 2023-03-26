using rNascar23.LiveFeeds.Models;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23TestApp.Views
{
    public partial class MoversFallersGridView : UserControl, IGridView<Vehicle>
    {
        #region enums

        public enum ViewTypes
        {
            Movers,
            Fallers
        }

        #endregion

        #region properties

        public ApiSources ApiSource => ApiSources.Vehicles;
        public string Title => "MoversFallers";

        private ViewTypes _viewType = ViewTypes.Movers;
        public ViewTypes ViewType
        {
            get
            {
                return _viewType;
            }
            set
            {
                _viewType = value;
            }
        }

        private IList<Vehicle> _data = new List<Vehicle>();
        public IList<Vehicle> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;

                if (_data != null)
                    SetDataSource(_data);
            }
        }
        public string CustomGridName { get; set; }
        public string Description { get; set; }
        public GridSettings Settings { get; set; }
        public bool IsCustomGrid { get; set; }
        public DataGridView DataGridView
        {
            get
            {
                return Grid;
            }
        }

        #endregion

        #region ctor

        public MoversFallersGridView()
            : this(ViewTypes.Movers)
        {

        }

        public MoversFallersGridView(ViewTypes viewType)
        {
            InitializeComponent();

            _viewType = viewType;

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.Vehicles,
                SortOrderField = "Change",
                SortOrder = _viewType == ViewTypes.Movers ? 2 : 1
            };

            SetGridTitle(_viewType);
        }

        #endregion

        #region private

        private void SetGridTitle(ViewTypes viewType)
        {
            switch (viewType)
            {
                case ViewTypes.Movers:
                    TitleLabel.Text = "Biggest Movers";
                    TitleLabel.ForeColor = Color.Black;
                    TitleLabel.BackColor = Color.White;
                    break;
                case ViewTypes.Fallers:
                    TitleLabel.Text = "Biggest Fallers";
                    TitleLabel.ForeColor = Color.White;
                    TitleLabel.BackColor = Color.Black;
                    break;
                default:
                    break;
            }
        }

        private void SetDataSource<T>(IList<T> values)
        {
            var models = BuildViewModels((IList<Vehicle>)values);

            var dataTable = GridViewTableBuilder.ToDataTable<PositionChangeViewModel>(models.ToList());

            var dataView = new DataView(dataTable);

            GridBindingSource.DataSource = dataView;

            Grid.DataSource = GridBindingSource;

            foreach (DataGridViewTextBoxColumn column in Grid.Columns)
            {
                column.Name = column.DataPropertyName;
            }

            if (!String.IsNullOrEmpty(Settings.SortOrderField))
            {
                var sortDirection = Settings.SortOrder == 0 ? String.Empty :
                    Settings.SortOrder == 1 ? "ASC" :
                    "DESC";

                var sortString = $"{Settings.SortOrderField} {sortDirection}".Trim();

                GridBindingSource.Sort = sortString;
            }
        }

        private IList<PositionChangeViewModel> BuildViewModels(IList<Vehicle> vehicles)
        {
            if (ViewType == ViewTypes.Movers)
            {
                var biggestMovers = vehicles.
                    Where(v => v.position_differential_last_10_percent != 0).
                    OrderByDescending(v => v.position_differential_last_10_percent).
                    Take(10).
                    Select(v => new PositionChangeViewModel()
                {
                    Driver = v.driver.FullName,
                    Change = v.position_differential_last_10_percent
                }).ToList();

                for (int i = 0; i < biggestMovers.Count; i++)
                {
                    biggestMovers[i].Position = i + 1;
                }

                return biggestMovers;
            }
            else
            {
                var biggestFallers = vehicles.
                    Where(v => v.position_differential_last_10_percent != 0).
                    OrderBy(v => v.position_differential_last_10_percent).
                    Take(10).
                    Select(v => new PositionChangeViewModel()
                {
                    Driver = v.driver.FullName,
                    Change = v.position_differential_last_10_percent
                }).ToList();

                for (int i = 0; i < biggestFallers.Count; i++)
                {
                    biggestFallers[i].Position = i + 1;
                }

                return biggestFallers;
            }
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            GridViewColumnBuilder.ConfigureColumn(Column1, "Position", 25);

            GridViewColumnBuilder.ConfigureColumn(Column2, "Driver", 150);

            var gainLossTitle = ViewType == ViewTypes.Movers ? "Gain" : "Loss";
            GridViewColumnBuilder.ConfigureColumn(Column3, "Change", 75, gainLossTitle);

            dataGridView.ColumnHeadersVisible = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ReadOnly = true;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.SelectionChanged += (s, e) => Grid.ClearSelection();

            return dataGridView;
        }

        #endregion
    }
}