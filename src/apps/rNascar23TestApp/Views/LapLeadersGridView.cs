using rNascar23.LiveFeeds.Models;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23TestApp.Views
{
    public partial class LapLeadersGridView : UserControl, IGridView<Vehicle>
    {
        #region properties

        public ApiSources ApiSource => ApiSources.Vehicles;
        public string Title => "Lap Leaders";

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

        public LapLeadersGridView()
        {
            InitializeComponent();

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.Vehicles,
                SortOrderField = "Laps",
                SortOrder = 2
            };

            this.Height = 225;
        }

        #endregion

        #region private

        private void SetDataSource<T>(IList<T> values)
        {
            var models = BuildViewModels((IList<Vehicle>)values);

            var dataTable = GridViewTableBuilder.ToDataTable<LapLeaderViewModel>(models.ToList());

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

        private IList<LapLeaderViewModel> BuildViewModels(IList<Vehicle> vehicles)
        {
            IList<LapLeaderViewModel> lapLeaders = new List<LapLeaderViewModel>();

            foreach (var lapLedLeader in vehicles.Where(v => v.laps_led.Length > 0))
            {
                var lapLeader = new LapLeaderViewModel()
                {
                    Driver = lapLedLeader.driver.FullName,
                    Laps = lapLedLeader.laps_led.Sum(l => l.end_lap - l.start_lap) + 1
                };

                lapLeaders.Add(lapLeader);

            }

            int i = 1;
            foreach (LapLeaderViewModel lapLeader in lapLeaders.OrderByDescending(l => l.Laps))
            {
                lapLeader.Position = i;
                i++;
            }

            return lapLeaders;
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column0,
                Column1,
                Column2,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            GridViewColumnBuilder.ConfigureColumn(Column0, "Position", 25, "");

            GridViewColumnBuilder.ConfigureColumn(Column1, "Driver", 150, "Driver");

            GridViewColumnBuilder.ConfigureColumn(Column2, "Laps", 55, "Laps");

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
