using rNascar23.LiveFeeds.Models;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.Settings;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23TestApp.Views
{
    public partial class FastestLapsGridView : UserControl, IGridView<Vehicle>
    {
        #region properties

        public ApiSources ApiSource => ApiSources.Vehicles;
        public string Title => "Fastest Laps";

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
        public SpeedTimeType DisplayType { get; set; }

        #endregion

        #region ctor

        public FastestLapsGridView()
        {
            InitializeComponent();

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.Vehicles,
                SortOrderField = "Position",
                SortOrder = 1
            };

            this.Height = 275;

            var userSettings = UserSettingsService.LoadUserSettings();

            DisplayType = userSettings.FastestLapsDisplayType;
        }

        #endregion

        #region private

        private void SetDataSource<T>(IList<T> values)
        {
            var gridDisplayType = DisplayType == SpeedTimeType.Seconds ? "Lap Time" : "M.P.H.";

            this.TitleLabel.Text = $"Fastest Laps ({gridDisplayType})";

            var models = BuildViewModels((IList<Vehicle>)values);

            var dataTable = GridViewTableBuilder.ToDataTable<FastestLapViewModel>(models.ToList());

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

        private IList<FastestLapViewModel> BuildViewModels(IList<Vehicle> vehicles)
        {
            List<FastestLapViewModel> fastestLaps = null;

            if (DisplayType == SpeedTimeType.MPH)
            {
                fastestLaps = vehicles.
                    OrderByDescending(v => v.best_lap_speed).
                    Take(10).
                    Select(v => new FastestLapViewModel()
                    {
                        Driver = v.driver.FullName,
                        Value = Math.Round(v.best_lap_speed, 3).ToString("N3")
                    }).ToList();
            }
            else
            {
                fastestLaps = vehicles.
                    Where(v => v.best_lap_time > 0).
                    OrderBy(v => v.best_lap_time).
                    Take(10).
                    Select(v => new FastestLapViewModel()
                {
                    Driver = v.driver.FullName,
                    Value = Math.Round(v.best_lap_time, 3).ToString("N3")
                }).ToList();
            }

            for (int i = 0; i < fastestLaps.Count; i++)
            {
                fastestLaps[i].Position = i + 1;
            }

            return fastestLaps;
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.RowHeadersVisible = false;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            GridViewColumnBuilder.ConfigureColumn(Column1, "Position", 25);

            GridViewColumnBuilder.ConfigureColumn(Column2, "Driver", 150);

            var valueColumnTitle = DisplayType == SpeedTimeType.Seconds ? "Speed" : "Lap Time";

            GridViewColumnBuilder.ConfigureColumn(Column3, "Value", 75, valueColumnTitle);

            dataGridView.ColumnHeadersVisible = false;
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
