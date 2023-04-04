using rNasar23.Common;
using rNascar23.Common;
using rNascar23.LiveFeeds.Models;
using rNascar23.CustomViews;
using rNascar23.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class LeaderboardGridView : UserControl, IGridView<Vehicle>
    {
        #region consts

        private const int VehicleCountPerLeaderboardSide = 20;

        #endregion

        #region emuns

        public enum LeaderboardSides
        {
            Left,
            Right
        }

        public enum RunTypes
        {
            Practice,
            Qualifying,
            Race
        }

        #endregion

        #region properties

        public ApiSources ApiSource => ApiSources.Vehicles;
        public string Title => "Leaderboard";

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
        public LeaderboardSides LeaderboardSide { get; set; }
        public RunTypes RunType { get; set; }
        public DataGridView DataGridView
        {
            get
            {
                return Grid;
            }
        }
        public SpeedTimeType LastLapDisplayType { get; set; }
        public SpeedTimeType BestLapDisplayType { get; set; }

        #endregion

        #region ctor

        public LeaderboardGridView()
            : this(LeaderboardSides.Left, RunTypes.Race)
        {

        }

        public LeaderboardGridView(LeaderboardSides leaderboardSide, RunTypes runType)
        {
            InitializeComponent();

            LeaderboardSide = leaderboardSide;
            RunType = runType;

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.Vehicles,
                SortOrderField = "RunningPosition",
                SortOrder = 1
            };

            var userSettings = UserSettingsService.LoadUserSettings();

            LastLapDisplayType = userSettings.LeaderboardLastLapDisplayType;
            BestLapDisplayType = userSettings.LeaderboardBestLapDisplayType;
        }

        #endregion

        #region private

        private void SetDataSource<T>(IList<T> values)
        {
            var models = BuildViewModels((IList<Vehicle>)values);

            var dataTable = GridViewTableBuilder.ToDataTable<RaceVehicleViewModel>(models.ToList());

            var dataView = new DataView(dataTable);

            GridBindingSource.DataSource = dataView;

            Grid.DataSource = GridBindingSource;

            foreach (DataGridViewColumn column in Grid.Columns)
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

            var settings = UserSettingsService.LoadUserSettings();

            foreach (DataGridViewRow row in Grid.Rows)
            {
                if (settings.FavoriteDrivers.Contains(row.Cells["Driver"]?.Value?.ToString()))
                {
                    row.Cells["Driver"].Style.BackColor = Color.Gold;
                }

                if (row.Cells["FastestLapInRace"]?.Value?.ToString() == "True")
                {
                    row.Cells["BestLap"].Style.BackColor = Color.LimeGreen;
                }
                if (row.Cells["PersonalBestLapThisLap"]?.Value?.ToString() == "True")
                {
                    row.Cells["BestLap"].Style.BackColor = Color.LimeGreen;
                    row.Cells["BestLapNumber"].Style.BackColor = Color.LimeGreen;
                }
                if (row.Cells["FastestCarThisLap"]?.Value?.ToString() == "True")
                {
                    row.Cells["LastLap"].Style.BackColor = Color.LawnGreen;
                }

                if (row.Cells["IsOnDvp"]?.Value?.ToString() == "True")
                {
                    row.Cells["VehicleStatusImage"].Style.BackColor = Color.Red;
                }
                if (row.Cells["IsOnTrack"]?.Value?.ToString() == "False")
                {
                    row.DefaultCellStyle.ForeColor = Color.Silver;
                }

                if (row.Cells["CarManufacturer"]?.Value?.ToString() == "Chv")
                {
                    ((DataGridViewImageCell)row.Cells["CarManufacturerImage"]).Value = Properties.Resources.Chevrolet_Small;
                }
                else if (row.Cells["CarManufacturer"]?.Value?.ToString() == "Frd")
                {
                    ((DataGridViewImageCell)row.Cells["CarManufacturerImage"]).Value = Properties.Resources.Ford_Small;
                }
                else if (row.Cells["CarManufacturer"]?.Value?.ToString() == "Tyt")
                {
                    ((DataGridViewImageCell)row.Cells["CarManufacturerImage"]).Value = Properties.Resources.Toyota_Small;
                }
                else
                {
                    ((DataGridViewImageCell)row.Cells["CarManufacturerImage"]).Value = Properties.Resources.TransparentPixel;
                }

                if (row.Cells["VehicleStatus"]?.Value != null)
                {
                    if ((VehicleEventStatus)row.Cells["VehicleStatus"].Value == VehicleEventStatus.InPits)
                    {
                        row.DefaultCellStyle.ForeColor = Color.DimGray;
                        ((DataGridViewImageCell)row.Cells["VehicleStatusImage"]).Value = Properties.Resources.Tire;
                    }
                    else if ((VehicleEventStatus)row.Cells["VehicleStatus"].Value == VehicleEventStatus.Garage)
                    {
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                        ((DataGridViewImageCell)row.Cells["VehicleStatusImage"]).Value = Properties.Resources.Garage;
                    }
                    else if ((VehicleEventStatus)row.Cells["VehicleStatus"].Value == VehicleEventStatus.Retired)
                    {
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                        ((DataGridViewImageCell)row.Cells["VehicleStatusImage"]).Value = Properties.Resources.Retired;
                    }
                    else
                    {
                        ((DataGridViewImageCell)row.Cells["VehicleStatusImage"]).Value = Properties.Resources.TransparentPixel;
                    }
                }
                else
                {
                    ((DataGridViewImageCell)row.Cells["VehicleStatusImage"]).Value = Properties.Resources.TransparentPixel;
                }
            }
        }

        private IList<RaceVehicleViewModel> BuildViewModels(IList<Vehicle> vehicles)
        {
            var raceVehicles = new List<RaceVehicleViewModel>();

            Vehicle lastVehicle = null;

            var bestLapSpeedInRace = vehicles.Max(v => v.best_lap_speed);
            var currentLap = vehicles.Max(v => v.laps_completed);
            var bestLapSpeedThisLap = vehicles.Max(v => v.last_lap_speed);

            var splitVehiclesList = LeaderboardSide == LeaderboardSides.Left ?
                vehicles.OrderBy(v => v.running_position).Take(VehicleCountPerLeaderboardSide) :
                vehicles.OrderBy(v => v.running_position).Skip(VehicleCountPerLeaderboardSide);

            foreach (var vehicle in splitVehiclesList)
            {
                var model = new RaceVehicleViewModel()
                {
                    RunningPosition = vehicle.running_position,
                    CarManufacturer = vehicle.vehicle_manufacturer,
                    CarNumber = vehicle.vehicle_number,
                    DeltaLeader = vehicle.delta,
                    DeltaNext = (float)Math.Round(lastVehicle == null ? 0 : vehicle.delta - lastVehicle.delta, 3),
                    Driver = vehicle.driver.FullName,
                    LapsCompleted = vehicle.laps_completed,
                    VehicleStatus = (VehicleEventStatus)vehicle.status,
                    IsOnTrack = vehicle.is_on_track,
                    IsOnDvp = vehicle.is_on_dvp,
                    LastLap = LastLapDisplayType == SpeedTimeType.Seconds ? vehicle.last_lap_time : vehicle.last_lap_speed,
                    BestLap = BestLapDisplayType == SpeedTimeType.Seconds ? vehicle.best_lap_time : vehicle.best_lap_speed,
                    BestLapNumber = vehicle.best_lap,
                    LastPit = vehicle.last_pit,
                    PersonalBestLapThisLap = vehicle.best_lap == vehicle.laps_completed,
                    FastestLapInRace = vehicle.best_lap_speed == bestLapSpeedInRace,
                    FastestCarThisLap = vehicle.last_lap_speed == bestLapSpeedThisLap
                };

                raceVehicles.Add(model);

                lastVehicle = vehicle;
            }

            return raceVehicles;
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn Column8 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            DataGridViewTextBoxColumn Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewImageColumn colManufacturerImage = new System.Windows.Forms.DataGridViewImageColumn();
            DataGridViewImageColumn colVehicleStatusImage = new System.Windows.Forms.DataGridViewImageColumn();

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
                Column4,
                colManufacturerImage,
                Column5,
                Column6,
                Column7,
                Column8,
                Column9,
                Column10,
                Column11,
                Column12,
                Column13,
                Column14,
                Column15,
                Column16,
                Column17,
                Column18,
                colVehicleStatusImage
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);

            Column1.HeaderText = "";
            Column1.Name = "RunningPosition";
            Column1.Width = 45;
            Column1.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column1.DataPropertyName = "RunningPosition";

            Column2.HeaderText = "#";
            Column2.Name = "CarNumber";
            Column2.Width = 45;
            Column2.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column2.DataPropertyName = "CarNumber";

            Column3.HeaderText = "Driver";
            Column3.Name = "Driver";
            Column3.Width = 200;
            Column3.DataPropertyName = "Driver";

            Column4.HeaderText = "Car";
            Column4.Name = "CarManufacturer";
            Column4.Width = 0;
            Column4.Visible = false;
            Column4.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column4.DataPropertyName = "CarManufacturer";

            Column5.HeaderText = "Laps";
            Column5.Name = "LapsCompleted";
            Column5.Width = 50;
            Column5.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column5.DataPropertyName = "LapsCompleted";

            Column6.HeaderText = "To Leader";
            Column6.Name = "DeltaLeader";
            Column6.Width = 65;
            Column6.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column6.DataPropertyName = "DeltaLeader";

            Column7.HeaderText = "To Next";
            Column7.Name = "DeltaNext";
            Column7.Width = 65;
            Column7.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column7.DataPropertyName = "DeltaNext";

            Column8.HeaderText = "On Track";
            Column8.Name = "IsOnTrack";
            Column8.Width = 0;
            Column8.Visible = false;
            Column8.DataPropertyName = "IsOnTrack";

            Column9.HeaderText = "Last Lap";
            Column9.Name = "LastLap";
            Column9.Width = 75;
            Column9.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column9.DefaultCellStyle.Format = "N3";
            Column9.DataPropertyName = "LastLap";

            Column10.HeaderText = "Best Lap";
            Column10.Name = "BestLap";
            Column10.Width = 75;
            Column10.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column10.DefaultCellStyle.Format = "N3";
            Column10.DataPropertyName = "BestLap";

            Column11.HeaderText = "On Lap";
            Column11.Name = "BestLapNumber";
            Column11.Width = 45;
            Column11.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column11.DataPropertyName = "BestLapNumber";

            Column12.HeaderText = "Last Pit";
            Column12.Name = "LastPit";
            Column12.Width = 45;
            Column12.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column12.DataPropertyName = "LastPit";

            Column13.HeaderText = "PersonalBestLapThisLap";
            Column13.Name = "PersonalBestLapThisLap";
            Column13.Width = 0;
            Column13.Visible = false;
            Column13.DataPropertyName = "PersonalBestLapThisLap";

            Column14.HeaderText = "FastestLapInRace";
            Column14.Name = "FastestLapInRace";
            Column14.Width = 0;
            Column14.Visible = false;
            Column14.DataPropertyName = "FastestLapInRace";

            Column15.HeaderText = "FastestCarThisLap";
            Column15.Name = "FastestCarThisLap";
            Column15.Width = 0;
            Column15.Visible = false;
            Column15.DataPropertyName = "FastestCarThisLap";

            Column16.HeaderText = "VehicleStatus";
            Column16.Name = "VehicleStatus";
            Column16.Width = 0;
            Column16.Visible = false;
            Column16.DataPropertyName = "VehicleStatus";

            Column17.HeaderText = "IsOnDvp";
            Column17.Name = "IsOnDvp";
            Column17.Width = 0;
            Column17.Visible = false;
            Column17.DataPropertyName = "IsOnDvp";

            Column18.HeaderText = "Status";
            Column18.Name = "Status";
            Column18.Width = 0;
            Column18.Visible = false;
            Column18.DataPropertyName = "StatusLabel";

            ((DataGridViewImageColumn)colManufacturerImage).HeaderText = "";
            ((DataGridViewImageColumn)colManufacturerImage).Name = "CarManufacturerImage";
            ((DataGridViewImageColumn)colManufacturerImage).Width = 30;
            ((DataGridViewImageColumn)colManufacturerImage).Visible = true;
            ((DataGridViewImageColumn)colManufacturerImage).ImageLayout = DataGridViewImageCellLayout.Zoom;
            ((DataGridViewImageColumn)colManufacturerImage).DataPropertyName = "CarManufacturerImage";

            ((DataGridViewImageColumn)colVehicleStatusImage).HeaderText = "Status";
            ((DataGridViewImageColumn)colVehicleStatusImage).Name = "VehicleStatusImage";
            ((DataGridViewImageColumn)colVehicleStatusImage).Width = 40;
            ((DataGridViewImageColumn)colVehicleStatusImage).Visible = true;
            ((DataGridViewImageColumn)colVehicleStatusImage).ImageLayout = DataGridViewImageCellLayout.Zoom;
            ((DataGridViewImageColumn)colVehicleStatusImage).DataPropertyName = "VehicleStatusImage";

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
