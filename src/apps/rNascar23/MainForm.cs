using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using rNascar23.Common;
using rNascar23.Configuration;
using rNascar23.CustomViews;
using rNascar23.Data.Flags.Ports;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.Dialogs;
using rNascar23.Flags.Models;
using rNascar23.LapTimes.Models;
using rNascar23.LapTimes.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.LiveFeeds.Ports;
using rNascar23.Logic;
using rNascar23.LoopData.Ports;
using rNascar23.PitStops.Ports;
using rNascar23.Points.Models;
using rNascar23.Points.Ports;
using rNascar23.Replay;
using rNascar23.Schedules.Models;
using rNascar23.Schedules.Ports;
using rNascar23.Screens;
using rNascar23.ViewModels;
using rNascar23.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using PitStop = rNascar23.PitStops.Models.PitStop;

namespace rNascar23
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
            ScheduleView,
            PitStops
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

        private EventReplay _eventReplay = null;
        private int _replayFrameIndex = 0;
        private int _replaySpeed = 1;
        private IList<GridSettings> _customGridSettings = null;
        private FormState _formState = new FormState();
        private bool _isFullScreen = false;
        private bool _isImportedData = false;
        private int? _dataDelayInSeconds = null;
        private FormWindowState _windowState = FormWindowState.Normal;

        private LapStateViewModel _lapStates = new LapStateViewModel();

        private IList<ScreenDefinition> _screenDefinitions = new List<ScreenDefinition>();
        private readonly IDictionary<Keys, ToolStripButton> _screenDefinitionShortcutKeys = new Dictionary<Keys, ToolStripButton>();

        private readonly ILogger<MainForm> _logger = null;
        private readonly ILapTimesRepository _lapTimeRepository = null;
        private readonly ILapAveragesRepository _lapAveragesRepository = null;
        private readonly ILiveFeedRepository _liveFeedRepository = null;
        private readonly ILoopDataRepository _LoopDataRepository = null;
        private readonly IFlagStateRepository _flagStateRepository = null;
        private readonly ISchedulesRepository _raceScheduleRepository = null;
        private readonly IPointsRepository _pointsRepository = null;
        private readonly IPitStopsRepository _pitStopsRepository = null;
        private readonly IScreenService _screenService = null;
        private readonly ICustomViewSettingsService _customViewSettingsService = null;
        private readonly ICustomGridViewFactory _customGridViewFactory = null;
        private readonly IStyleService _styleService = null;
        private readonly IOptions<Features> _features = null;
        private readonly IMoversFallersService _moversFallersService = null;
        private readonly IKeyMomentsRepository _keyMomentsRepository = null;

        #endregion

        #region ctor / load

        public MainForm(
            ILogger<MainForm> logger,
            ILapTimesRepository lapTimeRepository,
            ILapAveragesRepository lapAveragesRepository,
            ILiveFeedRepository liveFeedRepository,
            ILoopDataRepository LoopDataRepository,
            IFlagStateRepository flagStateRepository,
            ISchedulesRepository raceScheduleRepository,
            IPointsRepository pointsRepository,
            IPitStopsRepository pitStopsRepository,
            IScreenService screenService,
            ICustomViewSettingsService customViewSettingsService,
            ICustomGridViewFactory customGridViewFactory,
            IStyleService styleService,
            IMoversFallersService moversFallersService,
            IKeyMomentsRepository keyMomentsRepository,
            IOptions<Features> features)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lapTimeRepository = lapTimeRepository ?? throw new ArgumentNullException(nameof(lapTimeRepository));
            _lapAveragesRepository = lapAveragesRepository ?? throw new ArgumentNullException(nameof(lapAveragesRepository));
            _liveFeedRepository = liveFeedRepository ?? throw new ArgumentNullException(nameof(liveFeedRepository));
            _LoopDataRepository = LoopDataRepository ?? throw new ArgumentNullException(nameof(LoopDataRepository));
            _flagStateRepository = flagStateRepository ?? throw new ArgumentNullException(nameof(flagStateRepository));
            _raceScheduleRepository = raceScheduleRepository ?? throw new ArgumentNullException(nameof(raceScheduleRepository));
            _pointsRepository = pointsRepository ?? throw new ArgumentNullException(nameof(pointsRepository));
            _pitStopsRepository = pitStopsRepository ?? throw new ArgumentNullException(nameof(pitStopsRepository));
            _keyMomentsRepository = keyMomentsRepository ?? throw new ArgumentNullException(nameof(keyMomentsRepository));
            _screenService = screenService ?? throw new ArgumentNullException(nameof(screenService));
            _customViewSettingsService = customViewSettingsService ?? throw new ArgumentNullException(nameof(customViewSettingsService));
            _customGridViewFactory = customGridViewFactory ?? throw new ArgumentNullException(nameof(customGridViewFactory));
            _styleService = styleService ?? throw new ArgumentNullException(nameof(styleService));
            _moversFallersService = moversFallersService ?? throw new ArgumentNullException(nameof(moversFallersService));
            _features = features ?? throw new ArgumentNullException(nameof(features));
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetFeaturesStatus(_features.Value);

                lblEventName.Text = "";

                lblVersion.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version}";

                await SetViewStateAsync(ViewState.None, true);

                ReloadScreenDefinitions();

                await DisplayTodaysScheduleAsync(true);

                var settings = UserSettingsService.LoadUserSettings();

                _dataDelayInSeconds = settings.DataDelayInSeconds;

                if (settings.AutoUpdateEnabledOnStart)
                {
                    await SetAutoUpdateStateAsync(true);
                }
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

                    cautionDetails = flagData != null && (flagData.Comment != null || flagData.Beneficiary != null) ? $"{Environment.NewLine}{flagData.Comment?.ToString()}{Environment.NewLine}Lucky Dog:{flagData.Beneficiary?.ToString().Trim()}" : String.Empty;
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

        private void SetCheckedStates(ToolStripButton buttonClicked)
        {
            btnRaceView.Checked = false;
            btnQualifyingView.Checked = false;
            btnPracticeView.Checked = false;
            btnPitStopsView.Checked = false;

            if (buttonClicked != null)
                buttonClicked.Checked = true;
        }

        private async void btnRaceView_Click(object sender, EventArgs e)
        {
            try
            {
                SetCheckedStates(sender as ToolStripButton);

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
                SetCheckedStates(sender as ToolStripButton);

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
                SetCheckedStates(sender as ToolStripButton);

                await SetViewStateAsync(ViewState.Practice);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void btnPitStopsView_Click(object sender, EventArgs e)
        {
            try
            {
                SetCheckedStates(sender as ToolStripButton);

                await SetViewStateAsync(ViewState.PitStops);
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
                SetCheckedStates(null);

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
                SetCheckedStates(null);

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
                SetCheckedStates(null);

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
                SetCheckedStates(null);

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
                SetCheckedStates(null);

                _selectedScheduleType = ScheduleType.ThisWeek;

                await DisplayScheduleViewAsync(_selectedScheduleType);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void nextWeekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetCheckedStates(null);

                _selectedScheduleType = ScheduleType.NextWeek;

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
                SetCheckedStates(null);

                await DisplayTodaysScheduleAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void historicalDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetCheckedStates(null);

                await DisplayHistoricalScheduleAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async Task DisplayTodaysScheduleAsync(bool switchToThisWeekIfEmpty = false)
        {
            _selectedScheduleType = ScheduleType.Today;

            var hasEventsScheduledToday = await DisplayScheduleViewAsync(_selectedScheduleType, !switchToThisWeekIfEmpty);

            if (!hasEventsScheduledToday && switchToThisWeekIfEmpty)
            {
                _selectedScheduleType = ScheduleType.ThisWeek;

                await DisplayScheduleViewAsync(_selectedScheduleType);
            }
        }

        private async Task DisplayHistoricalScheduleAsync()
        {
            _selectedScheduleType = ScheduleType.Historical;

            await DisplayScheduleViewAsync(_selectedScheduleType, true);
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
            if (_isImportedData)
                return true;

            _formState.LiveFeed = await _liveFeedRepository.GetLiveFeedAsync();

            if (_formState.LiveFeed.TimeOfDayOs == _lastLiveFeedTimestamp)
                return false;

            _lastLiveFeedTimestamp = _formState.LiveFeed.TimeOfDayOs;

            if (_formState.CurrentSeriesRace == null || _formState.LiveFeed.RaceId != _formState.CurrentSeriesRace.RaceId)
            {
                _formState.SeriesSchedules = await GetSeriesScheduleAsync((ScheduleType)_formState.LiveFeed.SeriesId);

                var currentRace = _formState.SeriesSchedules.FirstOrDefault(s => s.RaceId == _formState.LiveFeed.RaceId);

                _formState.CurrentSeriesRace = currentRace;
            }

            _formState.LapTimes = await _lapTimeRepository.GetLapTimeDataAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);
            _formState.FlagStates = await _flagStateRepository.GetFlagStatesAsync();

            _formState.EventStatistics = await _LoopDataRepository.GetEventAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

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

            _formState.PitStops = await _pitStopsRepository.GetPitStopsAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

            _formState.KeyMoments = await _keyMomentsRepository.GetKeyMomentsAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

            if (_dataDelayInSeconds.HasValue)
            {
                await Task.Delay(_dataDelayInSeconds.Value * 1000);
            }

            return true;
        }

        private async Task<IList<SeriesEvent>> GetSeriesScheduleAsync(ScheduleType scheduleType)
        {
            if (scheduleType == ScheduleType.Historical)
                return new List<SeriesEvent>();

            var raceLists = await _raceScheduleRepository.GetRaceListAsync();

            switch (scheduleType)
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
                        var range = DayOfWeekHelper.GetScheduleRange(scheduleType);

                        return raceLists.CupSeries.
                            Concat(raceLists.XfinitySeries).
                            Concat(raceLists.TruckSeries).
                            Where(s => s.DateScheduled.Date >= range.Start.Date && s.DateScheduled.Date <= range.End.Date).
                            ToList();
                    }
                case ScheduleType.NextWeek:
                    {
                        var range = DayOfWeekHelper.GetScheduleRange(scheduleType);

                        return raceLists.CupSeries.
                            Concat(raceLists.XfinitySeries).
                            Concat(raceLists.TruckSeries).
                            Where(s => s.DateScheduled.Date >= range.Start.Date && s.DateScheduled.Date <= range.End.Date).
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
                        LogError($"Error selecting schedule to read: Unrecognized Series {scheduleType}");
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

        private void LoadLeaderboards(Panel panel, LeaderboardGridView.RunTypes runType, UserSettings settings)
        {
            LeaderboardGridView leftLeaderboardGridView = new LeaderboardGridView(
              LeaderboardGridView.LeaderboardSides.Left,
              runType)
            {
                SeriesId = _formState.LiveFeed.SeriesId,
                FontOverride = settings.UseLowScreenResolutionSizes ?
                    new Font(
                        settings.OverrideFontName,
                        settings.OverrideFontSize.GetValueOrDefault(10),
                        (FontStyle)settings.OverrideFontStyle) :
                        null
            };
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
                runType)
            {
                SeriesId = _formState.LiveFeed.SeriesId                
            };
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

        private void LoadSelectedViews(Panel panel, IList<int> selectedViews, DockStyle dockStyle, UserSettings settings)
        {
            for (int i = 0; i < selectedViews.Count; i++)
            {
                var selectedView = selectedViews[i];

                var viewType = (GridViewTypes)selectedView;

                switch (viewType)
                {
                    case GridViewTypes.DriverPoints:
                        AddGridToPanel(panel, ViewFactory.GetDriverPointsGridView(), dockStyle);
                        break;
                    case GridViewTypes.FastestLaps:
                        AddGridToPanel(panel, ViewFactory.GetFastestLapsGridView(settings.FastestLapsDisplayType), dockStyle);
                        break;
                    case GridViewTypes.LapLeaders:
                        AddGridToPanel(panel, ViewFactory.GetLapLeadersGridView(), dockStyle);
                        break;
                    case GridViewTypes.StagePoints:
                        AddGridToPanel(panel, ViewFactory.GetStagePointsGridView(), dockStyle);
                        break;
                    case GridViewTypes.Best5Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsGridView.ViewTypes.Best5Laps, settings.BestNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Best10Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsGridView.ViewTypes.Best10Laps, settings.BestNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Best15Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsGridView.ViewTypes.Best15Laps, settings.BestNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Last5Laps:
                        AddGridToPanel(panel, ViewFactory.GetNLapsGridView(NLapsGridView.ViewTypes.Last5Laps, settings.LastNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Last10Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsGridView.ViewTypes.Last10Laps, settings.LastNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Last15Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsGridView.ViewTypes.Last15Laps, settings.LastNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Movers:
                        AddGridToPanel(panel, ViewFactory.GetMoversGridView(), dockStyle);
                        break;
                    case GridViewTypes.Fallers:
                        AddGridToPanel(panel, ViewFactory.GetFallersGridView(), dockStyle);
                        break;
                    case GridViewTypes.Flags:
                        AddGridToPanel(panel, ViewFactory.GetFlagsGridView(), dockStyle);
                        break;
                    case GridViewTypes.KeyMoments:
                        AddGridToPanel(panel, ViewFactory.GetKeyMomentsGridView(), dockStyle);
                        break;
                    default:
                        break;
                }
            }
        }

        private void DisplayPracticeScreen()
        {
            var settings = UserSettingsService.LoadUserSettings();

            UpdateViewStatusLabel("Practice");

            LoadDefaultPanels();

            /*** Main ***/
            LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Practice, settings);

            /*** Right ***/
            LoadSelectedViews(pnlRight, settings.PracticeViewRightGrids, DockStyle.Top, settings);

            /*** Bottom ***/
            LoadSelectedViews(pnlBottom, settings.PracticeViewBottomGrids, DockStyle.Left, settings);

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
            UpdateViewStatusLabel("Qualifying");

            LoadDefaultPanels();

            var settings = UserSettingsService.LoadUserSettings();

            if (settings.QualifyingViewRightGrids.Count == 0)
            {
                pnlRight.Visible = false;
                pnlMain.BringToFront();
            }
            if (settings.QualifyingViewBottomGrids.Count == 0)
            {
                pnlBottom.Visible = false;
                pnlMain.BringToFront();
            }

            /*** main panel ***/
            LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Qualifying, settings);

            /*** Right ***/
            LoadSelectedViews(pnlRight, settings.QualifyingViewRightGrids, DockStyle.Top, settings);

            /*** Bottom ***/
            LoadSelectedViews(pnlBottom, settings.QualifyingViewBottomGrids, DockStyle.Left, settings);

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
                UpdateViewStatusLabel("Race");

                this.SuspendLayout();

                LoadDefaultPanels();

                var settings = UserSettingsService.LoadUserSettings();

                /*** main panel ***/
                LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Race, settings);

                /*** Right ***/
                LoadSelectedViews(pnlRight, settings.RaceViewRightGrids, DockStyle.Top, settings);

                /*** Bottom ***/
                LoadSelectedViews(pnlBottom, settings.RaceViewBottomGrids, DockStyle.Left, settings);

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

            lblLastUpdate.Text = $"Last Update: {_formState.LiveFeed.TimeOfDayOs}";

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
                    case ApiSources.LoopData:
                        ((IGridView<rNascar23.LoopData.Models.Driver>)gridView).Data = _formState.EventStatistics.drivers;
                        break;
                    case ApiSources.Flags:
                        ((IGridView<FlagState>)gridView).Data = _formState.FlagStates;
                        break;
                    case ApiSources.LapTimes:
                        ((IGridView<DriverLaps>)gridView).Data = _formState.LapTimes.Drivers;
                        break;
                    case ApiSources.LapTimeData:
                        ((IGridView<LapTimeData>)gridView).Data = new List<LapTimeData>() { _formState.LapTimes };
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
                    case ApiSources.PitStops:
                        ((IGridView<PitStop>)gridView).Data = _formState.PitStops;
                        break;
                    default:
                        break;
                }
            }

            if (pnlBottom.Visible)
                SetGridData(pnlBottom);
            if (pnlRight.Visible)
                SetGridData(pnlRight);
            if (pnlMain.Visible)
                SetGridData(pnlMain);
            if (pnlHost.Visible)
                SetGridData(pnlHost);

            DisplayHeaderData();
        }

        private void SetGridData(Panel panel)
        {
            foreach (GenericGridView uc in panel.Controls.OfType<GenericGridView>())
            {
                switch (uc.GridViewType)
                {
                    case GridViewTypes.DriverPoints:
                        uc.SetDataSource<GenericGridViewModel>(BuildDriverPointsData(_formState.LivePoints));
                        break;
                    case GridViewTypes.FastestLaps:
                        uc.SetDataSource<GenericGridViewModel>(BuildFastestLapsData(_formState.LiveFeed.Vehicles));
                        break;
                    case GridViewTypes.LapLeaders:
                        uc.SetDataSource<GenericGridViewModel>(BuildLapLeadersData(_formState.LiveFeed.Vehicles));
                        break;
                    case GridViewTypes.StagePoints:
                        uc.SetDataSource<GenericGridViewModel>(BuildStagePointsData(_formState.StagePoints, _formState.LiveFeed?.RaceId));
                        break;
                    case GridViewTypes.Best5Laps:
                    case GridViewTypes.Best10Laps:
                    case GridViewTypes.Best15Laps:
                    case GridViewTypes.Last5Laps:
                    case GridViewTypes.Last10Laps:
                    case GridViewTypes.Last15Laps:
                        uc.SetDataSource<GenericGridViewModel>(BuildNLapsData(uc.GridViewType, _formState.LapTimes.Drivers, _formState.LapAverages));
                        break;
                    case GridViewTypes.Movers:
                        uc.SetDataSource<GenericGridViewModel>(BuildMoversData(_formState.LapTimes));
                        break;
                    case GridViewTypes.Fallers:
                        uc.SetDataSource<GenericGridViewModel>(BuildFallersData(_formState.LapTimes));
                        break;
                    default:
                        break;
                }
            }

            foreach (GridViewBase uc in panel.Controls.OfType<FlagsView>())
            {
                uc.SetDataSource<FlagState>(_formState.FlagStates.ToList());
            }

            foreach (GridViewBase uc in panel.Controls.OfType<KeyMomentsView>())
            {
                uc.SetDataSource<KeyMoment>(_formState.KeyMoments);
            }
        }

        private IList<GenericGridViewModel> BuildLapLeadersData(IList<Vehicle> vehicles)
        {
            var models = new List<GenericGridViewModel>();

            int i = 1;
            foreach (var lapLedLeader in vehicles.
                Where(v => v.laps_led.Length > 0).
                OrderByDescending(v => v.laps_led.Sum(l => l.end_lap - l.start_lap)))
            {
                var lapLeader = new GenericGridViewModel()
                {
                    Position = i,
                    Driver = lapLedLeader.driver.FullName,
                    Value = lapLedLeader.laps_led.Sum(l => l.end_lap - l.start_lap) + 1
                };

                models.Add(lapLeader);
                i++;
            }

            return models;
        }

        private IList<GenericGridViewModel> BuildDriverPointsData(IList<DriverPoints> driverPoints)
        {
            return driverPoints.
                Select(p => new GenericGridViewModel()
                {
                    Position = p.PointsPosition,
                    Driver = p.Driver,
                    Value = p.Points
                }).OrderBy(p => p.Position).ToList();
        }

        private IList<GenericGridViewModel> BuildStagePointsData(IList<StagePoints2> stagePoints, int? raceId)
        {
            IList<GenericGridViewModel> models = new List<GenericGridViewModel>();

            if (stagePoints == null || stagePoints.Count == 0)
                return models;

            if (raceId.HasValue && stagePoints.Any(s => s.race_id == raceId.Value))
            {
                stagePoints = stagePoints.Where(s => s.race_id == raceId.Value).ToList();
            }

            int i = 1;
            foreach (var driverStagePoints in stagePoints.
                Where(s => (s.stage_1_points + s.stage_2_points + s.stage_3_points) > 0).
                OrderByDescending(s => (s.stage_1_points + s.stage_2_points + s.stage_3_points)))
            {
                var model = new GenericGridViewModel()
                {
                    Position = i,
                    Driver = $"{driverStagePoints.first_name} {driverStagePoints.last_name}",
                    Value = driverStagePoints.stage_1_points + driverStagePoints.stage_2_points + driverStagePoints.stage_3_points
                };

                models.Add(model);

                i++;
            }

            return models;
        }

        private IList<GenericGridViewModel> BuildFastestLapsData(IList<Vehicle> vehicles)
        {
            var models = new List<GenericGridViewModel>();

            var settings = UserSettingsService.LoadUserSettings();

            if (settings.FastestLapsDisplayType == SpeedTimeType.MPH)
            {
                models = vehicles.
                    Where(v => v.best_lap_speed > 0).
                    OrderByDescending(v => v.best_lap_speed).
                    Take(10).
                    Select(v => new GenericGridViewModel()
                    {
                        Driver = v.driver.FullName,
                        Value = (float)Math.Round(v.best_lap_speed, 3)
                    }).ToList();
            }
            else
            {
                models = vehicles.
                    Where(v => v.best_lap_time > 0).
                    OrderBy(v => v.best_lap_time).
                    Take(10).
                    Select(v => new GenericGridViewModel()
                    {
                        Driver = v.driver.FullName,
                        Value = (float)Math.Round(v.best_lap_time, 3)
                    }).ToList();
            }

            for (int i = 0; i < models.Count; i++)
            {
                models[i].Position = i + 1;
            }

            return models;
        }

        private IList<GenericGridViewModel> BuildMoversData(LapTimeData lapTimeData)
        {
            var models = new List<GenericGridViewModel>();

            var changes = _moversFallersService.GetDriverPositionChanges(lapTimeData);

            // Movers
            models = changes.
                OrderByDescending(c => c.ChangeSinceFlagChange).
                Where(c => c.ChangeSinceFlagChange > 0).
                Select(c => new GenericGridViewModel()
                {
                    Driver = c.Driver,
                    Value = c.ChangeSinceFlagChange
                }).
                ToList();

            for (int i = 0; i < models.Count; i++)
            {
                models[i].Position = i + 1;
            }

            return models;
        }

        private IList<GenericGridViewModel> BuildFallersData(LapTimeData lapTimeData)
        {
            IList<GenericGridViewModel> models = new List<GenericGridViewModel>();

            var changes = _moversFallersService.GetDriverPositionChanges(lapTimeData);

            // Fallers
            models = changes.
                OrderBy(c => c.ChangeSinceFlagChange).
                Where(c => c.ChangeSinceFlagChange < 0).
                Select(c => new GenericGridViewModel()
                {
                    Driver = c.Driver,
                    Value = c.ChangeSinceFlagChange
                }).
                ToList();

            for (int i = 0; i < models.Count; i++)
            {
                models[i].Position = i + 1;
            }

            return models;
        }

        private IList<GenericGridViewModel> BuildNLapsData(GridViewTypes viewType, IList<DriverLaps> dataSource1, IList<LapAverages> dataSource2)
        {
            IList<GenericGridViewModel> models = new List<GenericGridViewModel>();

            var settings = UserSettingsService.LoadUserSettings();

            var displayType = viewType == GridViewTypes.Best5Laps || viewType == GridViewTypes.Best10Laps || viewType == GridViewTypes.Best15Laps ?
                settings.BestNLapsDisplayType : settings.LastNLapsDisplayType;

            if (dataSource1 == null || dataSource1.Count == 0)
                return BuildViewModelsByLapAverages(viewType, dataSource2);

            switch (displayType)
            {
                case SpeedTimeType.Seconds:
                    models = BuildViewModelsByTimeByDriverLaps(viewType, dataSource1);
                    break;
                case SpeedTimeType.MPH:
                    models = BuildViewModelsBySpeedByDriverLaps(viewType, dataSource1);
                    break;
                default:
                    models = new List<GenericGridViewModel>();
                    break;
            }

            for (int i = 0; i < models.Count; i++)
            {
                models[i].Position = i;
            }

            return models;
        }
        private IList<GenericGridViewModel> BuildViewModelsByTimeByDriverLaps(GridViewTypes viewType, IList<DriverLaps> data)
        {
            switch (viewType)
            {
                case GridViewTypes.Best5Laps:
                    return data.
                        OrderBy(d => d.Best5LapAverageTime().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.Best5LapAverageTime().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case GridViewTypes.Best10Laps:
                    return data.
                        OrderBy(d => d.Best10LapAverageTime().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.Best10LapAverageTime().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case GridViewTypes.Best15Laps:
                    return data.
                        OrderBy(d => d.Best15LapAverageTime().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.Best15LapAverageTime().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case GridViewTypes.Last5Laps:
                    return data.
                        OrderBy(d => d.AverageTimeLast5Laps().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.AverageTimeLast5Laps().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case GridViewTypes.Last10Laps:
                    return data.
                        OrderBy(d => d.AverageTimeLast10Laps().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.AverageTimeLast10Laps().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                case GridViewTypes.Last15Laps:
                    return data.
                        OrderBy(d => d.AverageTimeLast15Laps().GetValueOrDefault(999)).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.AverageTimeLast15Laps().GetValueOrDefault(999), 3)
                        }).
                        ToList();

                default:
                    return new List<GenericGridViewModel>();
            }
        }
        private IList<GenericGridViewModel> BuildViewModelsBySpeedByDriverLaps(GridViewTypes viewType, IList<DriverLaps> data)
        {
            switch (viewType)
            {
                case GridViewTypes.Best5Laps:
                    return data.
                        OrderByDescending(d => d.Best5LapAverageSpeed().GetValueOrDefault(-1)).
                        Where(d => d.Best5LapAverageSpeed().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.Best5LapAverageSpeed().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case GridViewTypes.Best10Laps:
                    return data.
                        OrderByDescending(d => d.Best10LapAverageSpeed().GetValueOrDefault(-1)).
                        Where(d => d.Best10LapAverageSpeed().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.Best10LapAverageSpeed().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case GridViewTypes.Best15Laps:
                    return data.
                        OrderByDescending(d => d.Best15LapAverageSpeed().GetValueOrDefault(-1)).
                        Where(d => d.Best15LapAverageSpeed().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.Best15LapAverageSpeed().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case GridViewTypes.Last5Laps:
                    return data.
                        OrderByDescending(d => d.AverageSpeedLast5Laps().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast5Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.AverageSpeedLast5Laps().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case GridViewTypes.Last10Laps:
                    return data.
                        OrderByDescending(d => d.AverageSpeedLast10Laps().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast10Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.AverageSpeedLast10Laps().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                case GridViewTypes.Last15Laps:
                    return data.
                        OrderByDescending(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1)).
                        Where(d => d.AverageSpeedLast15Laps().GetValueOrDefault(-1) != -1).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Driver = d.FullName,
                            Value = (float)Math.Round(d.AverageSpeedLast15Laps().GetValueOrDefault(-1), 3)
                        }).
                        ToList();

                default:
                    return new List<GenericGridViewModel>();
            }
        }
        private IList<GenericGridViewModel> BuildViewModelsByLapAverages(GridViewTypes viewType, IList<LapAverages> data)
        {
            switch (viewType)
            {
                case GridViewTypes.Best5Laps:
                    var best5Laps = data.
                        Where(v => v.Con5LapRank != null).
                        OrderBy(v => v.Con5LapRank).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Position = d.Con5LapRank.Value,
                            Driver = d.FullName,
                            Value = (float)d.Con5Lap
                        }).
                        ToList();

                    return best5Laps;

                case GridViewTypes.Best10Laps:
                    var best10Laps = data.
                        Where(v => v.Con10LapRank != null).
                        OrderBy(v => v.Con10LapRank).
                        Take(10).
                        Select(d => new GenericGridViewModel()
                        {
                            Position = d.Con10LapRank.Value,
                            Driver = d.FullName,
                            Value = (float)d.Con10Lap
                        }).
                        ToList();

                    return best10Laps;

                case GridViewTypes.Best15Laps:
                    var best15Laps = data.
                      Where(v => v.Con15LapRank != null).
                      OrderBy(v => v.Con15LapRank).
                      Take(10).
                      Select(d => new GenericGridViewModel()
                      {
                          Position = d.Con15LapRank.Value,
                          Driver = d.FullName,
                          Value = (float)d.Con15Lap
                      }).
                      ToList();

                    return best15Laps;

                case GridViewTypes.Last5Laps:
                    return new List<GenericGridViewModel>();

                case GridViewTypes.Last10Laps:
                    return new List<GenericGridViewModel>();

                case GridViewTypes.Last15Laps:
                    return new List<GenericGridViewModel>();

                default:
                    return new List<GenericGridViewModel>();
            }
        }

        private void SetTheme()
        {
            var settings = UserSettingsService.LoadUserSettings();

            if (settings.UseDarkTheme)
            {
                pnlBottom.BackColor = Color.Black;
                pnlRight.BackColor = Color.Black;
                pnlMain.BackColor = Color.Black;
                pnlHost.BackColor = Color.Black;
                lblEventName.ForeColor = Color.White;
                lblEventName.BackColor = Color.Black;
                lblRaceLaps.ForeColor = Color.WhiteSmoke;
                lblRaceLaps.BackColor = Color.Black;
                lblStageLaps.ForeColor = Color.WhiteSmoke;
                lblStageLaps.BackColor = Color.Black;
                pnlEventInfo.BackColor = Color.Black;
            }
            else
            {
                pnlBottom.BackColor = Color.White;
                pnlRight.BackColor = Color.White;
                pnlMain.BackColor = Color.White;
                pnlHost.BackColor = Color.White;
                lblEventName.ForeColor = Color.Black;
                lblEventName.BackColor = Color.White;
                lblRaceLaps.ForeColor = Color.Black;
                lblRaceLaps.BackColor = Color.White;
                lblStageLaps.ForeColor = Color.Black;
                lblStageLaps.BackColor = Color.White;
                pnlEventInfo.BackColor = Color.WhiteSmoke;
            }
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

        private void DisplayPitStopsViewScreen()
        {
            pnlMain.Visible = false;
            pnlRight.Visible = false;
            pnlBottom.Visible = false;
            pnlHeader.Visible = true;
            pnlFlagGreenYellow.Visible = true;
            picGreenYelllowLapIndicator.Visible = true;

            pnlHost.Controls.Clear();
            pnlHost.Visible = true;
            pnlHost.Dock = DockStyle.Fill;

            UpdateViewStatusLabel("Pit Stops");

            IApiDataView<PitStop> pitStopsView = (IApiDataView<PitStop>)new PitStopView()
            {
                CurrentLap = _formState.LiveFeed.LapNumber,
                SeriesId = _formState.LiveFeed.SeriesId,
                RaceId = _formState.LiveFeed.RaceId,
            };

            pnlHost.Controls.Add((Control)pitStopsView);

            ((Control)pitStopsView).Dock = DockStyle.Fill;
            ((Control)pitStopsView).BackColor = Color.White;

            pitStopsView.Data = _formState.PitStops;
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

        private async Task<bool> DisplayScheduleViewAsync(ScheduleType scheduleType, bool displayEmptySchedule = true)
        {
            UpdateViewStatusLabel("Schedules");

            if (_viewState != ViewState.ScheduleView)
                await SetViewStateAsync(ViewState.ScheduleView);

            var schedule = await GetSeriesScheduleAsync(scheduleType);

            if (!displayEmptySchedule && schedule.Count == 0)
                return false;

            IApiDataView<SeriesEvent> scheduleView = null;

            if (pnlHost.Controls.OfType<ScheduleView>().Count() > 0)
            {
                scheduleView = pnlHost.Controls.OfType<ScheduleView>().First();
            }
            else
            {
                scheduleView = new ScheduleView(scheduleType);

                pnlHost.Controls.Clear();

                pnlHost.Dock = DockStyle.Fill;
                pnlHost.Controls.Add((Control)scheduleView);

                ((Control)scheduleView).Dock = DockStyle.Fill;
                ((Control)scheduleView).BackColor = Color.White;
            }

            ((ScheduleView)scheduleView).ScheduleType = scheduleType;
            scheduleView.Data = schedule;

            return true;
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
                    Dictionary<string, List<Schedules.Models.Schedule>> eventSchedule = new Dictionary<string, List<Schedules.Models.Schedule>>();
                    SeriesEvent seriesEvent = null;

                    foreach (DataGridViewRow seriesScheduleRow in _seriesScheduleDataGridView.Rows)
                    {
                        seriesEvent = (SeriesEvent)seriesScheduleRow.DataBoundItem;

                        List<Schedules.Models.Schedule> seriesEventSchedule = (List<Schedules.Models.Schedule>)seriesEvent.Schedule.ToList();

                        eventSchedule.Add(seriesEvent.SeriesName, seriesEventSchedule);
                    }

                    var seriesEventActivities = new List<SeriesEventScheduleViewModel>();

                    foreach (KeyValuePair<string, List<Schedules.Models.Schedule>> item in eventSchedule)
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
                    Dictionary<string, List<Schedules.Models.Schedule>> eventSchedule = new Dictionary<string, List<Schedules.Models.Schedule>>();
                    SeriesEvent seriesEvent = null;

                    foreach (DataGridViewRow seriesScheduleRow in _seriesScheduleDataGridView.Rows)
                    {
                        seriesEvent = (SeriesEvent)seriesScheduleRow.DataBoundItem;

                        List<Schedules.Models.Schedule> seriesEventSchedule = (List<Schedules.Models.Schedule>)seriesEvent.Schedule.ToList();

                        eventSchedule.Add(seriesEvent.SeriesName, seriesEventSchedule);
                    }

                    var seriesEventActivities = new List<SeriesEventScheduleViewModel>();

                    foreach (KeyValuePair<string, List<Schedules.Models.Schedule>> item in eventSchedule)
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

                if (_viewState == ViewState.Race)
                    DisplayRaceLaps(_formState.LiveFeed.LapNumber, _formState.LiveFeed.LapsInRace);

                if (_viewState == ViewState.Race && _lapStates.Stage1Laps > 0 && _lapStates.Stage1Laps > 0)
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

            int f = 6;
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

                Keys key = Keys.F6;

                switch (f)
                {
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
                        LoadLeaderboards(targetPanel, LeaderboardGridView.RunTypes.Race, settings);
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
                    ToggleFullscreen(!_isFullScreen);
                }
                else if (e.KeyCode == Keys.F12)
                {
                    await SetAutoUpdateStateAsync(!AutoUpdateTimer.Enabled);
                }
                else if (e.KeyCode == Keys.F1)
                {
                    SetCheckedStates(btnRaceView);

                    await SetViewStateAsync(ViewState.Race);
                }
                else if (e.KeyCode == Keys.F2)
                {
                    SetCheckedStates(btnQualifyingView);

                    await SetViewStateAsync(ViewState.Qualifying);
                }
                else if (e.KeyCode == Keys.F3)
                {
                    SetCheckedStates(btnPracticeView);

                    await SetViewStateAsync(ViewState.Practice);
                }
                else if (e.KeyCode == Keys.F4)
                {
                    ddbSchedules.ShowDropDown();
                }
                else if (e.KeyCode == Keys.F5)
                {
                    SetCheckedStates(btnPitStopsView);

                    await SetViewStateAsync(ViewState.PitStops);
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

        private void tsbFullScreen_Click(object sender, EventArgs e)
        {
            ToggleFullscreen(!_isFullScreen);
        }

        private void ToggleFullscreen(bool fullscreen)
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

        #endregion

        #region private [ user settings ]

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

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
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

        private async void DisplaySettingsDialog()
        {
            var dialog = Program.Services.GetRequiredService<UserSettingsDialog>();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                _logger.LogInformation("User settings updated");

                _dataDelayInSeconds = dialog.UserSettings.DataDelayInSeconds;

                var viewState = _viewState;

                await SetViewStateAsync(ViewState.None, true);

                await SetViewStateAsync(viewState, true);

                if (viewState == ViewState.ScheduleView)
                    await DisplayScheduleViewAsync(_selectedScheduleType);
            }
        }

        #endregion

        #region private [ replay ]

        private async void replayEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var replay = SelectEventReplay();

                if (replay != null)
                    await BeginReplayEventAsync(replay);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception selecting event replay");
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PlayEventReplay();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception playing replay event");
            }
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PauseEventReplay();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception pausing replay event");
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CloseEventReplay();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception closing replay event");
            }
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _replaySpeed = 1;
            xToolStripMenuItem.Checked = true;
            xToolStripMenuItem1.Checked = false;
            xToolStripMenuItem2.Checked = false;
        }

        private void xToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _replaySpeed = 5;
            xToolStripMenuItem.Checked = false;
            xToolStripMenuItem1.Checked = true;
            xToolStripMenuItem2.Checked = false;
        }

        private void xToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _replaySpeed = 10;
            xToolStripMenuItem.Checked = false;
            xToolStripMenuItem1.Checked = false;
            xToolStripMenuItem2.Checked = true;
        }

        private async void timEventReplay_Tick(object sender, EventArgs e)
        {
            try
            {
                await LoadEventReplayFrameAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception playing event replay frame");
            }
        }

        private EventReplay SelectEventReplay()
        {
            var dialog = Program.Services.GetRequiredService<ReplaySelectionDialog>();

            dialog.ReplayDirectory = "C:\\Users\\Rob\\Documents\\rNascar23\\Events";

            if (dialog.ShowDialog(this) == DialogResult.OK)
                return dialog.SelectedReplay;
            else
                return null;
        }

        private async Task BeginReplayEventAsync(EventReplay replay)
        {
            _eventReplay = replay;
            _replayFrameIndex = -1;

            playToolStripMenuItem.Enabled = true;
            pauseToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            replaySpeedToolStripMenuItem.Enabled = true;

            lblEventReplayStatus.Visible = true;

            _isImportedData = true;

            await SetAutoUpdateStateAsync(false);

            PlayEventReplay();
        }

        private void PlayEventReplay()
        {
            playToolStripMenuItem.Enabled = false;
            pauseToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            importDumpFileToolStripMenuItem.Enabled = false;
            replayEventToolStripMenuItem.Enabled = false;
            replaySpeedToolStripMenuItem.Enabled = true;

            timEventReplay.Enabled = true;
        }

        private void PauseEventReplay()
        {
            timEventReplay.Enabled = false;

            playToolStripMenuItem.Enabled = true;
            pauseToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = true;
            replaySpeedToolStripMenuItem.Enabled = true;
        }

        private void CloseEventReplay()
        {
            timEventReplay.Enabled = false;

            playToolStripMenuItem.Enabled = false;
            pauseToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            importDumpFileToolStripMenuItem.Enabled = true;
            replayEventToolStripMenuItem.Enabled = true;
            replaySpeedToolStripMenuItem.Enabled = false;

            _isImportedData = false;

            lblEventReplayStatus.Visible = false;
        }

        private async Task LoadEventReplayFrameAsync()
        {
            timEventReplay.Enabled = false;

            if (_replayFrameIndex >= _eventReplay.Frames.Count - _replaySpeed)
            {
                _replayFrameIndex = -1;

                PauseEventReplay();

                return;
            }
            else
                _replayFrameIndex += _replaySpeed;

            UpdateReplayLabel(_eventReplay, _replayFrameIndex);

            var frame = _eventReplay.Frames.OrderBy(f => f.Index).ToArray()[_replayFrameIndex];

            if (frame != null)
                await LoadStateFromReplayFrameAsync(frame);

            timEventReplay.Enabled = true;
        }

        private async Task LoadStateFromReplayFrameAsync(EventReplayFrame frame)
        {
            var jsonFileName = Path.GetFileNameWithoutExtension(frame.FileName);

            using (FileStream compressedFileStream = File.Open(frame.FileName, FileMode.Open))
            {
                using (FileStream outputFileStream = File.Create(jsonFileName))
                {
                    using (var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        decompressor.CopyTo(outputFileStream);
                    }
                }
            }

            LoadStateFromJsonFile(jsonFileName);

            await UpdateUiAsync();
        }

        private void LoadStateFromJsonFile(string jsonFileName)
        {
            var json = File.ReadAllText(jsonFileName);

            _formState = JsonConvert.DeserializeObject<FormState>(json);

            File.Delete(jsonFileName);
        }

        private void UpdateReplayLabel(EventReplay replay, int index)
        {
            lblEventReplayStatus.Text = $"Replaying {replay.EventName} {replay.TrackName} {replay.Series} {replay.EventType} {replay.EventDate.ToString("yyyy-M-d")} [Frame {index} of {replay.Frames.Count - 1}]";
        }

        #endregion

        #region private [ audio/video ]

        private void audioChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = Program.Services.GetRequiredService<AudioSelectionDialog>();

                dialog.SeriesId = _formState.LiveFeed.SeriesId;

                dialog.Show(this);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception calling audio channel dialog from main form");
            }
        }

        private void inCarCamerasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = Program.Services.GetRequiredService<VideoPlayer>();

                dialog.SeriesId = _formState.LiveFeed.SeriesId;

                dialog.Show(this);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception calling in-car video dialog from main form");
            }
        }

        private void audioChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void SetFeaturesStatus(Features features)
        {
            if (features.EnableDeveloperFeatures)
            {
                toolsToolStripMenuItem1.Visible = false;
                toolsToolStripMenuItem.Visible = true;

                rawLiveFeedDataToolStripMenuItem.Visible = true;
                rawVehicleListToolStripMenuItem.Visible = true;
                rawLoopDataToolStripMenuItem.Visible = true;
                formattedLiveFeedDataToolStripMenuItem.Visible = true;
                toolStripMenuItem3.Visible = true;
                toolStripMenuItem2.Visible = true;
                localDataToolStripMenuItem.Visible = true;
            }
            else
            {
                toolsToolStripMenuItem1.Visible = true;
                toolsToolStripMenuItem.Visible = false;

                rawLiveFeedDataToolStripMenuItem.Visible = false;
                rawVehicleListToolStripMenuItem.Visible = false;
                rawLoopDataToolStripMenuItem.Visible = false;
                formattedLiveFeedDataToolStripMenuItem.Visible = false;
                toolStripMenuItem3.Visible = false;
                toolStripMenuItem2.Visible = false;
                localDataToolStripMenuItem.Visible = false;
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

            SetTheme();

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
                case ViewState.PitStops:
                    DisplayPitStopsViewScreen();
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

        private async void importDumpFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                await OpenFormStateFileAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async Task OpenFormStateFileAsync()
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                LoadStateFromJsonFile(dialog.FileName);

                _isImportedData = true;

                await UpdateUiAsync();
            }
        }

        #endregion
    }
}