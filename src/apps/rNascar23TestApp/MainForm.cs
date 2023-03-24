using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using rNascar23.Data.Flags.Ports;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.DriverStatistics.Models;
using rNascar23.DriverStatistics.Ports;
using rNascar23.Flags.Models;
using rNascar23.LapTimes.Models;
using rNascar23.LapTimes.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.RaceLists.Models;
using rNascar23.RaceLists.Ports;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.Dialogs;
using rNascar23TestApp.ViewModels;
using rNascar23TestApp.Views;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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
            Cup = 1,
            Xfinity,
            Trucks,
            All
        }

        private enum ScheduleType
        {
            Cup = 1,
            Xfinity,
            Trucks,
            All,
            ThisWeek,
            Today
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
        private DataGridView _cautionsDataGridView = null;
        private DataGridView _lapLeadersDataGridView = null;
        private DataGridView _5LapAverageTimeDataGridView = null;
        private DataGridView _10LapAverageTimeDataGridView = null;
        private DataGridView _15LapAverageTimeDataGridView = null;
        private ViewState _viewState = ViewState.None;
        private DateTime _lastLiveFeedTimestamp = DateTime.MinValue;
        private ScheduleType _selectedScheduleType = ScheduleType.All;
        IList<FastestLapViewModel> _fastestLaps;
        IList<PositionChangeViewModel> _biggestMovers;
        IList<PositionChangeViewModel> _biggestFallers;
        private LapStateViewModel _lapStates = new LapStateViewModel();
        private IList<GridSettings> _customGridSettings = null;
        private IList<GridView> _gridViews = null;


        private FormState _formState = new FormState();
        private readonly ILogger<MainForm> _logger = null;
        private readonly ILapTimesRepository _lapTimeRepository = null;
        private readonly ILapAveragesRepository _lapAveragesRepository = null;
        private readonly ILiveFeedRepository _liveFeedRepository = null;
        private readonly IDriverStatisticsRepository _driverStatisticsRepository = null;
        private readonly IFlagStateRepository _flagStateRepository = null;
        private readonly IRaceListRepository _raceScheduleRepository = null;

        #endregion

        #region ctor / load

        public MainForm(
            ILogger<MainForm> logger,
            ILapTimesRepository lapTimeRepository,
            ILapAveragesRepository lapAveragesRepository,
            ILiveFeedRepository liveFeedRepository,
            IDriverStatisticsRepository driverStatisticsRepository,
            IFlagStateRepository flagStateRepository,
            IRaceListRepository raceScheduleRepository)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lapTimeRepository = lapTimeRepository ?? throw new ArgumentNullException(nameof(lapTimeRepository));
            _lapAveragesRepository = lapAveragesRepository ?? throw new ArgumentNullException(nameof(lapAveragesRepository));
            _liveFeedRepository = liveFeedRepository ?? throw new ArgumentNullException(nameof(liveFeedRepository));
            _driverStatisticsRepository = driverStatisticsRepository ?? throw new ArgumentNullException(nameof(driverStatisticsRepository));
            _flagStateRepository = flagStateRepository ?? throw new ArgumentNullException(nameof(flagStateRepository));
            _raceScheduleRepository = raceScheduleRepository ?? throw new ArgumentNullException(nameof(raceScheduleRepository));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewState.None, true);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error loading main form");
            }
        }

        #endregion

        #region private [event handlers]

        // timer
        private async void AutoUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                await UpdateUiAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        // menu items
        private async void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplaySeriesScheduleAsync(ScheduleType.All);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void truckRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplaySeriesScheduleAsync(ScheduleType.Trucks);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void xfinityRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplaySeriesScheduleAsync(ScheduleType.Xfinity);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void cupRacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplaySeriesScheduleAsync(ScheduleType.Cup);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void vehicleListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplayRawVehicleDataAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void liveFeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplayRawLiveFeedDataAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void formattedLiveFeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await UpdateUiAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void driverStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplayDriverStatisticsAsync();
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
                SetAutoUpdateStateAsync(!AutoUpdateTimer.Enabled);
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

        private void _seriesScheduleDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (_selectedScheduleType == ScheduleType.ThisWeek || _selectedScheduleType == ScheduleType.Today)
                    return;

                DisplayEventSchedule();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        // paint
        private void picGreenYelllowLapIndicator_Paint(object sender, PaintEventArgs e)
        {
            if (_lapStates == null || _lapStates.Stage1Laps == 0)
                return;

            e.Graphics.Clear(picGreenYelllowLapIndicator.BackColor);

            for (int i = picGreenYelllowLapIndicator.Controls.Count - 1; i >= 0; i--)
            {
                picGreenYelllowLapIndicator.Controls[i].Dispose();
            }
            picGreenYelllowLapIndicator.Controls.Clear();

            Color stageBlockBackgroundColor = Color.Black;
            Color flagSegmentGreenColor = Color.DarkGreen;
            Color flagSegmentYellowColor = Color.Gold;
            Color flagSegmentRedColor = Color.Red;
            Color flagSegmentWhiteColor = Color.White;
            Color flagSegmentCheckeredColor = Color.Gray;

            int endPadding = 5;
            int stagePadding = 10;
            int verticalPadding = 3;
            int stageBlockStartY = 0;

            int totalPadding = (stagePadding * 2) + (endPadding * 2);
            int totalBlockWidth = picGreenYelllowLapIndicator.Width - totalPadding;
            int totalLaps = _lapStates.Stage1Laps + _lapStates.Stage2Laps + _lapStates.Stage3Laps;
            float firstStageWidth = (float)Math.Round(((double)_lapStates.Stage1Laps / totalLaps) * totalBlockWidth, 0);
            float secondStageWidth = (float)Math.Round(((double)_lapStates.Stage2Laps / totalLaps) * totalBlockWidth, 0);
            float thirdStageWidth = (float)Math.Round(((double)_lapStates.Stage3Laps / totalLaps) * totalBlockWidth, 0);
            float stage1StartX = endPadding;
            float stage2StartX = endPadding + firstStageWidth + stagePadding;
            float stage3StartX = endPadding + firstStageWidth + stagePadding + secondStageWidth + stagePadding;
            int stageBlockHeight = picGreenYelllowLapIndicator.Height;

            RectangleF stage1Block = new RectangleF(
                   stage1StartX,
                   stageBlockStartY,
                   firstStageWidth,
                   stageBlockHeight);

            RectangleF stage2Block = new RectangleF(
                   stage2StartX,
                   stageBlockStartY,
                   secondStageWidth,
                   stageBlockHeight);

            RectangleF stage3Block = new RectangleF(
                 stage3StartX,
                 stageBlockStartY,
                 thirdStageWidth,
                 stageBlockHeight);

            float lapWidth1 = (float)stage1Block.Width / _lapStates.Stage1Laps;
            float lapWidth2 = (float)stage2Block.Width / _lapStates.Stage2Laps;
            float lapWidth3 = (float)stage3Block.Width / _lapStates.Stage3Laps;

            for (int i = 0; i < _lapStates.LapSegments.Count; i++)
            {
                var lapSegment = _lapStates.LapSegments[i];
                float lapWidth = lapSegment.Stage == 1 ? lapWidth1 : lapSegment.Stage == 2 ? lapWidth2 : lapWidth3;
                float segmentOffset = lapSegment.Stage == 1 ? 0 : lapSegment.Stage == 2 ? stagePadding : stagePadding + stagePadding;
                float segmentLength = (float)lapSegment.Laps * lapWidth;
                float segmentStart = ((float)lapSegment.StartLapNumber * lapWidth) + segmentOffset;
                float lapSegmentStartX = endPadding + segmentStart;

                var segmentColor = lapSegment.FlagState == LapStateViewModel.FlagState.Green ?
                    flagSegmentGreenColor : lapSegment.FlagState == LapStateViewModel.FlagState.Yellow ?
                    flagSegmentYellowColor : lapSegment.FlagState == LapStateViewModel.FlagState.Red ?
                    flagSegmentRedColor : lapSegment.FlagState == LapStateViewModel.FlagState.White ?
                    flagSegmentWhiteColor : flagSegmentCheckeredColor;

                RectangleF flagSegment = new RectangleF(
                       lapSegmentStartX,
                       stageBlockStartY + verticalPadding,
                       segmentLength,
                       stageBlockHeight - (verticalPadding * 2));

                PictureBox flagSegmentPB = new PictureBox();
                flagSegmentPB.Location = new Point(picGreenYelllowLapIndicator.Location.X + (int)lapSegmentStartX, stageBlockStartY + verticalPadding);
                flagSegmentPB.Size = new Size((int)segmentLength, stageBlockHeight - (verticalPadding * 2));
                flagSegmentPB.BackColor = segmentColor;

                var flagState = lapSegment.FlagState == LapStateViewModel.FlagState.Green ?
                    "Green" : lapSegment.FlagState == LapStateViewModel.FlagState.Yellow ?
                    "Caution" : lapSegment.FlagState == LapStateViewModel.FlagState.Red ?
                    "Red" : lapSegment.FlagState == LapStateViewModel.FlagState.White ?
                    "White" : "Checkered";

                var startLap = lapSegment.StartLapNumber == 0 ? 1 : lapSegment.StartLapNumber;
                var endLap = lapSegment.Laps == 1 ? lapSegment.StartLapNumber : lapSegment.StartLapNumber + lapSegment.Laps;
                var lapsText = endLap == startLap ? $"Lap {startLap + 1}" : $"Laps {startLap} to {endLap}";

                var cautionDetails = String.Empty;

                if (lapSegment.FlagState == LapStateViewModel.FlagState.Yellow)
                {
                    var flagData = _formState.FlagStates.FirstOrDefault(f => f.LapNumber >= lapSegment.StartLapNumber && f.LapNumber <= lapSegment.StartLapNumber);

                    cautionDetails = flagData == null && (flagData.Comment != null || flagData.Beneficiary != null) ? String.Empty : $"{Environment.NewLine}{flagData.Comment?.ToString()}{Environment.NewLine}Lucky Dog:{flagData.Beneficiary?.ToString().Trim()}";
                }

                toolTip1.SetToolTip(flagSegmentPB, $"{flagState} {lapsText}{cautionDetails}");

                picGreenYelllowLapIndicator.Controls.Add(flagSegmentPB);
            }
        }

        private void btnRaceView_Click(object sender, EventArgs e)
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

        private void btnQualifyingView_Click(object sender, EventArgs e)
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

        private void btnPracticeView_Click(object sender, EventArgs e)
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

        private async void btnCustomGridsView_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayCustomGridsView();

                await SetCustomGridViewDataAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void customViewEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplayGridEditorDialogAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void truckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedScheduleType = ScheduleType.Trucks;
                SetViewState(ViewState.SeriesSchedule, true);
                await DisplaySeriesScheduleAsync(_selectedScheduleType);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void xfinityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedScheduleType = ScheduleType.Xfinity;
                SetViewState(ViewState.SeriesSchedule, true);
                await DisplaySeriesScheduleAsync(_selectedScheduleType);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void cupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedScheduleType = ScheduleType.Cup;
                SetViewState(ViewState.SeriesSchedule, true);
                await DisplaySeriesScheduleAsync(_selectedScheduleType);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void allToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedScheduleType = ScheduleType.All;
                SetViewState(ViewState.SeriesSchedule, true);
                await DisplaySeriesScheduleAsync(_selectedScheduleType);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void thisWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedScheduleType = ScheduleType.ThisWeek;
                SetViewState(ViewState.SeriesSchedule, true);
                await DisplaySeriesScheduleAsync(_selectedScheduleType);

                DisplayEventSchedule();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void todayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _selectedScheduleType = ScheduleType.Today;
                SetViewState(ViewState.SeriesSchedule, true);
                await DisplaySeriesScheduleAsync(_selectedScheduleType);

                DisplayEventSchedule();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion

        #region private [build controls]

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

        private DataGridView BuildCautionsViewGrid()
        {
            var dataGridView = new DataGridView();

            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.RowHeadersVisible = false;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AllowUserToResizeColumns = true;

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            ConfigureColumn(Column1, "LapNumber", 50, "Lap");

            ConfigureColumn(Column2, "Comment", 165, "Caution For");

            ConfigureColumn(Column3, "Beneficiary", 65, "Lucky Dog");

            return dataGridView;
        }

        private DataGridView BuildLapLeadersViewGrid()
        {
            var dataGridView = new DataGridView();

            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.RowHeadersVisible = false;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AllowUserToResizeColumns = true;

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            ConfigureColumn(Column1, "Driver", 200, "Lap Leaders");

            ConfigureColumn(Column2, "Laps", 50, "Laps");

            return dataGridView;
        }

        private DataGridView Build5LapAverageTimeViewGrid()
        {
            return BuildLastNLapAverageViewGrid(5, "Lap Time");
        }
        private DataGridView Build5LapAverageMphViewGrid()
        {
            return BuildLastNLapAverageViewGrid(5, "M.P.H.");
        }
        private DataGridView Build10LapAverageTimeViewGrid()
        {
            return BuildLastNLapAverageViewGrid(10, "Lap Time");
        }
        private DataGridView Build10LapAverageMphViewGrid()
        {
            return BuildLastNLapAverageViewGrid(10, "M.P.H.");
        }
        private DataGridView Build15LapAverageTimeViewGrid()
        {
            return BuildLastNLapAverageViewGrid(15, "Lap Time");
        }
        private DataGridView Build15LapAverageMphViewGrid()
        {
            return BuildLastNLapAverageViewGrid(15, "M.P.H.");
        }
        private DataGridView BuildLastNLapAverageViewGrid(int count, string averageTitle)
        {
            var dataGridView = new DataGridView();

            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.RowHeadersVisible = false;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AllowUserToResizeColumns = true;

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            ConfigureColumn(Column1, "Driver", 190, $"Last {count} Lap Avg");

            ConfigureColumn(Column2, "Average", 75, averageTitle);

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

        #endregion

        #region private [read data]

        private async Task<bool> ReadDataAsync()
        {
            _formState.LiveFeed = await _liveFeedRepository.GetLiveFeedAsync();

            if (_formState.LiveFeed.TimeOfDayOs == _lastLiveFeedTimestamp)
                return false;

            _lastLiveFeedTimestamp = _formState.LiveFeed.TimeOfDayOs;

            if (_formState.CurrentSeriesRace == null || _formState.LiveFeed.RaceId != _formState.CurrentSeriesRace.race_id)
            {
                _formState.SeriesSchedules = await GetSeriesScheduleAsync((ScheduleType)_formState.LiveFeed.SeriesId);

                _formState.CurrentSeriesRace = _formState.SeriesSchedules.FirstOrDefault(s => s.race_id == _formState.LiveFeed.RaceId);
            }

            _formState.LapTimes = await _lapTimeRepository.GetLapTimeDataAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);
            _formState.FlagStates = await _flagStateRepository.GetFlagStatesAsync();

            _formState.EventStatistics = await _driverStatisticsRepository.GetEventAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

            if (_formState.EventStatistics != null && _formState.EventStatistics.drivers != null)
            {
                foreach (var driverStats in _formState.EventStatistics?.drivers)
                {
                    var liveFeedDriver = _formState.LiveFeed.Vehicles.FirstOrDefault(v => v.driver.driver_id == driverStats.driver_id);

                    if (liveFeedDriver != null)
                        driverStats.driver_name = liveFeedDriver.driver.full_name;
                }
            }

            _formState.LapAverages = await _lapAveragesRepository.GetLapAveragesAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

            return true;
        }

        private async Task<IList<Series>> GetSeriesScheduleAsync(ScheduleType seriesType)
        {
            var raceLists = await _raceScheduleRepository.GetRaceListAsync();

            switch (seriesType)
            {
                case ScheduleType.Trucks:
                    {
                        return raceLists.TruckSeries;
                    }
                case ScheduleType.Xfinity:
                    {
                        return raceLists.XfinitySeries;
                    }
                case ScheduleType.Cup:
                    {
                        return raceLists.CupSeries;
                    }
                case ScheduleType.All:
                    {
                        return raceLists.CupSeries.Concat(raceLists.XfinitySeries).Concat(raceLists.TruckSeries).ToList();
                    }
                case ScheduleType.ThisWeek:
                    {
                        var firstDayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

                        return raceLists.CupSeries.
                            Concat(raceLists.XfinitySeries).
                            Concat(raceLists.TruckSeries).
                            Where(s => s.date_scheduled.Date > firstDayOfThisWeek.Date.AddDays(1) && s.date_scheduled.Date.AddDays(1) <= (firstDayOfThisWeek.AddDays(8).Date)).
                            ToList();
                    }
                case ScheduleType.Today:
                    {
                        return raceLists.CupSeries.
                           Concat(raceLists.XfinitySeries).
                           Concat(raceLists.TruckSeries).
                           Where(s => s.schedule.Any(x => x.StartTimeLocal.Date == DateTime.Now.Date)).
                           ToList();
                    }
                default:
                    {
                        throw new ArgumentException($"Unrecognized Series: {seriesType}");
                    }
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

        #endregion

        #region private [display data]

        private async Task UpdateUiAsync()
        {
            var hasNewData = await ReadDataAsync();

            if (!hasNewData)
                return;

            DisplayHeaderData();

            DisplayVehicleData();

            await SetCustomGridViewDataAsync();
        }

        private async Task DisplayDriverStatisticsAsync()
        {
            if (_viewState != ViewState.Info)
            {
                await SetAutoUpdateStateAsync(false);

                SetViewState(ViewState.Info);
            }

            if (_formState.EventStatistics == null)
            {
                await ReadDataAsync();
            }

            _genericDataGridView.DataSource = _formState.EventStatistics?.drivers;
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

            if (_fastestLapsDataGridView != null)
            {
                _fastestLapsDataGridView.Dispose();
                _fastestLapsDataGridView = null;
            }
            _fastestLapsDataGridView = BuildFastestLapsViewGrid();
            pnlRight.Controls.Add(_fastestLapsDataGridView);
            _fastestLapsDataGridView.Height = 275;
            _fastestLapsDataGridView.Dock = DockStyle.Top;

            if (_5LapAverageTimeDataGridView != null)
            {
                _5LapAverageTimeDataGridView.Dispose();
                _5LapAverageTimeDataGridView = null;
            }
            _5LapAverageTimeDataGridView = Build5LapAverageTimeViewGrid();
            pnlBottom.Controls.Add(_5LapAverageTimeDataGridView);
            _5LapAverageTimeDataGridView.Width = 275;
            _5LapAverageTimeDataGridView.Dock = DockStyle.Left;
            _5LapAverageTimeDataGridView.BringToFront();

            if (_10LapAverageTimeDataGridView != null)
            {
                _10LapAverageTimeDataGridView.Dispose();
                _10LapAverageTimeDataGridView = null;
            }
            _10LapAverageTimeDataGridView = Build10LapAverageTimeViewGrid();
            pnlBottom.Controls.Add(_10LapAverageTimeDataGridView);
            _10LapAverageTimeDataGridView.Width = 275;
            _10LapAverageTimeDataGridView.Dock = DockStyle.Left;
            _10LapAverageTimeDataGridView.BringToFront();

            if (_15LapAverageTimeDataGridView != null)
            {
                _15LapAverageTimeDataGridView.Dispose();
                _15LapAverageTimeDataGridView = null;
            }
            _15LapAverageTimeDataGridView = Build15LapAverageTimeViewGrid();
            pnlBottom.Controls.Add(_15LapAverageTimeDataGridView);
            _15LapAverageTimeDataGridView.Width = 275;
            _15LapAverageTimeDataGridView.Dock = DockStyle.Left;
            _15LapAverageTimeDataGridView.BringToFront();

            pnlRight.Visible = true;
            pnlHeader.Visible = true;

            picGreenYelllowLapIndicator.Visible = false;
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

            pnlHeader.Visible = true;
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
            _biggestMoversDataGridView.Width = 265;
            _biggestMoversDataGridView.Dock = DockStyle.Left;

            if (_biggestFallersDataGridView != null)
            {
                _biggestFallersDataGridView.Dispose();
                _biggestFallersDataGridView = null;
            }
            _biggestFallersDataGridView = BuildBiggestFallersViewGrid();
            pnlBottom.Controls.Add(_biggestFallersDataGridView);
            _biggestFallersDataGridView.Width = 265;
            _biggestFallersDataGridView.Dock = DockStyle.Left;

            if (_cautionsDataGridView != null)
            {
                _cautionsDataGridView.Dispose();
                _cautionsDataGridView = null;
            }
            _cautionsDataGridView = BuildCautionsViewGrid();
            pnlBottom.Controls.Add(_cautionsDataGridView);
            _cautionsDataGridView.Width = 290;
            _cautionsDataGridView.Dock = DockStyle.Left;
            _cautionsDataGridView.BringToFront();

            if (_lapLeadersDataGridView != null)
            {
                _lapLeadersDataGridView.Dispose();
                _lapLeadersDataGridView = null;
            }
            _lapLeadersDataGridView = BuildLapLeadersViewGrid();
            pnlBottom.Controls.Add(_lapLeadersDataGridView);
            _lapLeadersDataGridView.Width = 275;
            _lapLeadersDataGridView.Dock = DockStyle.Left;
            _lapLeadersDataGridView.BringToFront();

            if (_5LapAverageTimeDataGridView != null)
            {
                _5LapAverageTimeDataGridView.Dispose();
                _5LapAverageTimeDataGridView = null;
            }
            _5LapAverageTimeDataGridView = Build5LapAverageTimeViewGrid();
            pnlBottom.Controls.Add(_5LapAverageTimeDataGridView);
            _5LapAverageTimeDataGridView.Width = 275;
            _5LapAverageTimeDataGridView.Dock = DockStyle.Left;
            _5LapAverageTimeDataGridView.BringToFront();

            if (_10LapAverageTimeDataGridView != null)
            {
                _10LapAverageTimeDataGridView.Dispose();
                _10LapAverageTimeDataGridView = null;
            }
            _10LapAverageTimeDataGridView = Build10LapAverageTimeViewGrid();
            pnlBottom.Controls.Add(_10LapAverageTimeDataGridView);
            _10LapAverageTimeDataGridView.Width = 275;
            _10LapAverageTimeDataGridView.Dock = DockStyle.Left;
            _10LapAverageTimeDataGridView.BringToFront();

            if (_15LapAverageTimeDataGridView != null)
            {
                _15LapAverageTimeDataGridView.Dispose();
                _15LapAverageTimeDataGridView = null;
            }
            _15LapAverageTimeDataGridView = Build15LapAverageTimeViewGrid();
            pnlBottom.Controls.Add(_15LapAverageTimeDataGridView);
            _15LapAverageTimeDataGridView.Width = 275;
            _15LapAverageTimeDataGridView.Dock = DockStyle.Left;
            _15LapAverageTimeDataGridView.BringToFront();

            lblRaceLaps.Visible = true;
            lblRaceLaps.Text = "-";

            lblStageLaps.Visible = true;
            lblStageLaps.Text = "-";

            picGreenYelllowLapIndicator.Visible = true;

            pnlHeader.Visible = true;
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

        private async Task DisplaySeriesScheduleAsync(ScheduleType seriesType)
        {
            if (AutoUpdateTimer.Enabled)
                await SetAutoUpdateStateAsync(false);

            if (_viewState != ViewState.SeriesSchedule)
                SetViewState(ViewState.SeriesSchedule);

            var schedule = await GetSeriesScheduleAsync(seriesType);

            _seriesScheduleDataGridView.DataSource = schedule.OrderBy(s => s.date_scheduled).ToList();

            _seriesScheduleDataGridView.SelectionChanged += _seriesScheduleDataGridView_SelectionChanged;

            foreach (DataGridViewRow row in _seriesScheduleDataGridView.Rows)
            {
                // event complete
                if (row.Cells[43].Value != null && !String.IsNullOrEmpty(row.Cells[43].Value.ToString()))
                {
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void DisplayEventSchedule()
        {
            if (_seriesScheduleDataGridView.SelectedRows.Count > 0)
            {
                var selectedSeriesEvent = (Series)_seriesScheduleDataGridView.SelectedRows[0].DataBoundItem;

                Console.WriteLine(selectedSeriesEvent.race_name);

                _eventScheduleDataGridView.DataSource = selectedSeriesEvent.schedule;
            }
            else
            {
                if (_selectedScheduleType == ScheduleType.ThisWeek)
                {
                    Dictionary<string, List<Schedule>> eventSchedule = new Dictionary<string, List<Schedule>>();
                    Series series = null;

                    foreach (DataGridViewRow seriesScheduleRow in _seriesScheduleDataGridView.Rows)
                    {
                        series = (Series)seriesScheduleRow.DataBoundItem;

                        List<Schedule> seriesEventSchedule = (List<Schedule>)series.schedule.ToList();

                        eventSchedule.Add(series.SeriesName, seriesEventSchedule);
                    }

                    var seriesEventActivities = new List<SeriesEventScheduleViewModel>();

                    foreach (KeyValuePair<string, List<Schedule>> item in eventSchedule)
                    {
                        seriesEventActivities.AddRange(item.Value.Select(x => new SeriesEventScheduleViewModel
                        {
                            Series = item.Key,
                            Activity = x.EventName,
                            Notes = x.Notes,
                            StartTime = x.StartTimeLocal,
                            Description = x.Description
                        }));
                    }

                    _eventScheduleDataGridView.DataSource = seriesEventActivities.OrderBy(x => x.StartTime).ToList();
                }
                else if (_selectedScheduleType == ScheduleType.Today)
                {
                    Dictionary<string, List<Schedule>> eventSchedule = new Dictionary<string, List<Schedule>>();
                    Series series = null;

                    foreach (DataGridViewRow seriesScheduleRow in _seriesScheduleDataGridView.Rows)
                    {
                        series = (Series)seriesScheduleRow.DataBoundItem;

                        List<Schedule> seriesEventSchedule = (List<Schedule>)series.schedule.ToList();

                        eventSchedule.Add(series.SeriesName, seriesEventSchedule);
                    }

                    var seriesEventActivities = new List<SeriesEventScheduleViewModel>();

                    foreach (KeyValuePair<string, List<Schedule>> item in eventSchedule)
                    {
                        seriesEventActivities.AddRange(item.Value.
                            Where(x => x.StartTimeLocal.Date == DateTime.Now.Date).
                            Select(x => new SeriesEventScheduleViewModel
                            {
                                Series = item.Key,
                                Activity = x.EventName,
                                Notes = x.Notes,
                                StartTime = x.StartTimeLocal,
                                Description = x.Description
                            }));
                    }

                    _eventScheduleDataGridView.DataSource = seriesEventActivities.OrderBy(x => x.StartTime).ToList();
                }
                else
                {
                    _eventScheduleDataGridView.DataSource = null;
                }
            }

            foreach (DataGridViewRow row in _eventScheduleDataGridView.Rows)
            {
                if (_selectedScheduleType == ScheduleType.Today || _selectedScheduleType == ScheduleType.Today)
                {
                    // event complete
                    if (row.Cells[3].Value != null && ((DateTime)row.Cells[3].Value) < DateTime.Now)
                    {
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                    }
                    else
                    {
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                else
                {
                    // event complete
                    if (row.Cells[3].Value != null && ((DateTime)row.Cells[3].Value) < DateTime.Now)
                    {
                        row.DefaultCellStyle.ForeColor = Color.DarkGray;
                    }
                    else
                    {
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }

            _eventScheduleDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private async Task DisplayRawVehicleDataAsync()
        {
            if (AutoUpdateTimer.Enabled)
                SetAutoUpdateStateAsync(false);

            if (_viewState != ViewState.Info)
                SetViewState(ViewState.Info);

            var liveFeed = await _liveFeedRepository.GetLiveFeedAsync();

            _genericDataGridView.DataSource = liveFeed.Vehicles.OrderBy(v => v.running_position).ToList();
        }

        private async Task DisplayRawLiveFeedDataAsync()
        {
            if (AutoUpdateTimer.Enabled)
                await SetAutoUpdateStateAsync(false);

            if (_viewState != ViewState.Info)
                SetViewState(ViewState.Info);

            var liveFeed = await _liveFeedRepository.GetLiveFeedAsync();

            _genericDataGridView.DataSource = new List<LiveFeed>() { liveFeed };
        }

        private void DisplayVehicleData()
        {
            var raceVehicles = new List<RaceVehicleViewModel>();

            Vehicle lastVehicle = null;
            foreach (var vehicle in _formState.LiveFeed.Vehicles)
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

            var bestLapTime = raceVehicles.OrderBy(v => v.LastLap).FirstOrDefault()?.LastLap;

            try
            {
                if (_leftRaceDataGridView != null)
                {
                    _leftRaceDataGridView.SuspendLayout();

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

                        if (bestLapTime.HasValue)
                        {
                            // Fastest this lap.
                            if ((float)row.Cells[9].Value == bestLapTime && bestLapTime > 0)
                            {
                                row.Cells[9].Style.BackColor = Color.LimeGreen;

                                // Best lap for driver for the race
                                if ((float)row.Cells[9].Value == (float)row.Cells[8].Value)
                                {
                                    row.Cells[8].Style.BackColor = Color.LimeGreen;
                                }
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
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (_leftRaceDataGridView != null)
                    _leftRaceDataGridView.ResumeLayout(false);
            }

            try
            {
                if (_rightRaceDataGridView != null)
                {
                    _rightRaceDataGridView.SuspendLayout();

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

                        if (bestLapTime.HasValue)
                        {
                            // Fastest this lap.
                            if ((float)row.Cells[9].Value == bestLapTime && bestLapTime > 0)
                            {
                                row.Cells[9].Style.BackColor = Color.LimeGreen;

                                if ((float)row.Cells[9].Value == (float)row.Cells[8].Value)
                                {
                                    row.Cells[8].Style.BackColor = Color.LimeGreen;
                                }
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
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (_rightRaceDataGridView != null)
                    _rightRaceDataGridView.ResumeLayout(false);
            }

            var fastestLaps = _formState.LiveFeed.Vehicles.OrderByDescending(v => v.best_lap_speed).Take(10).Select(v => new FastestLapViewModel()
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

            var biggestMovers = _formState.LiveFeed.Vehicles.OrderByDescending(v => v.position_differential_last_10_percent).Take(10).Select(v => new PositionChangeViewModel()
            {
                Driver = v.driver.full_name,
                Change = v.position_differential_last_10_percent
            }).ToList();

            for (int i = 0; i < biggestMovers.Count; i++)
            {
                biggestMovers[i].Position = i + 1;
            }

            DisplayBiggestMovers(biggestMovers);

            var biggestFallers = _formState.LiveFeed.Vehicles.OrderBy(v => v.position_differential_last_10_percent).Take(10).Select(v => new PositionChangeViewModel()
            {
                Driver = v.driver.full_name,
                Change = v.position_differential_last_10_percent
            }).ToList();

            for (int i = 0; i < biggestFallers.Count; i++)
            {
                biggestFallers[i].Position = i + 1;
            }

            DisplayBiggestFallers(biggestFallers);

            UpdateGreenYellowLapIndicator();

            DisplayCautionsList();

            DisplayLapLeadersList();

            DisplayBestNLapAverages();

            //DisplayLastNLapAverages();
        }

        private void DisplayLastNLapAverages()
        {
            if (_formState.CurrentSeriesRace == null)
                return;

            if (_formState.LapTimes == null)
                return;

            // 5 lap avg time
            var last5LapTimeAverages = _formState.LapTimes.
                Drivers.
                OrderBy(d => d.AverageTimeLast5Laps().GetValueOrDefault(999)).
                Take(10).
                Select(d => new LapAverageViewModel()
                {
                    Driver = d.FullName,
                    Average = (float)Math.Round(d.AverageTimeLast5Laps().GetValueOrDefault(999), 3)
                }).
                ToList();

            _5LapAverageTimeDataGridView.DataSource = last5LapTimeAverages;

            // 10 lap avg time
            var last10LapTimeAverages = _formState.LapTimes.
               Drivers.
               OrderBy(d => d.AverageTimeLast10Laps().GetValueOrDefault(999)).
               Take(10).
               Select(d => new LapAverageViewModel()
               {
                   Driver = d.FullName,
                   Average = (float)Math.Round(d.AverageTimeLast10Laps().GetValueOrDefault(999), 3)
               }).
               ToList();

            _10LapAverageTimeDataGridView.DataSource = last10LapTimeAverages;

            // 15 lap avg time
            var last15LapTimeAverages = _formState.LapTimes.
               Drivers.
               OrderBy(d => d.AverageTimeLast15Laps().GetValueOrDefault(999)).
               Take(10).
               Select(d => new LapAverageViewModel()
               {
                   Driver = d.FullName,
                   Average = (float)Math.Round(d.AverageTimeLast15Laps().GetValueOrDefault(999), 3)
               }).
               ToList();

            _15LapAverageTimeDataGridView.DataSource = last15LapTimeAverages;
        }

        private void DisplayBestNLapAverages()
        {
            if (_formState.CurrentSeriesRace == null)
                return;

            if (_formState.LapTimes == null)
                return;

            // 5 lap avg Speed
            var best5LapTimeAverages = _formState.LapTimes.
                Drivers.
                OrderByDescending(d => d.Best5LapAverageSpeed().GetValueOrDefault(-1)).
                Take(10).
                Select(d => new LapAverageViewModel()
                {
                    Driver = d.FullName,
                    Average = (float)Math.Round(d.Best5LapAverageSpeed().GetValueOrDefault(-1), 3)
                }).
                ToList();

            _5LapAverageTimeDataGridView.DataSource = best5LapTimeAverages;

            // 10 lap avg Speed
            var best10LapTimeAverages = _formState.LapTimes.
               Drivers.
               OrderByDescending(d => d.Best10LapAverageSpeed().GetValueOrDefault(-1)).
               Take(10).
               Select(d => new LapAverageViewModel()
               {
                   Driver = d.FullName,
                   Average = (float)Math.Round(d.Best10LapAverageSpeed().GetValueOrDefault(-1), 3)
               }).
               ToList();

            _10LapAverageTimeDataGridView.DataSource = best10LapTimeAverages;

            // 15 lap avg Speed
            var best15LapTimeAverages = _formState.LapTimes.
               Drivers.
               OrderByDescending(d => d.Best15LapAverageSpeed().GetValueOrDefault(-1)).
               Take(10).
               Select(d => new LapAverageViewModel()
               {
                   Driver = d.FullName,
                   Average = (float)Math.Round(d.Best15LapAverageSpeed().GetValueOrDefault(-1), 3)
               }).
               ToList();

            _15LapAverageTimeDataGridView.DataSource = best15LapTimeAverages;
        }

        private void DisplayHeaderData()
        {
            picStatus.BackColor = _formState.LiveFeed.FlagState == 8 ? Color.Orange :
                _formState.LiveFeed.FlagState == 1 ? Color.LimeGreen :
                _formState.LiveFeed.FlagState == 2 ? Color.Yellow :
                _formState.LiveFeed.FlagState == 3 ? Color.Red :
                _formState.LiveFeed.FlagState == 4 ? Color.White :
                Color.DimGray;

            if (_formState.LiveFeed.RunType == (int)RunType.Race)
            {
                if (_lapStates == null || _lapStates.Stage1Laps == 0)
                {
                    _lapStates = new LapStateViewModel();
                    _lapStates.Stage1Laps = _formState.CurrentSeriesRace.stage_1_laps;
                    _lapStates.Stage2Laps = _formState.CurrentSeriesRace.stage_2_laps;
                    _lapStates.Stage3Laps = _formState.CurrentSeriesRace.stage_3_laps;
                }

                DisplayEventName(_formState.LiveFeed.RunName, GetSeriesName(_formState.LiveFeed.SeriesId), _formState.LiveFeed.TrackName, _formState.CurrentSeriesRace.stage_1_laps, _formState.CurrentSeriesRace.stage_2_laps, _formState.CurrentSeriesRace.stage_3_laps);

                DisplayRaceLaps(_formState.LiveFeed.LapNumber, _formState.LiveFeed.LapsInRace);

                DisplayStageLaps(_formState.LiveFeed.Stage.Number, _formState.LiveFeed.LapNumber, _formState.LiveFeed.Stage.FinishAtLap, _formState.LiveFeed.Stage.LapsInStage);
            }
            else
            {
                DisplayEventName(_formState.LiveFeed.RunName, GetSeriesName(_formState.LiveFeed.SeriesId), _formState.LiveFeed.TrackName);
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

        private void DisplayFastestLaps(IList<FastestLapViewModel> laps)
        {
            _fastestLaps = laps;

            if (_fastestLapsDataGridView != null && _fastestLapsDataGridView.DataSource == null)
                _fastestLapsDataGridView.DataSource = _fastestLaps;
        }

        private void DisplayBiggestMovers(IList<PositionChangeViewModel> biggestMovers)
        {
            _biggestMovers = biggestMovers;

            if (_biggestMoversDataGridView != null && _biggestMoversDataGridView.DataSource == null)
                _biggestMoversDataGridView.DataSource = _biggestMovers;
        }

        private void DisplayBiggestFallers(IList<PositionChangeViewModel> biggestFallers)
        {
            _biggestFallers = biggestFallers;

            if (_biggestFallersDataGridView != null && _biggestFallersDataGridView.DataSource == null)
                _biggestFallersDataGridView.DataSource = _biggestFallers;
        }

        private void DisplayCautionsList()
        {
            if (_cautionsDataGridView == null)
                return;

            IList<CautionFlagViewModel> cautions = new List<CautionFlagViewModel>();

            foreach (var item in _formState.FlagStates.Where(f => f.State == 2).OrderBy(f => f.LapNumber))
            {
                var caution = new CautionFlagViewModel()
                {
                    LapNumber = item.LapNumber,
                    Comment = item.Comment,
                    Beneficiary = item.Beneficiary,
                };

                cautions.Add(caution);
            }

            _cautionsDataGridView.DataSource = cautions;
        }

        private void DisplayLapLeadersList()
        {
            if (_lapLeadersDataGridView == null)
                return;

            IList<LapLeaderViewModel> lapLeaders = new List<LapLeaderViewModel>();

            foreach (var lapLedLeader in _formState.LiveFeed.Vehicles.Where(v => v.laps_led.Length > 0))
            {
                var lapLeader = new LapLeaderViewModel()
                {
                    Driver = lapLedLeader.driver.full_name,
                    Laps = lapLedLeader.laps_led.Sum(l => l.end_lap - l.start_lap) + 1
                };

                lapLeaders.Add(lapLeader);
            }

            _lapLeadersDataGridView.DataSource = lapLeaders.OrderByDescending(l => l.Laps).ToList();
        }

        private void UpdateGreenYellowLapIndicator()
        {
            int lap = 0;
            LapStateViewModel.FlagState previousFlagState = LapStateViewModel.FlagState.Green;

            for (int i = 0; i < _formState.FlagStates.Count; i++)
            {
                if (_formState.FlagStates[i].State != (int)previousFlagState)
                {
                    // flag has changed
                    if ((_formState.FlagStates[i].LapNumber - lap) > 0)
                    {
                        // race has started   
                        var newLapSegment = new LapStateViewModel.LapSegment
                        {
                            StartLapNumber = lap,
                            Laps = _formState.FlagStates[i].LapNumber - lap,
                            Stage = lap >= _lapStates.Stage1Laps + _lapStates.Stage2Laps ?
                            3 : lap >= _lapStates.Stage1Laps ?
                            2 :
                            1,
                            FlagState = previousFlagState
                        };

                        _lapStates.LapSegments.Add(newLapSegment);

                        previousFlagState = (LapStateViewModel.FlagState)_formState.FlagStates[i].State;
                        lap = _formState.FlagStates[i].LapNumber;
                    }
                }
            }

            if (lap == (_lapStates.Stage1Laps + _lapStates.Stage2Laps + _lapStates.Stage3Laps - 1))
            {
                var lastLapSegment = new LapStateViewModel.LapSegment
                {
                    StartLapNumber = lap,
                    Laps = 1,
                    Stage = 3,
                    FlagState = LapStateViewModel.FlagState.Checkered
                };

                _lapStates.LapSegments.Add(lastLapSegment);
            }

            picGreenYelllowLapIndicator.Invalidate();
        }

        private async Task DisplayGridEditorDialogAsync()
        {
            SetViewState(ViewState.None);

            await ReadDataAsync();

            var dialog = Program.Services.GetRequiredService<GridSettingsDialog>();

            if (_customGridSettings == null)
            {
                var service = Program.Services.GetRequiredService<CustomViewSettingsService>();
                _customGridSettings = service.GetCustomViewSettings();
            }

            dialog.ShowDialog();
        }

        private void DisplayCustomGridsView()
        {
            var service = Program.Services.GetRequiredService<CustomViewSettingsService>();
            _customGridSettings = service.GetCustomViewSettings();

            var factory = Program.Services.GetRequiredService<CustomGridViewFactory>();

            _gridViews = factory.GetCustomGridViews(_customGridSettings);

            SetViewState(ViewState.None, true);

            foreach (var customGridView in _gridViews.OrderBy(g => g.Settings.Location).ThenBy(g => g.Settings.DisplayOrder))
            {
                Panel selectedPanel = null;
                switch (customGridView.Settings.Location)
                {
                    case GridLocations.Main:
                        selectedPanel = pnlMain;
                        break;
                    case GridLocations.Right:
                        selectedPanel = pnlRight;
                        break;
                    case GridLocations.Bottom:
                        selectedPanel = pnlBottom;
                        break;
                    default:
                        break;
                }

                selectedPanel.Controls.Add(customGridView);
                customGridView.Dock = DockStyle.Left;
                customGridView.BringToFront();
                var gridSplitter = new Splitter()
                {
                    Dock = DockStyle.Left
                };
                selectedPanel.Controls.Add(gridSplitter);
                gridSplitter.BringToFront();
            }
        }

        private async Task SetCustomGridViewDataAsync()
        {
            if (_gridViews == null || _gridViews.Count == 0)
                return;

            if (_formState.LiveFeed == null)
                await ReadDataAsync();

            foreach (GridView gridView in _gridViews)
            {
                switch (gridView.Settings.ApiSource)
                {
                    case ApiSources.DriverStatistics:
                        if (_formState.EventStatistics != null && _formState.EventStatistics.drivers != null)
                        {
                            var driversDataSource = new DataSource<rNascar23.DriverStatistics.Models.Driver>()
                            {
                                Values = _formState.EventStatistics.drivers
                            };
                            gridView.SetDataSource(driversDataSource);
                        }
                        break;
                    case ApiSources.Flags:
                        var flagsDataSource = new DataSource<FlagState>()
                        {
                            Values = _formState.FlagStates
                        };
                        gridView.SetDataSource(flagsDataSource);
                        break;
                    case ApiSources.LapTimes:
                        var lapsDataSource = new DataSource<LapDetails>()
                        {
                            Values = _formState.LapTimes.Drivers.SelectMany(d => d.Laps).ToList()
                        };
                        gridView.SetDataSource(lapsDataSource);
                        break;

                    case ApiSources.LapAverages:
                        var lapAveragesDataSource = new DataSource<LapAverages>()
                        {
                            Values = _formState.LapAverages.ToList()
                        };
                        gridView.SetDataSource(lapAveragesDataSource);
                        break;
                    case ApiSources.LiveFeed:
                        var liveFeedDataSource = new DataSource<LiveFeed>()
                        {
                            Values = new List<LiveFeed>() { _formState.LiveFeed }
                        };
                        gridView.SetDataSource(liveFeedDataSource);
                        break;
                    case ApiSources.RaceLists:
                        var schedulesDataSource = new DataSource<Series>()
                        {
                            Values = _formState.SeriesSchedules
                        };
                        gridView.SetDataSource(schedulesDataSource);
                        break;
                    case ApiSources.Vehicles:
                        var vehicleDataSource = new DataSource<Vehicle>()
                        {
                            Values = _formState.LiveFeed.Vehicles
                        };
                        gridView.SetDataSource(vehicleDataSource);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region private

        private void ExceptionHandler(Exception ex)
        {
            ExceptionHandler(ex, String.Empty, true);
        }
        private void ExceptionHandler(Exception ex, string message = "")
        {
            ExceptionHandler(ex, message, true);
        }
        private void ExceptionHandler(Exception ex, string message = "", bool logMessage = false)
        {
            MessageBox.Show(ex.Message);
            if (logMessage)
            {
                string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

                _logger.LogError(ex, errorMessage);
            }
        }

        private async Task SetAutoUpdateStateAsync(bool isEnabled)
        {
            if (AutoUpdateTimer.Enabled == isEnabled)
                return;

            autoUpdateToolStripMenuItem.Checked = isEnabled;

            AutoUpdateTimer.Enabled = isEnabled;

            if (AutoUpdateTimer.Enabled)
            {
                lblAutoUpdateStatus.Text = "Auto-Update On";
                lblAutoUpdateStatus.BackColor = Color.LimeGreen;

                await ReadDataAsync();
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

            foreach (Control existingControl in pnlRight.Controls)
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

            for (int i = picGreenYelllowLapIndicator.Controls.Count - 1; i >= 0; i--)
            {
                picGreenYelllowLapIndicator.Controls[i].Dispose();
            }
            picGreenYelllowLapIndicator.Controls.Clear();

            lblRaceLaps.Visible = false;
            lblRaceLaps.Text = "-";

            lblStageLaps.Visible = false;
            lblStageLaps.Text = "-";

            picGreenYelllowLapIndicator.Visible = false;

            lblEventName.Text = "-";
            pnlHeader.Visible = false;
        }

        #endregion

        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}