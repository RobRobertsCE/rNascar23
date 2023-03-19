using Microsoft.Extensions.DependencyInjection;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.RaceLists.Models;
using rNascar23.RaceLists.Ports;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23TestApp
{
    public partial class MainForm : Form
    {
        #region enums

        private enum ViewState
        {
            None,
            Practice,
            Qualifying,
            Race,
            Info,
            SeriesSchedule,
            EventSchedule
        }

        private enum RunType
        {
            Practice = 1,
            Qualifying = 2,
            Race = 3
        }

        private enum SeriesType
        {
            Trucks = 1,
            Xfinity,
            Cup,
            All
        }

        #endregion

        #region fields

        private DataGridView _leftRaceDataGridView = null;
        private DataGridView _rightRaceDataGridView = null;
        private DataGridView _genericDataGridView = null;
        private DataGridView _seriesScheduleDataGridView = null;
        private DataGridView _eventScheduleDataGridView = null;
        private DataGridView _fastestLapsDataGridView = null;
        private DataGridView _biggestMoversDataGridView = null;
        private DataGridView _biggestFallersDataGridView = null;
        private ViewState _viewState = ViewState.None;
        private DateTime _lastLiveFeedTimestamp = DateTime.MinValue;

        #endregion

        #region ctor / load

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewState.None, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion

        #region private [event handlers]

        // timer
        private void AutoUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                GetLiveFeedData();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        // menu items
        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplaySeriesSchedule(SeriesType.All);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void truckRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplaySeriesSchedule(SeriesType.Trucks);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void xfinityRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplaySeriesSchedule(SeriesType.Xfinity);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void cupRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplaySeriesSchedule(SeriesType.Cup);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void vehicleListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayRawVehicleData();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void liveFeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayRawLiveFeedData();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void autoUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetAutoUpdateState(!AutoUpdateTimer.Enabled);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // view state
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewState.Info);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void raceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewState.Race);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void qualifyingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewState.Qualifying);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void practiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewState.Practice);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewState.None);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion

        #region private

        private void ExceptionHandler(Exception ex)
        {
            ExceptionHandler(ex, false);
        }
        private void ExceptionHandler(Exception ex, bool logMessage = false)
        {
            Console.WriteLine(ex.ToString());
            MessageBox.Show(ex.Message);
        }

        private void SetAutoUpdateState(bool isEnabled)
        {
            if (AutoUpdateTimer.Enabled == isEnabled)
                return;

            autoUpdateToolStripMenuItem.Checked = isEnabled;

            AutoUpdateTimer.Enabled = isEnabled;

            if (AutoUpdateTimer.Enabled)
            {
                lblAutoUpdateStatus.Text = "Auto-Update On";
                lblAutoUpdateStatus.BackColor = Color.LimeGreen;
            }
            else
            {
                lblAutoUpdateStatus.Text = "Auto-Update Off";
                lblAutoUpdateStatus.BackColor = SystemColors.Control;
            }
        }

        private void SetViewState(ViewState newViewState)
        {
            SetViewState(newViewState, false);
        }
        private void SetViewState(ViewState newViewState, bool forceRefresh = false)
        {
            if (newViewState == _viewState && forceRefresh == false)
                return;

            SetUiView(newViewState);

            _viewState = newViewState;

            lblViewState.Text = $"View: {_viewState}";
        }

        private void SetUiView(ViewState viewState)
        {
            ClearViewControls();

            switch (viewState)
            {
                case ViewState.None:
                    break;
                case ViewState.Practice:
                    DisplayPracticeViewState();
                    break;
                case ViewState.Qualifying:
                    DisplayQualifyingViewState();
                    break;
                case ViewState.Race:
                    DisplayRaceViewState();
                    break;
                case ViewState.Info:
                    DisplayInfoViewState();
                    break;
                case ViewState.SeriesSchedule:
                    DisplaySeriesScheduleViewState();
                    break;
                case ViewState.EventSchedule:
                    DisplayEventScheduleViewState();
                    break;
                default:
                    break;
            }
        }

        private void ClearViewControls()
        {
            var existingControls = new List<Control>();

            foreach (Control existingControl in pnlMain.Controls)
            {
                existingControls.Add(existingControl);
            }

            foreach (Control existingControl in pnlBottom.Controls)
            {
                existingControls.Add(existingControl);
            }

            pnlMain.Controls.Clear();
            pnlBottom.Controls.Clear();

            for (int i = existingControls.Count - 1; i >= 0; i--)
            {
                existingControls[i].Dispose();
                existingControls[i] = null;
            }

            lblRaceLaps.Visible = false;
            lblRaceLaps.Text = "-";

            lblStageLaps.Visible = false;
            lblStageLaps.Text = "-";
        }

        private void DisplaySeriesScheduleViewState()
        {
            if (_seriesScheduleDataGridView != null)
            {
                _seriesScheduleDataGridView.SelectionChanged -= _seriesScheduleDataGridView_SelectionChanged;
                _seriesScheduleDataGridView.Dispose();
                _seriesScheduleDataGridView = null;
            }

            _seriesScheduleDataGridView = new DataGridView();
            pnlMain.Controls.Add(_seriesScheduleDataGridView);
            _seriesScheduleDataGridView.Dock = DockStyle.Fill;

            DisplayEventScheduleViewState();
        }

        private void DisplayEventScheduleViewState()
        {
            if (_eventScheduleDataGridView != null)
            {
                _eventScheduleDataGridView.Dispose();
                _eventScheduleDataGridView = null;
            }

            _eventScheduleDataGridView = new DataGridView();

            pnlBottom.Controls.Add(_eventScheduleDataGridView);
            _eventScheduleDataGridView.Dock = DockStyle.Fill;
        }

        private void DisplayPracticeViewState()
        {
            if (_rightRaceDataGridView != null)
            {
                _rightRaceDataGridView.Dispose();
                _rightRaceDataGridView = null;
            }
            _rightRaceDataGridView = BuildPracticeViewGrid();
            pnlMain.Controls.Add(_rightRaceDataGridView);
            _rightRaceDataGridView.Width = 850;
            _rightRaceDataGridView.Dock = DockStyle.Left;

            if (_leftRaceDataGridView != null)
            {
                _leftRaceDataGridView.Dispose();
                _leftRaceDataGridView = null;
            }
            _leftRaceDataGridView = BuildPracticeViewGrid();
            pnlMain.Controls.Add(_leftRaceDataGridView);
            _leftRaceDataGridView.Width = 850;
            _leftRaceDataGridView.Dock = DockStyle.Left;
        }

        private void DisplayQualifyingViewState()
        {
            if (_rightRaceDataGridView != null)
            {
                _rightRaceDataGridView.Dispose();
                _rightRaceDataGridView = null;
            }
            _rightRaceDataGridView = BuildQualifyingViewGrid();
            pnlMain.Controls.Add(_rightRaceDataGridView);
            _rightRaceDataGridView.Width = 850;
            _rightRaceDataGridView.Dock = DockStyle.Left;

            if (_leftRaceDataGridView != null)
            {
                _leftRaceDataGridView.Dispose();
                _leftRaceDataGridView = null;
            }
            _leftRaceDataGridView = BuildQualifyingViewGrid();
            pnlMain.Controls.Add(_leftRaceDataGridView);
            _leftRaceDataGridView.Width = 850;
            _leftRaceDataGridView.Dock = DockStyle.Left;
        }

        private void DisplayRaceViewState()
        {
            if (_rightRaceDataGridView != null)
            {
                _rightRaceDataGridView.Dispose();
                _rightRaceDataGridView = null;
            }
            _rightRaceDataGridView = BuildRaceViewGrid();
            pnlMain.Controls.Add(_rightRaceDataGridView);
            _rightRaceDataGridView.Width = 835;
            _rightRaceDataGridView.Dock = DockStyle.Left;

            if (_leftRaceDataGridView != null)
            {
                _leftRaceDataGridView.Dispose();
                _leftRaceDataGridView = null;
            }
            _leftRaceDataGridView = BuildRaceViewGrid();
            pnlMain.Controls.Add(_leftRaceDataGridView);
            _leftRaceDataGridView.Width = 835;
            _leftRaceDataGridView.Dock = DockStyle.Left;

            if (_fastestLapsDataGridView != null)
            {
                _fastestLapsDataGridView.Dispose();
                _fastestLapsDataGridView = null;
            }
            _fastestLapsDataGridView = BuildFastestLapsViewGrid();
            pnlRight.Controls.Add(_fastestLapsDataGridView);
            _fastestLapsDataGridView.Height = 275;
            _fastestLapsDataGridView.Dock = DockStyle.Top;

            if (_biggestMoversDataGridView != null)
            {
                _biggestMoversDataGridView.Dispose();
                _biggestMoversDataGridView = null;
            }
            _biggestMoversDataGridView = BuildBiggestMoversViewGrid();
            pnlBottom.Controls.Add(_biggestMoversDataGridView);
            _biggestMoversDataGridView.Width = 275;
            _biggestMoversDataGridView.Dock = DockStyle.Left;

            // _biggestFallersDataGridView
            if (_biggestFallersDataGridView != null)
            {
                _biggestFallersDataGridView.Dispose();
                _biggestFallersDataGridView = null;
            }
            _biggestFallersDataGridView = BuildBiggestFallersViewGrid();
            pnlBottom.Controls.Add(_biggestFallersDataGridView);
            _biggestFallersDataGridView.Width = 275;
            _biggestFallersDataGridView.Dock = DockStyle.Left;

            lblRaceLaps.Visible = true;
            lblRaceLaps.Text = "-";

            lblStageLaps.Visible = true;
            lblStageLaps.Text = "-";
        }

        private void DisplayInfoViewState()
        {
            if (_genericDataGridView != null)
            {
                _genericDataGridView.Dispose();
                _genericDataGridView = null;
            }

            _genericDataGridView = new DataGridView();
            pnlMain.Controls.Add(_genericDataGridView);
            _genericDataGridView.Dock = DockStyle.Fill;
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
            Column4.Width = 50;
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
            Column7.Width = 75;
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
            Column9.DataPropertyName = "LastLap";

            Column10.HeaderText = "Best Lap";
            Column10.Name = "BestLap";
            Column10.Width = 75;
            Column10.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column10.DataPropertyName = "BestLap";

            Column11.HeaderText = "On Lap";
            Column11.Name = "BestLapNumber";
            Column11.Width = 65;
            Column11.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column11.DataPropertyName = "BestLapNumber";

            Column12.HeaderText = "Last Pit";
            Column12.Name = "LastPit";
            Column12.Width = 75;
            Column12.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column12.DataPropertyName = "LastPit";

            return dataGridView;
        }

        private DataGridView BuildQualifyingViewGrid()
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
            Column4.Width = 50;
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
            Column7.Width = 75;
            Column7.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column7.DataPropertyName = "DeltaNext";

            Column8.HeaderText = "On Track";
            Column8.Name = "IsOnTrack";
            Column8.DataPropertyName = "IsOnTrack";
            Column8.Visible = false;

            Column9.HeaderText = "Last Lap";
            Column9.Name = "LastLap";
            Column9.Width = 75;
            Column9.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column9.DataPropertyName = "LastLap";

            Column10.HeaderText = "Best Lap";
            Column10.Name = "BestLap";
            Column10.Width = 75;
            Column10.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column10.DataPropertyName = "BestLap";

            Column11.HeaderText = "On Lap";
            Column11.Name = "BestLapNumber";
            Column11.Width = 65;
            Column11.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column11.DataPropertyName = "BestLapNumber";

            Column12.HeaderText = "Last Pit";
            Column12.Name = "LastPit";
            Column12.Width = 75;
            Column12.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column12.DataPropertyName = "LastPit";
            Column12.Visible = false;

            return dataGridView;
        }

        private DataGridView BuildPracticeViewGrid()
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
            Column4.Width = 50;
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
            Column7.Name = "Column7";
            Column7.Width = 75;
            Column7.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column7.DataPropertyName = "DeltaNext";

            Column8.HeaderText = "On Track";
            Column8.Name = "IsOnTrack";
            Column8.DataPropertyName = "IsOnTrack";
            Column8.Visible = false;

            Column9.HeaderText = "Last Lap";
            Column9.Name = "LastLap";
            Column9.Width = 75;
            Column9.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column9.DataPropertyName = "LastLap";

            Column10.HeaderText = "Best Lap";
            Column10.Name = "BestLap";
            Column10.Width = 75;
            Column10.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column10.DataPropertyName = "BestLap";

            Column11.HeaderText = "On Lap";
            Column11.Name = "BestLapNumber";
            Column11.Width = 65;
            Column11.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column11.DataPropertyName = "BestLapNumber";

            Column12.HeaderText = "Last Pit";
            Column12.Name = "LastPit";
            Column12.Width = 75;
            Column12.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold);
            Column12.DataPropertyName = "LastPit";
            Column12.Visible = false;

            return dataGridView;
        }

        private DataGridView BuildFastestLapsViewGrid()
        {
            var dataGridView = new DataGridView();

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

            ConfigureColumn(Column1, "Position", 25);

            ConfigureColumn(Column2, "Driver", 150, "Fastest Laps");

            ConfigureColumn(Column3, "Speed", 75, "M.P.H.");

            return dataGridView;
        }

        private DataGridView BuildBiggestMoversViewGrid()
        {
            var dataGridView = new DataGridView();

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

            ConfigureColumn(Column1, "Position", 25);

            ConfigureColumn(Column2, "Driver", 150, "Biggest Movers");

            ConfigureColumn(Column3, "Change", 75, "Gain");

            return dataGridView;
        }

        private DataGridView BuildBiggestFallersViewGrid()
        {
            var dataGridView = new DataGridView();

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

            ConfigureColumn(Column1, "Position", 25);

            ConfigureColumn(Column2, "Driver", 150, "Biggest Fallers");

            ConfigureColumn(Column3, "Change", 75, "Loss");

            return dataGridView;
        }

        private void ConfigureColumn(
            DataGridViewTextBoxColumn column,
            string propertyName,
            int? width = 125,
            string headerText = "")
        {
            column.HeaderText = headerText;
            column.Name = propertyName;
            column.DataPropertyName = propertyName;

            if (width.HasValue)
            {
                column.Width = width.Value;
                column.Resizable = DataGridViewTriState.False;
            }
            else
            {
                column.Visible = false;
            }
        }

        // get data
        private IList<Series> GetSeriesSchedule(SeriesType seriesType)
        {
            var raceListRepository = Program.Services.GetRequiredService<IRaceListRepository>();

            var raceLists = raceListRepository.GetRaceList();

            switch (seriesType)
            {
                case SeriesType.Trucks:
                    {
                        return raceLists.TruckSeries;
                    }
                case SeriesType.Xfinity:
                    {
                        return raceLists.XfinitySeries;
                    }
                case SeriesType.Cup:
                    {
                        return raceLists.CupSeries;
                    }
                case SeriesType.All:
                    {
                        return raceLists.CupSeries.Concat(raceLists.XfinitySeries).Concat(raceLists.TruckSeries).ToList();
                    }
                default:
                    {
                        throw new ArgumentException($"Unrecognized Series: {seriesType}");
                    }
            }
        }

        private void GetLiveFeedData()
        {
            try
            {
                var liveFeedRepository = Program.Services.GetRequiredService<ILiveFeedRepository>();

                var liveFeed = liveFeedRepository.GetLiveFeed();

                if (liveFeed.TimeOfDayOs == _lastLiveFeedTimestamp)
                    return;

                if (liveFeed.RunType != (int)_viewState)
                {
                    SetViewState((ViewState)liveFeed.RunType);
                }

                DisplayHeaderData(liveFeed);

                DisplayVehicleData(liveFeed);

                _lastLiveFeedTimestamp = liveFeed.TimeOfDayOs;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
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

        // display data
        private void DisplaySeriesSchedule(SeriesType seriesType)
        {
            if (AutoUpdateTimer.Enabled)
                SetAutoUpdateState(false);

            if (_viewState != ViewState.SeriesSchedule)
                SetViewState(ViewState.SeriesSchedule);

            var schedule = GetSeriesSchedule(seriesType);

            _seriesScheduleDataGridView.DataSource = schedule.OrderBy(s => s.date_scheduled).ToList();

            _seriesScheduleDataGridView.SelectionChanged += _seriesScheduleDataGridView_SelectionChanged;
        }

        private void _seriesScheduleDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (_seriesScheduleDataGridView.SelectedRows.Count > 0)
            {
                var selectedSeriesEvent = (Series)_seriesScheduleDataGridView.SelectedRows[0].DataBoundItem;

                Console.WriteLine(selectedSeriesEvent.race_name);

                _eventScheduleDataGridView.DataSource = selectedSeriesEvent.schedule;
            }
            else
            {
                _eventScheduleDataGridView.DataSource = null;
            }
        }

        private void DisplayRawVehicleData()
        {
            if (AutoUpdateTimer.Enabled)
                SetAutoUpdateState(false);

            if (_viewState != ViewState.Info)
                SetViewState(ViewState.Info);

            var liveFeedRepository = rNascar23TestApp.Program.Services.GetRequiredService<ILiveFeedRepository>();

            var liveFeed = liveFeedRepository.GetLiveFeed();

            _genericDataGridView.DataSource = liveFeed.Vehicles.OrderBy(v => v.running_position).ToList();
        }

        private void DisplayRawLiveFeedData()
        {
            if (AutoUpdateTimer.Enabled)
                SetAutoUpdateState(false);

            if (_viewState != ViewState.Info)
                SetViewState(ViewState.Info);

            var liveFeedRepository = rNascar23TestApp.Program.Services.GetRequiredService<ILiveFeedRepository>();

            var liveFeed = liveFeedRepository.GetLiveFeed();

            _genericDataGridView.DataSource = new List<LiveFeed>() { liveFeed };
        }

        private void DisplayVehicleData(LiveFeed liveFeed)
        {
            var raceVehicles = new List<RaceVehicleViewModel>();

            Vehicle lastVehicle = null;
            foreach (var vehicle in liveFeed.Vehicles)
            {
                raceVehicles.Add(new RaceVehicleViewModel()
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

            var fastestLaps = liveFeed.Vehicles.OrderByDescending(v => v.best_lap_speed).Take(10).Select(v => new FastestLapViewModel()
            {
                Driver = v.driver.full_name,
                Speed = Math.Round(v.best_lap_speed, 3).ToString("N3")
            }).ToList();

            for (int i = 0; i < fastestLaps.Count; i++)
            {
                fastestLaps[i].Position = i + 1;
            }

            DisplayFastestLaps(fastestLaps);

            // DisplayBiggestMovers

            var biggestMovers = liveFeed.Vehicles.OrderByDescending(v => v.position_differential_last_10_percent).Take(10).Select(v => new PositionChangeViewModel()
            {
                Driver = v.driver.full_name,
                Change = v.position_differential_last_10_percent
            }).ToList();

            for (int i = 0; i < biggestMovers.Count; i++)
            {
                biggestMovers[i].Position = i + 1;
            }

            DisplayBiggestMovers(biggestMovers);

            var biggestFallers = liveFeed.Vehicles.OrderBy(v => v.position_differential_last_10_percent).Take(10).Select(v => new PositionChangeViewModel()
            {
                Driver = v.driver.full_name,
                Change = v.position_differential_last_10_percent
            }).ToList();

            for (int i = 0; i < biggestFallers.Count; i++)
            {
                biggestFallers[i].Position = i + 1;
            }

            DisplayBiggestFallers(biggestFallers);
        }

        private void DisplayHeaderData(LiveFeed result)
        {
            picStatus.BackColor = result.FlagState == 8 ? Color.Orange :
                result.FlagState == 1 ? Color.LimeGreen :
                result.FlagState == 2 ? Color.Yellow :
                result.FlagState == 3 ? Color.Red :
                result.FlagState == 4 ? Color.White :
                Color.DimGray;

            if (result.RunType == (int)RunType.Race)
            {
                Series seriesRace = null;

                var seriesRaceList = GetSeriesSchedule((SeriesType)result.SeriesId);

                seriesRace = seriesRaceList.FirstOrDefault(s => s.race_id == result.RaceId);

                DisplayEventName(result.RunName, GetSeriesName(result.SeriesId), result.TrackName, seriesRace.stage_1_laps, seriesRace.stage_2_laps, seriesRace.stage_3_laps);

                DisplayRaceLaps(result.LapNumber, result.LapsInRace);

                DisplayStageLaps(result.Stage.Number, result.LapNumber, result.Stage.FinishAtLap, result.Stage.LapsInStage);
            }
            else
            {
                DisplayEventName(result.RunName, GetSeriesName(result.SeriesId), result.TrackName);
            }
        }

        private void DisplayEventName(string runName, string seriesName, string trackName, int? stage1Laps = null, int? stage2Laps = null, int? stage3Laps = null)
        {
            var eventDetails = $"{runName} - {seriesName} - {trackName} ";

            var stageDetails = stage1Laps.HasValue && stage2Laps.HasValue && stage3Laps.HasValue ?
                $"({stage1Laps.Value}/{stage2Laps.Value}/{stage3Laps.Value})" :
                String.Empty;

            lblEventName.Text = $"{eventDetails} {stageDetails}".TrimEnd();
        }

        private void DisplayRaceLaps(int lapNumber, int lapsInRace)
        {
            lblRaceLaps.Text = $"Race: Lap {lapNumber} of {lapsInRace}";
        }

        private void DisplayStageLaps(int stageNumber, int lapNumber, int stageFinishAtLap, int lapsInStage)
        {
            var stageStartLap = stageFinishAtLap - lapsInStage;

            lblStageLaps.Text = $"Stage {stageNumber}: Lap {lapNumber - stageStartLap} of {lapsInStage}";
        }

        IList<FastestLapViewModel> _fastestLaps;
        IList<PositionChangeViewModel> _biggestMovers;
        IList<PositionChangeViewModel> _biggestFallers;

        private void DisplayFastestLaps(IList<FastestLapViewModel> laps)
        {
            _fastestLaps = laps;

            if (_fastestLapsDataGridView.DataSource == null)
                _fastestLapsDataGridView.DataSource = _fastestLaps;
        }

        private void DisplayBiggestMovers(IList<PositionChangeViewModel> biggestMovers)
        {
            _biggestMovers = biggestMovers;

            if (_biggestMoversDataGridView.DataSource == null)
                _biggestMoversDataGridView.DataSource = _biggestMovers;
        }

        private void DisplayBiggestFallers(IList<PositionChangeViewModel> biggestFallers)
        {
            _biggestFallers = biggestFallers;

            if (_biggestFallersDataGridView.DataSource == null)
                _biggestFallersDataGridView.DataSource = _biggestFallers;
        }

        #endregion
    }
}

