using Microsoft.Extensions.DependencyInjection;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.RaceLists.Models;
using rNascar23.RaceLists.Ports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace rNascar23TestApp
{
    public partial class MainForm : Form
    {
        #region fields

        private DataGridView _leftRaceDataGridView = null;
        private DataGridView _rightRaceDataGridView = null;
        private DataGridView _genericDataGridView = null;

        #endregion

        #region ctor / load

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetupRaceView();
        }

        #endregion

        #region private [event handlers]

        // timer
        private void AutoUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // menu items

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = rNascar23TestApp.Program.Services.GetRequiredService<IRaceListRepository>();

                var result = repo.GetRaceList();

                var allRaces = result.CupSeries.Concat(result.XfinitySeries).Concat(result.TruckSeries).ToList();

                dataGridView1.DataSource = allRaces.OrderBy(s => s.date_scheduled).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void truckRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = rNascar23TestApp.Program.Services.GetRequiredService<IRaceListRepository>();

                var result = repo.GetRaceList();

                dataGridView1.DataSource = result.TruckSeries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void xfinityRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = rNascar23TestApp.Program.Services.GetRequiredService<IRaceListRepository>();

                var result = repo.GetRaceList();

                dataGridView1.DataSource = result.XfinitySeries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void cupRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = rNascar23TestApp.Program.Services.GetRequiredService<IRaceListRepository>();

                var result = repo.GetRaceList();

                dataGridView1.DataSource = result.CupSeries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void vehicleListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = rNascar23TestApp.Program.Services.GetRequiredService<ILiveFeedRepository>();

                var result = repo.GetLiveFeed();

                dataGridView1.DataSource = result.Vehicles.OrderBy(v => v.running_position).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void liveFeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var repo = rNascar23TestApp.Program.Services.GetRequiredService<ILiveFeedRepository>();

                var result = repo.GetLiveFeed();

                dataGridView1.DataSource = new List<LiveFeed>() { result };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void autoUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoUpdateTimer.Enabled = !AutoUpdateTimer.Enabled;

            if (AutoUpdateTimer.Enabled)
            {
                lblAutoUpdateStatus.Text = "Auto-Update Status: On";
                lblAutoUpdateStatus.BackColor = Color.LimeGreen;
            }
            else
            {
                lblAutoUpdateStatus.Text = "Auto-Update Status: Off";
                lblAutoUpdateStatus.BackColor = SystemColors.Control;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region private

        private void SetupRaceView()
        {
            _rightRaceDataGridView = BuildRaceViewGrid();

            panelGrid.Controls.Add(_rightRaceDataGridView);
            _rightRaceDataGridView.Width = 850;
            _rightRaceDataGridView.Dock = DockStyle.Left;

            _leftRaceDataGridView = BuildRaceViewGrid();
            panelGrid.Controls.Add(_leftRaceDataGridView);
            _leftRaceDataGridView.Width = 850;
            _leftRaceDataGridView.Dock = DockStyle.Left;
        }

        private DataGridView BuildRaceViewGrid()
        {
            var dataGridView = new DataGridView();

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

            dataGridView.RowHeadersVisible = false;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
                Column4,
                Column5,
                Column6,
                Column7,
                Column8,
                Column9,
                Column10,
                Column11,
                Column12
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Bold);

            Column1.HeaderText = "";
            Column1.Name = "Column1";
            Column1.Width = 45;
            Column1.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column1.DataPropertyName = "RunningPosition";

            Column2.HeaderText = "#";
            Column2.Name = "Column2";
            Column2.Width = 45;
            Column2.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column2.DataPropertyName = "CarNumber";

            Column3.HeaderText = "Driver";
            Column3.Name = "Column3";
            Column3.Width = 200;
            Column3.DataPropertyName = "Driver";

            Column4.HeaderText = "Car";
            Column4.Name = "Column4";
            Column4.Width = 50;
            Column4.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column4.DataPropertyName = "CarManufacturer";

            Column5.HeaderText = "Laps";
            Column5.Name = "Column5";
            Column5.Width = 50;
            Column5.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column5.DataPropertyName = "LapsCompleted";

            Column6.HeaderText = "To Leader";
            Column6.Name = "Column6";
            Column6.Width = 65;
            Column6.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column6.DataPropertyName = "DeltaLeader";

            Column7.HeaderText = "To Next";
            Column7.Name = "Column7";
            Column7.Width = 75;
            Column7.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column7.DataPropertyName = "DeltaNext";

            Column8.HeaderText = "On Track";
            Column8.Name = "Column8";
            Column8.Width = 0;
            Column8.Visible = false;
            Column8.DataPropertyName = "IsOnTrack";

            Column9.HeaderText = "Last Lap";
            Column9.Name = "Column9";
            Column9.Width = 75;
            Column9.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column9.DataPropertyName = "LastLap";

            Column10.HeaderText = "Best Lap";
            Column10.Name = "Column10";
            Column10.Width = 75;
            Column10.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column10.DataPropertyName = "BestLap";

            Column11.HeaderText = "On Lap";
            Column11.Name = "Column11";
            Column11.Width = 65;
            Column11.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column11.DataPropertyName = "BestLapNumber";

            Column12.HeaderText = "Last Pit";
            Column12.Name = "Column12";
            Column12.Width = 75;
            Column12.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column12.DataPropertyName = "LastPit";

            return dataGridView;
        }

        private IList<Series> GetSeriesRaceList(int seriesId)
        {
            var repo = rNascar23TestApp.Program.Services.GetRequiredService<IRaceListRepository>();

            var result = repo.GetRaceList();

            switch (seriesId)
            {
                case 1:
                    {
                        return result.CupSeries;
                    }
                case 2:
                    {
                        return result.XfinitySeries;
                    }
                case 3:
                    {
                        return result.TruckSeries;
                    }
                case 4:
                    {
                        return null;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private void UpdateData()
        {
            try
            {
                var repo = rNascar23TestApp.Program.Services.GetRequiredService<ILiveFeedRepository>();

                var result = repo.GetLiveFeed();

                DisplayHeaderData(result);

                DisplayVehicleData(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string GetSeriesName(int seriesId)
        {
            return seriesId == 1 ? "Cup Series" :
                seriesId == 2 ? "Xfinity Series" :
                seriesId == 3 ? "Craftsman Truck Series" :
                seriesId == 4 ? "ARCA Menards Series" :
                "Unknown";
        }

        private void DisplayVehicleData(LiveFeed result)
        {
            var raceVehicles = new List<RaceVehicleView>();

            Vehicle lastVehicle = null;
            foreach (var vehicle in result.Vehicles)
            {
                raceVehicles.Add(new RaceVehicleView()
                {
                    RunningPosition = vehicle.running_position,
                    CarManufacturer = vehicle.vehicle_manufacturer,
                    CarNumber = vehicle.vehicle_number,
                    DeltaLeader = vehicle.delta,
                    DeltaNext = (float)Math.Round(lastVehicle == null ? 0 : vehicle.delta - lastVehicle.delta, 3),
                    Driver = vehicle.driver.full_name,
                    LapsCompleted = vehicle.laps_completed,
                    IsOnTrack = vehicle.is_on_track,
                    LastLap = vehicle.last_lap_time,
                    BestLap = vehicle.best_lap_time,
                    BestLapNumber = vehicle.best_lap,
                    LastPit = vehicle.last_pit
                });

                lastVehicle = vehicle;
            }

            var bestLapTime = raceVehicles.OrderBy(v => v.LastLap).FirstOrDefault().LastLap;

            _leftRaceDataGridView.DataSource = raceVehicles.Where(v => v.RunningPosition <= 20).OrderBy(v => v.RunningPosition).ToList();
            foreach (DataGridViewRow row in _leftRaceDataGridView.Rows)
            {
                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }

                // Fastest this lap.
                if ((float)row.Cells[9].Value == bestLapTime)
                {
                    row.Cells[9].Style.BackColor = Color.LimeGreen;

                    // Best lap for driver for the race
                    if ((float)row.Cells[9].Value == (float)row.Cells[8].Value)
                    {
                        row.Cells[8].Style.BackColor = Color.LimeGreen;
                    }
                }

                // off track
                if ((bool)row.Cells[7].Value == false)
                {
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }

            _rightRaceDataGridView.DataSource = raceVehicles.Where(v => v.RunningPosition > 20).OrderBy(v => v.RunningPosition).ToList();
            foreach (DataGridViewRow row in _rightRaceDataGridView.Rows)
            {
                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }

                // Fastest this lap.
                if ((float)row.Cells[9].Value == bestLapTime)
                {
                    row.Cells[9].Style.BackColor = Color.LimeGreen;

                    if ((float)row.Cells[9].Value == (float)row.Cells[8].Value)
                    {
                        row.Cells[8].Style.BackColor = Color.LimeGreen;
                    }
                }

                // off track
                if ((bool)row.Cells[7].Value == false)
                {
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void DisplayHeaderData(LiveFeed result)
        {
            rNascar23.RaceLists.Models.Series seriesRace = null;

            var seriesRaceList = GetSeriesRaceList(result.SeriesId);

            seriesRace = seriesRaceList.FirstOrDefault(s => s.race_id == result.RaceId);

            picStatus.BackColor = result.FlagState == 8 ? Color.Orange :
                result.FlagState == 1 ? Color.LimeGreen :
                result.FlagState == 2 ? Color.Yellow :
                result.FlagState == 3 ? Color.Red :
                result.FlagState == 4 ? Color.White :
                Color.DimGray;

            lblEventName.Text = $"{result.RunName} - {GetSeriesName(result.SeriesId)} - {result.TrackName} ({seriesRace.stage_1_laps}/{seriesRace.stage_2_laps}/{seriesRace.stage_3_laps})";

            lblRaceLaps.Text = $"Race: Lap {result.LapNumber} of {result.LapsInRace}";

            var stageStartLap = result.Stage.FinishAtLap - result.Stage.LapsInStage;

            lblStageLaps.Text = $"Stage {result.Stage.Number}: Lap {result.LapNumber - stageStartLap} of {result.Stage.LapsInStage}";
        }

        #endregion
    }
}
