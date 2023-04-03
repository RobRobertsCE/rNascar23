using Microsoft.Extensions.DependencyInjection;
using rNascar23.LapTimes.Models;
using rNascar23.LapTimes.Ports;
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
    public partial class MoversFallersGridView : UserControl, IGridView<LapTimeData>
    {
        #region fields

        IMoversFallersService _moversFallersService = null;

        #endregion

        #region properties

        public ApiSources ApiSource => ApiSources.LapTimeData;
        public string Title => "MoversFallers";
        private IList<LapTimeData> _data = new List<LapTimeData>();
        public IList<LapTimeData> Data
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
        public int MoversCount { get; set; } = 10;
        public int FallersCount { get; set; } = 10;

        #endregion

        #region ctor

        public MoversFallersGridView()
        {
            InitializeComponent();

            BuildGridView(Grid);

            BuildGridView(FallersGrid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.LapTimeData
            };

            _moversFallersService = Program.Services.GetRequiredService<IMoversFallersService>();

            this.Width = 275;
            this.Height = 350;
        }

        #endregion

        #region private

        private void SetDataSource<T>(IList<T> values)
        {
            var lapTimeData = ((IList<LapTimeData>)values).FirstOrDefault();

            var changes = _moversFallersService.GetDriverPositionChanges(lapTimeData);

            // Movers
            var moversModels = changes.
                OrderByDescending(c => c.ChangeSinceFlagChange).
                Where(c => c.ChangeSinceFlagChange > 0).
                Take(MoversCount).
                ToList();

            var dataTable = GridViewTableBuilder.ToDataTable<PositionChange>(moversModels.ToList());

            var dataView = new DataView(dataTable);

            GridBindingSource.DataSource = dataView;

            Grid.DataSource = GridBindingSource;

            foreach (DataGridViewTextBoxColumn column in Grid.Columns)
            {
                column.Name = column.DataPropertyName;
            }

            GridBindingSource.Sort = "ChangeSinceFlagChange DESC";

            // Fallers
            var fallersModels = changes.
                OrderBy(c => c.ChangeSinceFlagChange).
                Where(c => c.ChangeSinceFlagChange < 0).
                Take(FallersCount).
                ToList();

            var fallersDataTable = GridViewTableBuilder.ToDataTable<PositionChange>(fallersModels.ToList());

            var fallersDataView = new DataView(fallersDataTable);

            FallersGridBindingSource.DataSource = fallersDataView;

            FallersGrid.DataSource = FallersGridBindingSource;

            foreach (DataGridViewTextBoxColumn column in FallersGrid.Columns)
            {
                column.Name = column.DataPropertyName;
            }

            FallersGridBindingSource.Sort = "ChangeSinceFlagChange ASC";

            Grid.ClearSelection();
            FallersGrid.ClearSelection();
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column2,
                Column3,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            GridViewColumnBuilder.ConfigureColumn(Column2, "Driver", 150);

            GridViewColumnBuilder.ConfigureColumn(Column3, "ChangeSinceFlagChange", 75);

            dataGridView.ColumnHeadersVisible = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ReadOnly = true;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.SelectionChanged += (s, e) => Grid.ClearSelection();
            dataGridView.ClearSelection();

            return dataGridView;
        }

        #endregion
    }
}