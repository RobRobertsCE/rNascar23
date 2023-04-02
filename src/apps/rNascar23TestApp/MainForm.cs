﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Data.Flags.Ports;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.DriverStatistics.Ports;
using rNascar23.Flags.Models;
using rNascar23.LapTimes.Models;
using rNascar23.LapTimes.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.Points.Models;
using rNascar23.Points.Ports;
using rNascar23.Schedules.Models;
using rNascar23.Schedules.Ports;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.Dialogs;
using rNascar23TestApp.Screens;
using rNascar23TestApp.Settings;
using rNascar23TestApp.ViewModels;
using rNascar23TestApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23TestApp
{
    public partial class MainForm : Form
    {
        #region consts

        private const string LogFileName = "rNascar23Log.{0}.txt";

        #endregion

        #region enums

        private enum ViewState
        {
            None,
            Practice,
            Qualifying,
            Race,
            Info,
            SeriesSchedule,
            EventSchedule,
            ScreenDefinition,
            ScheduleView
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

        #endregion

        #region fields

        private DataGridView _genericDataGridView = null;
        private DataGridView _seriesScheduleDataGridView = null;
        private DataGridView _eventScheduleDataGridView = null;

        private ViewState _viewState = ViewState.None;
        private DateTime _lastLiveFeedTimestamp = DateTime.MinValue;
        private ScheduleType _selectedScheduleType = ScheduleType.All;

        private IList<GridSettings> _customGridSettings = null;
        private readonly FormState _formState = new FormState();
        private bool _isFullScreen = false;
        private FormWindowState _windowState = FormWindowState.Normal;

        private LapStateViewModel _lapStates = new LapStateViewModel();

        private IList<ScreenDefinition> _screenDefinitions = new List<ScreenDefinition>();
        private readonly IDictionary<Keys, ToolStripButton> _screenDefinitionShortcutKeys = new Dictionary<Keys, ToolStripButton>();

        private readonly ILogger<MainForm> _logger = null;
        private readonly ILapTimesRepository _lapTimeRepository = null;
        private readonly ILapAveragesRepository _lapAveragesRepository = null;
        private readonly ILiveFeedRepository _liveFeedRepository = null;
        private readonly IDriverStatisticsRepository _driverStatisticsRepository = null;
        private readonly IFlagStateRepository _flagStateRepository = null;
        private readonly ISchedulesRepository _raceScheduleRepository = null;
        private readonly IPointsRepository _pointsRepository = null;
        private readonly IScreenService _screenService = null;
        private readonly ICustomViewSettingsService _customViewSettingsService = null;
        private readonly ICustomGridViewFactory _customGridViewFactory = null;
        private readonly IStyleService _styleService = null;

        #endregion

        #region ctor / load

        public MainForm(
            ILogger<MainForm> logger,
            ILapTimesRepository lapTimeRepository,
            ILapAveragesRepository lapAveragesRepository,
            ILiveFeedRepository liveFeedRepository,
            IDriverStatisticsRepository driverStatisticsRepository,
            IFlagStateRepository flagStateRepository,
            ISchedulesRepository raceScheduleRepository,
            IPointsRepository pointsRepository,
            IScreenService screenService,
            ICustomViewSettingsService customViewSettingsService,
            ICustomGridViewFactory customGridViewFactory,
            IStyleService styleService)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lapTimeRepository = lapTimeRepository ?? throw new ArgumentNullException(nameof(lapTimeRepository));
            _lapAveragesRepository = lapAveragesRepository ?? throw new ArgumentNullException(nameof(lapAveragesRepository));
            _liveFeedRepository = liveFeedRepository ?? throw new ArgumentNullException(nameof(liveFeedRepository));
            _driverStatisticsRepository = driverStatisticsRepository ?? throw new ArgumentNullException(nameof(driverStatisticsRepository));
            _flagStateRepository = flagStateRepository ?? throw new ArgumentNullException(nameof(flagStateRepository));
            _raceScheduleRepository = raceScheduleRepository ?? throw new ArgumentNullException(nameof(raceScheduleRepository));
            _pointsRepository = pointsRepository ?? throw new ArgumentNullException(nameof(pointsRepository));
            _screenService = screenService ?? throw new ArgumentNullException(nameof(screenService));
            _customViewSettingsService = customViewSettingsService ?? throw new ArgumentNullException(nameof(customViewSettingsService));
            _customGridViewFactory = customGridViewFactory ?? throw new ArgumentNullException(nameof(customGridViewFactory));
            _styleService = styleService ?? throw new ArgumentNullException(nameof(styleService));
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                lblEventName.Text = "";

                await SetViewStateAsync(ViewState.None, true);

                ReloadScreenDefinitions();

                await DisplayTodaysScheduleAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error loading main form");
            }
        }

        #endregion

        #region private [ event handlers ]

        // paint
        private void picGreenYelllowLapIndicator_Paint(object sender, PaintEventArgs e)
        {
            if (_lapStates == null || _lapStates.Stage3Laps == 0)
                return;

            e.Graphics.Clear(Color.Black);

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
            Color flagSegmentHotColor = Color.Orange;
            Color flagSegmentColdColor = Color.RoyalBlue;
            Color flagSegmentCheckeredColor = Color.Black;

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

                var segmentColor = lapSegment.FlagState == LapStateViewModel.FlagState.Green ? flagSegmentGreenColor :
                    lapSegment.FlagState == LapStateViewModel.FlagState.Yellow ? flagSegmentYellowColor :
                    lapSegment.FlagState == LapStateViewModel.FlagState.Red ? flagSegmentRedColor :
                    lapSegment.FlagState == LapStateViewModel.FlagState.White ? flagSegmentWhiteColor :
                    lapSegment.FlagState == LapStateViewModel.FlagState.Hot ? flagSegmentHotColor :
                    lapSegment.FlagState == LapStateViewModel.FlagState.Cold ? flagSegmentColdColor :
                    flagSegmentCheckeredColor;

                RectangleF flagSegment = new RectangleF(
                       lapSegmentStartX,
                       stageBlockStartY + verticalPadding,
                       segmentLength,
                       stageBlockHeight - (verticalPadding * 2));

                PictureBox flagSegmentPB = new PictureBox
                {
                    Location = new Point(picGreenYelllowLapIndicator.Location.X + (int)lapSegmentStartX, stageBlockStartY + verticalPadding),
                    Size = new Size((int)segmentLength, stageBlockHeight - (verticalPadding * 2)),
                    BackColor = segmentColor
                };

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
        private async void rawVehicleListToolStripMenuItem_Click(object sender, EventArgs e)
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

        private async void rawLiveFeedDataToolStripMenuItem_Click(object sender, EventArgs e)
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

        private async void rawLoopDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await DisplayLoopDataAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void formattedLiveFeedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await UpdateUiAsync(true);
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

        private void seriesScheduleDataGridView_SelectionChanged(object sender, EventArgs e)
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

        private async void btnRaceView_Click(object sender, EventArgs e)
        {
            try
            {
                await SetViewStateAsync(ViewState.Race);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void btnQualifyingView_Click(object sender, EventArgs e)
        {
            try
            {
                await SetViewStateAsync(ViewState.Qualifying);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void btnPracticeView_Click(object sender, EventArgs e)
        {
            try
            {
                await SetViewStateAsync(ViewState.Practice);
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

                await DisplayScheduleViewAsync(_selectedScheduleType);
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

                await DisplayScheduleViewAsync(_selectedScheduleType);

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

                await DisplayScheduleViewAsync(_selectedScheduleType);
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

                await DisplayScheduleViewAsync(_selectedScheduleType);
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

                await DisplayScheduleViewAsync(_selectedScheduleType);
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
                await DisplayTodaysScheduleAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async Task DisplayTodaysScheduleAsync()
        {
            _selectedScheduleType = ScheduleType.Today;

            await DisplayScheduleViewAsync(_selectedScheduleType);
        }

        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayLogFile();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void backupCustomViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackupCustomViews();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void importCustomViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ImportCustomViews();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void exportCustomViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExportCustomViews();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void backupStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackupStyles();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void importStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ImportStyles();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void exportStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExportStyles();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void backupScreensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BackupScreens();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void importScreensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ImportScreens();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void exportScreensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ExportScreens();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion

        #region private [ read data ]

        private async Task<bool> ReadDataAsync()
        {
            _formState.LiveFeed = await _liveFeedRepository.GetLiveFeedAsync();

            if (_formState.LiveFeed.TimeOfDayOs == _lastLiveFeedTimestamp)
                return false;

            _lastLiveFeedTimestamp = _formState.LiveFeed.TimeOfDayOs;

            if (_formState.CurrentSeriesRace == null || _formState.LiveFeed.RaceId != _formState.CurrentSeriesRace.RaceId)
            {
                _formState.SeriesSchedules = await GetSeriesScheduleAsync((ScheduleType)_formState.LiveFeed.SeriesId);

                var currentRace = _formState.SeriesSchedules.FirstOrDefault(s => s.RaceId == _formState.LiveFeed.RaceId);

                _formState.CurrentSeriesRace = currentRace;// == null ? new SeriesEvent() : currentRace; ;
            }

            _formState.LapTimes = await _lapTimeRepository.GetLapTimeDataAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);
            _formState.FlagStates = await _flagStateRepository.GetFlagStatesAsync();

            _formState.EventStatistics = await _driverStatisticsRepository.GetEventAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

            if (_formState.EventStatistics != null && _formState.EventStatistics.drivers != null)
            {
                foreach (var driverStats in _formState.EventStatistics?.drivers)
                {
                    var liveFeedDriver = _formState.LiveFeed.Vehicles.FirstOrDefault(v => v.driver.DriverId == driverStats.DriverId);

                    if (liveFeedDriver != null)
                        driverStats.DriverName = liveFeedDriver.driver.FullName;
                }
            }

            _formState.LapAverages = await _lapAveragesRepository.GetLapAveragesAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

            _formState.LivePoints = await _pointsRepository.GetDriverPoints(_formState.LiveFeed.RaceId, _formState.LiveFeed.SeriesId);

            _formState.StagePoints = await _pointsRepository.GetStagePoints(_formState.LiveFeed.RaceId, _formState.LiveFeed.SeriesId);

            return true;
        }

        private async Task<IList<SeriesEvent>> GetSeriesScheduleAsync(ScheduleType seriesType)
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
                            Where(s => s.DateScheduled.Date >= firstDayOfThisWeek.Date && s.DateScheduled.Date <= (firstDayOfThisWeek.AddDays(7).Date)).
                            ToList();
                    }
                case ScheduleType.Today:
                    {
                        return raceLists.CupSeries.
                           Concat(raceLists.XfinitySeries).
                           Concat(raceLists.TruckSeries).
                           Where(s => s.Schedule.Any(x => x.StartTimeLocal.Date == DateTime.Now.Date)).
                           ToList();
                    }
                default:
                    {
                        LogError($"Error selecting schedule to read: Unrecognized Series {seriesType}");
                        return new List<SeriesEvent>();
                    }
            }
        }

        private string GetSeriesName(int seriesId)
        {
            return seriesId == 1 ? "Cup Series" :
                seriesId == 2 ? "Xfinity Series" :
                seriesId == 3 ? "Craftsman Truck Series" :
                seriesId == 4 ? "ARCA Menards Series" :
                seriesId == 999 ? "Whelen Modified Tour" :
                "Unknown";
        }

        #endregion

        #region private [ display screens ]

        private void AddGridToPanel(Panel panel, Control grid, DockStyle dock, bool addSplitter = true)
        {
            panel.Controls.Add(grid);
            grid.Dock = dock;
            grid.BringToFront();

            if (addSplitter)
            {
                AddSplitter(panel, dock);
            }
        }

        private void AddSplitter(Panel panel, DockStyle dock)
        {
            var splitter = new Splitter()
            {
                Dock = dock
            };
            panel.Controls.Add(splitter);
            splitter.BringToFront();
        }

        private void LoadLeaderboards(Panel panel, LeaderboardGridView.RunTypes runType)
        {
            LeaderboardGridView leftLeaderboardGridView = new LeaderboardGridView(
              LeaderboardGridView.LeaderboardSides.Left,
              runType);
            panel.Controls.Add(leftLeaderboardGridView);
            leftLeaderboardGridView.Dock = DockStyle.Left;
            leftLeaderboardGridView.BringToFront();
            leftLeaderboardGridView.Width = (int)((panel.Width - 10) / 2);

            var splitter1 = new Splitter()
            {
                Dock = DockStyle.Left
            };
            panel.Controls.Add(splitter1);
            splitter1.BringToFront();

            LeaderboardGridView rightLeaderboardGridView = new LeaderboardGridView(
                LeaderboardGridView.LeaderboardSides.Right,
                runType);
            panel.Controls.Add(rightLeaderboardGridView);
            rightLeaderboardGridView.Dock = DockStyle.Left;
            rightLeaderboardGridView.BringToFront();
            rightLeaderboardGridView.Width = (int)((panel.Width - 10) / 2);

            var splitter2 = new Splitter()
            {
                Dock = DockStyle.Left
            };
            panel.Controls.Add(splitter2);
            splitter2.BringToFront();
        }

        private void LoadDefaultPanels()
        {
            pnlBottom.Dock = DockStyle.Bottom;

            pnlRight.Dock = DockStyle.Right;

            pnlMain.Dock = DockStyle.Fill;

            pnlMain.Visible = true;
            pnlRight.Visible = true;
            pnlBottom.Visible = true;
            pnlHeader.Visible = true;
            pnlHost.Visible = false;
        }

        private void DisplayPracticeScreen()
        {
            UpdateViewStatusLabel("Practice Screen");

            LoadDefaultPanels();

            /*** Main ***/
            LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Practice);

            /*** Right ***/
            AddGridToPanel(pnlRight, new FastestLapsGridView(), DockStyle.Top);
            AddGridToPanel(pnlRight, new DriverPointsGridView(), DockStyle.Top);

            /*** Bottom ***/
            AddGridToPanel(pnlBottom, new FlagsGridView(), DockStyle.Left);

            var settings = UserSettingsService.LoadUserSettings();

            AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Last5Laps, settings.LastNLapsDisplayType), DockStyle.Left);
            AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Last10Laps, settings.LastNLapsDisplayType), DockStyle.Left);
            AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Last15Laps, settings.LastNLapsDisplayType), DockStyle.Left);

            AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Best5Laps, settings.BestNLapsDisplayType), DockStyle.Left);
            AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Best10Laps, settings.BestNLapsDisplayType), DockStyle.Left);
            AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Best15Laps, settings.BestNLapsDisplayType), DockStyle.Left);

            pnlMain.Visible = true;
            pnlRight.Visible = true;
            pnlBottom.Visible = true;
            pnlHost.Visible = false;

            lblRaceLaps.Visible = false;
            lblRaceLaps.Text = "";

            lblStageLaps.Visible = false;
            lblStageLaps.Text = "";

            pnlHeader.Visible = true;
            pnlEventInfo.Visible = true;
            picFlagStatus.Visible = true;
            picGreenYelllowLapIndicator.Visible = false;

            var headerHeight = 0;
            if (pnlEventInfo.Visible) headerHeight += pnlEventInfo.Height + 1;
            if (picFlagStatus.Visible) headerHeight += picFlagStatus.Height + 1;
            if (picGreenYelllowLapIndicator.Visible) headerHeight += picGreenYelllowLapIndicator.Height + 1;

            pnlHeader.Height = headerHeight;
        }

        private void DisplayQualifyingScreen()
        {
            UpdateViewStatusLabel("Qualifying Screen");

            LoadDefaultPanels();

            /*** main panel ***/
            LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Qualifying);

            /*** right panel ***/
            AddGridToPanel(pnlRight, new DriverPointsGridView(), DockStyle.Top);

            pnlMain.Visible = true;
            pnlRight.Visible = true;
            pnlBottom.Visible = true;
            pnlHost.Visible = false;

            lblRaceLaps.Visible = false;
            lblRaceLaps.Text = "";

            lblStageLaps.Visible = false;
            lblStageLaps.Text = "";

            pnlHeader.Visible = true;
            pnlEventInfo.Visible = true;
            picFlagStatus.Visible = true;
            picGreenYelllowLapIndicator.Visible = false;

            var headerHeight = 0;
            if (pnlEventInfo.Visible) headerHeight += pnlEventInfo.Height + 1;
            if (picFlagStatus.Visible) headerHeight += picFlagStatus.Height + 1;
            if (picGreenYelllowLapIndicator.Visible) headerHeight += picGreenYelllowLapIndicator.Height + 1;

            pnlHeader.Height = headerHeight;
        }

        private void DisplayRaceScreen()
        {
            try
            {
                UpdateViewStatusLabel("Race Screen");

                this.SuspendLayout();

                LoadDefaultPanels();

                /*** main panel ***/
                LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Race);

                // right panel
                AddGridToPanel(pnlRight, new FastestLapsGridView(), DockStyle.Top);
                AddGridToPanel(pnlRight, new LapLeadersGridView(), DockStyle.Top);
                //AddGridToPanel(pnlRight, new DriverPointsGridView(), DockStyle.Top);
                AddGridToPanel(pnlRight, new MoversFallersGridView(), DockStyle.Top);

                // bottom panel
                AddGridToPanel(pnlBottom, new FlagsGridView(), DockStyle.Left);

                var settings = UserSettingsService.LoadUserSettings();

                AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Last5Laps, settings.LastNLapsDisplayType), DockStyle.Left);
                AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Last10Laps, settings.LastNLapsDisplayType), DockStyle.Left);
                AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Last15Laps, settings.LastNLapsDisplayType), DockStyle.Left);

                AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Best5Laps, settings.BestNLapsDisplayType), DockStyle.Left);
                AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Best10Laps, settings.BestNLapsDisplayType), DockStyle.Left);
                AddGridToPanel(pnlBottom, new NLapsGridView(NLapsGridView.ViewTypes.Best15Laps, settings.BestNLapsDisplayType), DockStyle.Left);

                pnlMain.Visible = true;
                pnlRight.Visible = true;
                pnlBottom.Visible = true;
                pnlHost.Visible = false;

                lblRaceLaps.Visible = true;
                lblRaceLaps.Text = "";

                lblStageLaps.Visible = true;
                lblStageLaps.Text = "";

                pnlHeader.Visible = true;
                pnlEventInfo.Visible = true;
                picFlagStatus.Visible = true;
                picGreenYelllowLapIndicator.Visible = true;

                var headerHeight = 0;
                if (pnlEventInfo.Visible) headerHeight += pnlEventInfo.Height + 1;
                if (picFlagStatus.Visible) headerHeight += picFlagStatus.Height + 1;
                if (picGreenYelllowLapIndicator.Visible) headerHeight += picGreenYelllowLapIndicator.Height + 1;

                pnlHeader.Height = headerHeight;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        private void DisplayInfoScreen()
        {
            LoadDefaultPanels();

            if (_genericDataGridView != null)
            {
                _genericDataGridView.Dispose();
                _genericDataGridView = null;
            }

            _genericDataGridView = new DataGridView();
            pnlMain.Controls.Add(_genericDataGridView);
            _genericDataGridView.Dock = DockStyle.Fill;

            pnlMain.Visible = true;
            pnlRight.Visible = false;
            pnlBottom.Visible = false;
            pnlHost.Visible = false;

            lblRaceLaps.Visible = false;
            lblRaceLaps.Text = "";

            lblStageLaps.Visible = false;
            lblStageLaps.Text = "";

            pnlHeader.Visible = false;
            pnlEventInfo.Visible = false;
            picFlagStatus.Visible = false;
            picGreenYelllowLapIndicator.Visible = false;

            var headerHeight = 0;
            if (pnlEventInfo.Visible) headerHeight += pnlEventInfo.Height + 1;
            if (picFlagStatus.Visible) headerHeight += picFlagStatus.Height + 1;
            if (picGreenYelllowLapIndicator.Visible) headerHeight += picGreenYelllowLapIndicator.Height + 1;

            pnlHeader.Height = headerHeight;
        }

        private void DisplayScreenDefinitionScreen()
        {
            pnlMain.Visible = false;
            pnlRight.Visible = false;
            pnlBottom.Visible = false;
            pnlHeader.Visible = true;

            pnlHost.Visible = true;
            pnlHost.Dock = DockStyle.Fill;
        }

        private void DisplayScheduleViewScreen()
        {
            pnlMain.Visible = false;
            pnlRight.Visible = false;
            pnlBottom.Visible = false;
            pnlHeader.Visible = false;

            pnlHost.Visible = true;
            pnlHost.Dock = DockStyle.Fill;
        }

        #endregion

        #region private [ display data ]

        private async Task UpdateUiAsync(bool refreshData = true)
        {
            var hasNewData = !refreshData && _formState.LiveFeed != null || await ReadDataAsync();

            if (!hasNewData)
                return;

            if (_formState.LiveFeed == null)
                return;

            List<IGridView> gridViews = new List<IGridView>();

            if (_viewState == ViewState.None)
            {
                await SetViewStateAsync((ViewState)_formState.LiveFeed.RunType);
            }
            else if (_viewState == ViewState.ScreenDefinition)
            {
                foreach (Panel screenPanel in pnlHost.Controls.OfType<Panel>())
                {
                    gridViews.AddRange(screenPanel.Controls.OfType<IGridView>());
                }
            }
            else
            {
                gridViews.AddRange(pnlMain.Controls.OfType<IGridView>());
                gridViews.AddRange(pnlRight.Controls.OfType<IGridView>());
                gridViews.AddRange(pnlBottom.Controls.OfType<IGridView>());
            }

            foreach (IGridView gridView in gridViews.Where(g => g.IsCustomGrid == false))
            {
                switch (gridView.Settings.ApiSource)
                {
                    case ApiSources.DriverStatistics:
                        ((IGridView<rNascar23.DriverStatistics.Models.Driver>)gridView).Data = _formState.EventStatistics.drivers;
                        break;
                    case ApiSources.Flags:
                        ((IGridView<FlagState>)gridView).Data = _formState.FlagStates;
                        break;
                    case ApiSources.LapTimes:
                        ((IGridView<DriverLaps>)gridView).Data = _formState.LapTimes.Drivers;
                        break;
                    case ApiSources.LapTimeData:
                        ((IGridView<LapTimeData>)gridView).Data = new List<LapTimeData>() { _formState.LapTimes};
                        break;
                    case ApiSources.LiveFeed:
                        ((IGridView<LiveFeed>)gridView).Data = new List<LiveFeed>() { _formState.LiveFeed };
                        break;
                    case ApiSources.RaceLists:
                        ((IGridView<SeriesEvent>)gridView).Data = _formState.SeriesSchedules;
                        break;
                    case ApiSources.Vehicles:
                        ((IGridView<Vehicle>)gridView).Data = _formState.LiveFeed.Vehicles;
                        break;
                    case ApiSources.LapAverages:
                        ((IGridView<LapAverages>)gridView).Data = _formState.LapAverages;
                        break;
                    case ApiSources.DriverPoints:
                        ((IGridView<DriverPoints>)gridView).Data = _formState.LivePoints;
                        break;
                    case ApiSources.StagePoints:
                        ((IGridView<rNascar23.Points.Models.Stage>)gridView).Data = GetCurrentStagePoints();
                        break;
                    default:
                        break;
                }
            }

            DisplayHeaderData();
        }

        private IList<rNascar23.Points.Models.Stage> GetCurrentStagePoints()
        {
            if (_formState.LiveFeed == null)
                return new List<rNascar23.Points.Models.Stage>();

            var stage = _formState.StagePoints.FirstOrDefault(s => s.race_id == _formState.LiveFeed.RaceId && s.run_id == _formState.LiveFeed.RunId && s.stage_number == _formState.LiveFeed.Stage.Number);

            if (stage == null)
                return new List<rNascar23.Points.Models.Stage>();
            else
                return new List<rNascar23.Points.Models.Stage>() { stage };
        }

        private async Task DisplayLoopDataAsync()
        {
            UpdateViewStatusLabel("Raw Loop Data");

            if (_viewState != ViewState.Info)
            {
                await SetAutoUpdateStateAsync(false);

                await SetViewStateAsync(ViewState.Info);
            }

            if (_formState.EventStatistics == null)
            {
                await ReadDataAsync();
            }

            _genericDataGridView.DataSource = _formState.EventStatistics?.drivers;
        }

        private void DisplaySeriesScheduleViewState()
        {
            pnlHeader.Visible = false;

            if (_seriesScheduleDataGridView != null)
            {
                _seriesScheduleDataGridView.SelectionChanged -= seriesScheduleDataGridView_SelectionChanged;
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

        private async Task DisplayScheduleViewAsync(ScheduleType scheduleType)
        {
            if (AutoUpdateTimer.Enabled)
                await SetAutoUpdateStateAsync(false);

            UpdateViewStatusLabel("Schedules");

            if (_viewState != ViewState.ScheduleView)
                await SetViewStateAsync(ViewState.ScheduleView);

            var schedule = await GetSeriesScheduleAsync(scheduleType);

            IApiDataView<SeriesEvent> scheduleView = new ScheduleView(scheduleType);

            pnlHost.Controls.Clear();

            pnlHost.Dock = DockStyle.Fill;
            pnlHost.Controls.Add((Control)scheduleView);

            ((Control)scheduleView).Dock = DockStyle.Fill;
            ((Control)scheduleView).BackColor = Color.White;

            scheduleView.Data = schedule;
        }

        private void DisplayEventSchedule()
        {
            if (_seriesScheduleDataGridView.SelectedRows.Count > 0)
            {
                var selectedSeriesEvent = (SeriesEvent)_seriesScheduleDataGridView.SelectedRows[0].DataBoundItem;

                _eventScheduleDataGridView.DataSource = selectedSeriesEvent.Schedule;
            }
            else
            {
                if (_selectedScheduleType == ScheduleType.ThisWeek)
                {
                    Dictionary<string, List<Schedule>> eventSchedule = new Dictionary<string, List<Schedule>>();
                    SeriesEvent seriesEvent = null;

                    foreach (DataGridViewRow seriesScheduleRow in _seriesScheduleDataGridView.Rows)
                    {
                        seriesEvent = (SeriesEvent)seriesScheduleRow.DataBoundItem;

                        List<Schedule> seriesEventSchedule = (List<Schedule>)seriesEvent.Schedule.ToList();

                        eventSchedule.Add(seriesEvent.SeriesName, seriesEventSchedule);
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
                    SeriesEvent seriesEvent = null;

                    foreach (DataGridViewRow seriesScheduleRow in _seriesScheduleDataGridView.Rows)
                    {
                        seriesEvent = (SeriesEvent)seriesScheduleRow.DataBoundItem;

                        List<Schedule> seriesEventSchedule = (List<Schedule>)seriesEvent.Schedule.ToList();

                        eventSchedule.Add(seriesEvent.SeriesName, seriesEventSchedule);
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
            UpdateViewStatusLabel("Raw Vehicle Data");

            if (AutoUpdateTimer.Enabled)
                await SetAutoUpdateStateAsync(false);

            if (_viewState != ViewState.Info)
                await SetViewStateAsync(ViewState.Info);

            var liveFeed = await _liveFeedRepository.GetLiveFeedAsync();

            _genericDataGridView.DataSource = liveFeed.Vehicles.OrderBy(v => v.running_position).ToList();
        }

        private async Task DisplayRawLiveFeedDataAsync()
        {
            UpdateViewStatusLabel("Raw Live Feed Data");

            if (AutoUpdateTimer.Enabled)
                await SetAutoUpdateStateAsync(false);

            if (_viewState != ViewState.Info)
                await SetViewStateAsync(ViewState.Info);

            var liveFeed = await _liveFeedRepository.GetLiveFeedAsync();

            _genericDataGridView.DataSource = new List<LiveFeed>() { liveFeed };
        }

        private void DisplayHeaderData()
        {
            picFlagStatus.BackColor = _formState.LiveFeed.FlagState == 8 ? Color.Orange :
                _formState.LiveFeed.FlagState == 1 ? Color.LimeGreen :
                _formState.LiveFeed.FlagState == 2 ? Color.Yellow :
                _formState.LiveFeed.FlagState == 3 ? Color.Red :
                _formState.LiveFeed.FlagState == 4 ? Color.White :
                _formState.LiveFeed.FlagState == 9 ? Color.RoyalBlue :
                Color.Black;

            if (_formState.LiveFeed.RunType == (int)RunType.Race)
            {
                if (_lapStates == null || _lapStates.Stage1Laps == 0)
                {
                    if (_formState.CurrentSeriesRace != null)
                    {
                        _lapStates = new LapStateViewModel
                        {
                            Stage1Laps = _formState.CurrentSeriesRace.Stage1Laps,
                            Stage2Laps = _formState.CurrentSeriesRace.Stage2Laps,
                            Stage3Laps = _formState.CurrentSeriesRace.Stage3Laps
                        };
                    }
                    else
                    {
                        _lapStates = new LapStateViewModel
                        {
                            Stage1Laps = 0,
                            Stage2Laps = 0,
                            Stage3Laps = _formState.LiveFeed.LapsInRace
                        };
                    }
                }

                DisplayEventName(_formState.LiveFeed.RunName, GetSeriesName(_formState.LiveFeed.SeriesId), _formState.LiveFeed.TrackName, _formState.CurrentSeriesRace?.Stage1Laps, _formState.CurrentSeriesRace?.Stage2Laps, _formState.CurrentSeriesRace?.Stage3Laps);

                DisplayRaceLaps(_formState.LiveFeed.LapNumber, _formState.LiveFeed.LapsInRace);

                if (_lapStates.Stage1Laps > 0 && _lapStates.Stage1Laps > 0)
                {
                    DisplayStageLaps(_formState.LiveFeed.Stage.Number, _formState.LiveFeed.LapNumber, _formState.LiveFeed.Stage.FinishAtLap, _formState.LiveFeed.Stage.LapsInStage);
                }
                else
                {
                    lblStageLaps.Visible = false;
                }
            }
            else
            {
                DisplayEventName(_formState.LiveFeed.RunName, GetSeriesName(_formState.LiveFeed.SeriesId), _formState.LiveFeed.TrackName);
            }

            UpdateGreenYellowLapIndicator();
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
            lblStageLaps.Visible = true;

            var stageStartLap = stageFinishAtLap - lapsInStage;

            lblStageLaps.Text = $"Stage {stageNumber}: Lap {lapNumber - stageStartLap} of {lapsInStage}";
        }

        private void UpdateGreenYellowLapIndicator()
        {
            if (_formState.FlagStates == null)
                return;

            int lap = 0;
            LapStateViewModel.FlagState previousFlagState = LapStateViewModel.FlagState.Green;

            _lapStates.LapSegments.Clear();

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

            // Add one for current flag state
            _lapStates.LapSegments.Add(new LapStateViewModel.LapSegment
            {
                StartLapNumber = lap,
                Laps = _formState.LiveFeed.LapNumber - lap,
                Stage = lap >= _lapStates.Stage1Laps + _lapStates.Stage2Laps ?
                            3 : lap >= _lapStates.Stage1Laps ?
                            2 :
                            1,
                FlagState = previousFlagState
            });

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
            await SetViewStateAsync(ViewState.None);

            await ReadDataAsync();

            var dialog = Program.Services.GetRequiredService<GridSettingsDialog>();

            if (_customGridSettings == null)
            {
                _customGridSettings = _customViewSettingsService.GetCustomViewSettings();
            }

            dialog.ShowDialog();
        }

        private void DisplayLogFile()
        {
            string currentLogFile = String.Format(LogFileName, DateTime.Now.ToString("yyyyMMdd")); ;

            var settings = UserSettingsService.LoadUserSettings();

            string logFileDirectory = settings.LogDirectory;

            string logFilePath = Path.Combine(logFileDirectory, currentLogFile);

            if (!File.Exists(logFilePath))
            {
                _logger.LogInformation($"Log file created {DateTime.Now}");
            }

            Process.Start("notepad.exe", logFilePath);
        }

        #endregion

        #region private [ screen definitions ]

        private void screenEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayScreenEditorDialog();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void DisplayScreenEditorDialog()
        {
            var dialog = Program.Services.GetRequiredService<ScreenEditor>();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ReloadScreenDefinitions();
            }
        }

        private void ReloadScreenDefinitions()
        {
            ClearScreenDefinitionToolbarButtons();

            _screenDefinitions = LoadScreenDefinitionsFromFile();

            AddScreenDefinitionToolbarButtons();
        }

        private void ClearScreenDefinitionToolbarButtons()
        {
            for (int i = toolStrip1.Items.Count - 1; i >= 0; i--)
            {
                var toolbarButtomItem = toolStrip1.Items[i];

                if (toolbarButtomItem.Tag != null && toolbarButtomItem.Tag is ScreenDefinition)
                {
                    toolbarButtomItem.Click -= screenDefinitionButton_Click;

                    toolStrip1.Items.Remove(toolbarButtomItem);

                    toolbarButtomItem.Dispose();
                }
            }
        }

        private void AddScreenDefinitionToolbarButtons()
        {
            _screenDefinitionShortcutKeys.Clear();

            int f = 5;
            foreach (ScreenDefinition screenDefinition in _screenDefinitions.OrderBy(s => s.DisplayIndex))
            {
                // add menu/toolbar item
                ToolStripButton screenDefinitionButton = new ToolStripButton();
                var buttonName = f <= 10 ? $"{screenDefinition.Name} (F{f})" : screenDefinition.Name;
                screenDefinitionButton.Text = buttonName;
                screenDefinitionButton.Tag = screenDefinition;
                screenDefinitionButton.ForeColor = Color.Silver;
                screenDefinitionButton.Click += new EventHandler(screenDefinitionButton_Click);

                toolStrip1.Items.Add(screenDefinitionButton);

                Keys key = Keys.F5;

                switch (f)
                {
                    case 5:
                        key = Keys.F5;
                        break;
                    case 6:
                        key = Keys.F6;
                        break;
                    case 7:
                        key = Keys.F7;
                        break;
                    case 8:
                        key = Keys.F8;
                        break;
                    case 9:
                        key = Keys.F9;
                        break;
                    case 10:
                        key = Keys.F10;
                        break;
                    default:
                        break;
                }

                _screenDefinitionShortcutKeys.Add(key, screenDefinitionButton);

                f++;
            }
        }

        private async void screenDefinitionButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is ToolStripButton screenDefinitionButton && screenDefinitionButton.Tag != null)
                {
                    var screenDefinition = screenDefinitionButton.Tag as ScreenDefinition;

                    await LoadScreenDefinitionAsync(screenDefinition);

                    await UpdateUiAsync();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async Task LoadScreenDefinitionAsync(ScreenDefinition screenDefinition)
        {
            UpdateViewStatusLabel(screenDefinition.Name);

            ClearScreenDefinition();

            await SetViewStateAsync(ViewState.ScreenDefinition);

            pnlHeader.Visible = screenDefinition.DisplayEventTitle ||
                screenDefinition.DisplayFlagStatus ||
                screenDefinition.DisplayGreenYellowLapIndicator;

            pnlEventInfo.Visible = screenDefinition.DisplayEventTitle;
            picFlagStatus.Visible = screenDefinition.DisplayFlagStatus;
            picGreenYelllowLapIndicator.Visible = screenDefinition.DisplayGreenYellowLapIndicator;

            var headerHeight = 0;
            if (pnlEventInfo.Visible) headerHeight += pnlEventInfo.Height + 1;
            if (picFlagStatus.Visible) headerHeight += picFlagStatus.Height + 1;
            if (picGreenYelllowLapIndicator.Visible) headerHeight += picGreenYelllowLapIndicator.Height + 1;

            pnlHeader.Height = headerHeight;

            foreach (ScreenPanel screenPanelDefinition in screenDefinition.Panels.OrderBy(p => p.DisplayIndex))
            {
                Panel screenPanelControl = new Panel()
                {
                    Name = screenPanelDefinition.Name,
                    BorderStyle = BorderStyle.FixedSingle,
                    Dock = screenPanelDefinition.Dock,
                    BackColor = Color.FromArgb(24, 24, 24)
                };

                pnlHost.Controls.Add(screenPanelControl);

                screenPanelControl.BringToFront();

                if (screenPanelDefinition.Size != 0 && (screenPanelDefinition.Dock == DockStyle.Top || screenPanelDefinition.Dock == DockStyle.Bottom))
                {
                    screenPanelControl.Height = (int)(screenPanelDefinition.Size * pnlHost.Height);
                }
                else if (screenPanelDefinition.Size != 0 && (screenPanelDefinition.Dock == DockStyle.Left || screenPanelDefinition.Dock == DockStyle.Right))
                {
                    screenPanelControl.Width = (int)(screenPanelDefinition.Size * pnlHost.Width);
                }

                if (screenPanelDefinition.Dock != DockStyle.None && screenPanelDefinition.Dock != DockStyle.Fill)
                {
                    var splitter = new Splitter()
                    {
                        Dock = screenPanelDefinition.Dock
                    };
                    pnlHost.Controls.Add(splitter);

                    splitter.BringToFront();
                }

                foreach (ScreenPanelGridDefinition gridDefinition in screenPanelDefinition.GridViews)
                {
                    AddGridToScreenPanel(screenPanelControl, gridDefinition, screenDefinition.Style);
                }
            }
        }

        private void AddGridToScreenPanel(Panel targetPanel, ScreenPanelGridDefinition gridDefinition, string screenDefaultStyle = "")
        {
            Control gridViewControl = null;

            if (gridDefinition.GridName.EndsWith("*"))
            {
                // custom grid

                /* Grid settings here have a style, but it is not specific to any screen */
                _customGridSettings = _customViewSettingsService.GetCustomViewSettings();

                var gridSettings = _customGridSettings.FirstOrDefault(g => g.Name == gridDefinition.GridName.Replace("*", "").TrimEnd());

                var customGridView = _customGridViewFactory.GetCustomGridView(gridSettings);

                targetPanel.Controls.Add(customGridView);

                customGridView.Dock = gridDefinition.Dock;
                customGridView.Height = gridDefinition.Height;
                customGridView.Width = gridDefinition.Width;

                customGridView.BringToFront();

                if (gridDefinition.HasSplitter)
                {
                    var gridSplitter = new Splitter()
                    {
                        Dock = gridDefinition.Dock
                    };

                    targetPanel.Controls.Add(gridSplitter);

                    gridSplitter.BringToFront();
                }
            }
            else
            {
                // static grid
                var settings = UserSettingsService.LoadUserSettings();

                switch (gridDefinition.GridName)
                {
                    case "Leaderboard":
                        LoadLeaderboards(targetPanel, LeaderboardGridView.RunTypes.Race);
                        break;

                    case "Fastest Laps":
                        gridViewControl = new FastestLapsGridView();
                        break;
                    case "Driver Points":
                        gridViewControl = new DriverPointsGridView();
                        break;
                    case "Stage Points":
                        gridViewControl = new StagePointsGridView();
                        break;
                    case "Flags":
                        gridViewControl = new FlagsGridView();
                        break;                  
                    case "MoversFallers":
                        gridViewControl = new MoversFallersGridView();
                        break;
                    case "Lap Leaders":
                        gridViewControl = new LapLeadersGridView();
                        break;
                    case "Best 5 Lap Average":
                        gridViewControl = new NLapsGridView(NLapsGridView.ViewTypes.Best5Laps, settings.BestNLapsDisplayType);
                        break;
                    case "Best 10 Lap Average":
                        gridViewControl = new NLapsGridView(NLapsGridView.ViewTypes.Best10Laps, settings.BestNLapsDisplayType);
                        break;
                    case "Best 15 Lap Average":
                        gridViewControl = new NLapsGridView(NLapsGridView.ViewTypes.Best15Laps, settings.BestNLapsDisplayType);
                        break;
                    case "Last 5 Lap Average":
                        gridViewControl = new NLapsGridView(NLapsGridView.ViewTypes.Last5Laps, settings.LastNLapsDisplayType);
                        break;
                    case "Last 10 Lap Average":
                        gridViewControl = new NLapsGridView(NLapsGridView.ViewTypes.Last10Laps, settings.LastNLapsDisplayType);
                        break;
                    case "Last 15 Lap Average":
                        gridViewControl = new NLapsGridView(NLapsGridView.ViewTypes.Last15Laps, settings.LastNLapsDisplayType);
                        break;

                    default:
                        break;
                }

                /* 'gridDefinition' here could be either custom grid or static grid. */

                if (gridDefinition.GridName != "Leaderboard")
                {
                    targetPanel.Controls.Add(gridViewControl);
                    gridViewControl.Height = gridDefinition.Height;
                    gridViewControl.Width = gridDefinition.Width;
                    gridViewControl.Dock = gridDefinition.Dock;

                    gridViewControl.BringToFront();

                    if (gridDefinition.HasSplitter)
                    {
                        var splitter = new Splitter()
                        {
                            Dock = gridViewControl.Dock
                        };
                        targetPanel.Controls.Add(splitter);

                        splitter.BringToFront();
                    }

                    if (!String.IsNullOrEmpty(gridDefinition.Style))
                    {
                        Console.WriteLine($"Loading style {gridDefinition.Style} for {gridDefinition.Name}");

                        var style = _styleService.GetStyle(gridDefinition.Style);

                        GridStyleHelper.ApplyGridStyleSettings(((IGridView)gridViewControl).DataGridView, style);
                    }
                    else
                    {
                        // no grid-specific style, defaulting to screen style.
                        if (!String.IsNullOrEmpty(screenDefaultStyle))
                        {
                            Console.WriteLine($"No grid style defined for {gridDefinition.Name}. Defaulting to screen style {screenDefaultStyle}");

                            var style = _styleService.GetStyle(screenDefaultStyle);

                            GridStyleHelper.ApplyGridStyleSettings(((IGridView)gridViewControl).DataGridView, style);
                        }
                        else
                        {
                            Console.WriteLine($"No grid style or screen style defined for {gridDefinition.Name}");
                        }
                    }

                }
                else
                {
                    foreach (IGridView leaderboardGridView in targetPanel.Controls.OfType<IGridView>())
                    {
                        if (!String.IsNullOrEmpty(gridDefinition.Style))
                        {
                            Console.WriteLine($"Loading style {gridDefinition.Style} for {gridDefinition.Name}");

                            var style = _styleService.GetStyle(gridDefinition.Style);

                            GridStyleHelper.ApplyGridStyleSettings(leaderboardGridView.DataGridView, style);
                        }
                        else
                        {
                            // no grid-specific style, defaulting to screen style.
                            if (!String.IsNullOrEmpty(screenDefaultStyle))
                            {
                                Console.WriteLine($"No grid style defined for {gridDefinition.Name}. Defaulting to screen style {screenDefaultStyle}");

                                var style = _styleService.GetStyle(screenDefaultStyle);

                                GridStyleHelper.ApplyGridStyleSettings(leaderboardGridView.DataGridView, style);
                            }
                            else
                            {
                                Console.WriteLine($"No grid style or screen style defined for {gridDefinition.Name}");
                            }
                        }
                    }
                }
            }
        }

        private void ClearScreenDefinition()
        {
            pnlHost.Controls.Clear();
        }

        private IList<ScreenDefinition> LoadScreenDefinitionsFromFile()
        {
            return _screenService.GetScreenDefinitions();
        }

        #endregion

        #region private [ styles ]

        private void styleEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayStyleEditor();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void DisplayStyleEditor()
        {
            var dialog = Program.Services.GetRequiredService<StyleEditor>();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                // TODO: Apply styles
            }
        }

        #endregion

        #region private [ fullscreen ]

        private async void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F11)
                {
                    GoFullscreen(!_isFullScreen);
                }
                else if (e.KeyCode == Keys.F12)
                {
                    await SetAutoUpdateStateAsync(!AutoUpdateTimer.Enabled);
                }
                else if (e.KeyCode == Keys.F1)
                {
                    await SetViewStateAsync(ViewState.Race);
                }
                else if (e.KeyCode == Keys.F2)
                {
                    await SetViewStateAsync(ViewState.Qualifying);
                }
                else if (e.KeyCode == Keys.F3)
                {
                    await SetViewStateAsync(ViewState.Practice);
                }
                else if (e.KeyCode == Keys.F4)
                {
                    ddbSchedules.ShowDropDown();
                }
                else if (e.KeyCode == Keys.D && e.Modifiers == (Keys.Control | Keys.Shift))
                {
                    DumpState();
                }
                else
                {
                    if (_screenDefinitionShortcutKeys.ContainsKey(e.KeyCode))
                    {
                        var toolstripButton = _screenDefinitionShortcutKeys[e.KeyCode];
                        toolstripButton.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void GoFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                this.statusStrip1.Visible = false;
                this.menuStrip1.Visible = false;
                _windowState = this.WindowState;

                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                Screen myScreen = Screen.FromHandle(this.Handle);
                this.Bounds = myScreen.Bounds;
            }
            else
            {
                this.WindowState = _windowState;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

                this.statusStrip1.Visible = true;
                this.menuStrip1.Visible = true;
            }

            _isFullScreen = fullscreen;
            tsbFullScreen.Checked = _isFullScreen;
        }

        private void tsbFullScreen_Click(object sender, EventArgs e)
        {
            GoFullscreen(!_isFullScreen);
        }

        #endregion

        #region private

        private void LogError(string message)
        {
            _logger.LogError(message);
        }
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

            AutoUpdateTimer.Enabled = isEnabled;

            if (AutoUpdateTimer.Enabled)
            {
                lblAutoUpdateStatus.Text = "Auto-Update On";
                lblAutoUpdateStatus.BackColor = Color.LimeGreen;
                tsbAutoUpdate.BackColor = Color.LimeGreen;
                tsbAutoUpdate.ForeColor = Color.Black;

                await ReadDataAsync();
            }
            else
            {
                lblAutoUpdateStatus.Text = "Auto-Update Off";
                lblAutoUpdateStatus.BackColor = SystemColors.Control;
                tsbAutoUpdate.BackColor = Color.DimGray;
                tsbAutoUpdate.ForeColor = Color.White;
            }
        }

        private async Task SetViewStateAsync(ViewState newViewState)
        {
            await SetViewStateAsync(newViewState, false);
        }
        private async Task SetViewStateAsync(ViewState newViewState, bool forceRefresh = false)
        {
            if (newViewState == _viewState && forceRefresh == false)
                return;

            SetUiView(newViewState);

            _viewState = newViewState;

            if (_viewState != ViewState.None)
                await UpdateUiAsync(false);
        }

        private void UpdateViewStatusLabel(string viewName)
        {
            lblViewState.Text = $"View: {viewName}";
        }

        private void SetUiView(ViewState viewState)
        {
            ClearViewControls();

            switch (viewState)
            {
                case ViewState.None:
                    break;
                case ViewState.Practice:
                    DisplayPracticeScreen();
                    break;
                case ViewState.Qualifying:
                    DisplayQualifyingScreen();
                    break;
                case ViewState.Race:
                    DisplayRaceScreen();
                    break;
                case ViewState.Info:
                    DisplayInfoScreen();
                    break;
                case ViewState.SeriesSchedule:
                    DisplaySeriesScheduleViewState();
                    break;
                case ViewState.EventSchedule:
                    DisplayEventScheduleViewState();
                    break;
                case ViewState.ScreenDefinition:
                    DisplayScreenDefinitionScreen();
                    break;
                case ViewState.ScheduleView:
                    DisplayScheduleViewScreen();
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

        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void tsbAutoUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                await SetAutoUpdateStateAsync(!AutoUpdateTimer.Enabled);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /* backup, import, export */
        private void BackupCustomViews()
        {
            var dialog = JsonFileHelper.CustomViewsBackupFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                File.Copy(CustomViewSettingsService.CustomViewsFile, dialog.FileName);
            }
        }

        private void ImportCustomViews()
        {
            var dialog = JsonFileHelper.CustomViewsImportFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                DisplayMergeDialog(dialog.FileName, ImportExportDialog.DataTypes.CustomViews);
            }
        }

        private void ExportCustomViews()
        {
            var dialog = JsonFileHelper.CustomViewsExportFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                DisplayExportDialog(dialog.FileName, ImportExportDialog.DataTypes.CustomViews);
            }
        }

        private void BackupStyles()
        {
            var dialog = JsonFileHelper.StylesBackupFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                File.Copy(StyleService.GetCustomStylesFilePath(), dialog.FileName);
            }
        }

        private void ImportStyles()
        {
            var dialog = JsonFileHelper.StylesImportFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                DisplayMergeDialog(dialog.FileName, ImportExportDialog.DataTypes.Styles);
            }
        }

        private void ExportStyles()
        {
            var dialog = JsonFileHelper.StylesExportFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                DisplayExportDialog(dialog.FileName, ImportExportDialog.DataTypes.Styles);
            }
        }

        private void BackupScreens()
        {
            var dialog = JsonFileHelper.ScreensBackupFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                File.Copy(ScreenService.GetScreenFilePath(), dialog.FileName);
            }
        }

        private void ImportScreens()
        {
            var dialog = JsonFileHelper.ScreensImportFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                DisplayMergeDialog(dialog.FileName, ImportExportDialog.DataTypes.Screens);
            }
        }

        private void ExportScreens()
        {
            var dialog = JsonFileHelper.ScreensExportFileDialog();

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                DisplayExportDialog(dialog.FileName, ImportExportDialog.DataTypes.Screens);
            }
        }

        private void DisplayMergeDialog(string importFile, ImportExportDialog.DataTypes dataType)
        {
            var dialog = Program.Services.GetRequiredService<ImportExportDialog>();
            dialog.DataType = dataType;
            dialog.ActionType = ImportExportDialog.ActionTypes.Import;
            dialog.ImportFile = importFile;

            dialog.ShowDialog();
        }

        private void DisplayExportDialog(string exportFile, ImportExportDialog.DataTypes dataType)
        {
            var dialog = Program.Services.GetRequiredService<ImportExportDialog>();
            dialog.ExportFile = exportFile;
            dialog.DataType = dataType;
            dialog.ActionType = ImportExportDialog.ActionTypes.Export;

            dialog.ShowDialog();
        }

        private string DumpState()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_formState, Formatting.Indented);

                var fileName = $"Dump {DateTime.Now:yyyy-MM-dd HHmmss.fff}.json";

                var settings = UserSettingsService.LoadUserSettings();

                var dumpFilePath = Path.Combine(settings.LogDirectory, fileName);

                File.WriteAllText(dumpFilePath, json);

                return fileName;
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        #endregion

        #region user settings

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplaySettingsDialog();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void DisplaySettingsDialog()
        {
            var dialog = Program.Services.GetRequiredService<UserSettingsDialog>();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                _logger.LogInformation("User settings updated");
            }
        }

        #endregion
    }
}