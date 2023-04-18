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
        private const string PatchFileName = "Patch Notes.txt";

        #endregion

        #region enums

        private enum ViewState
        {
            None,
            Practice,
            Qualifying,
            Race,
            Schedules,
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

        private ViewState _viewState = ViewState.None;
        private DateTime _lastLiveFeedTimestamp = DateTime.MinValue;
        private FormState _formState = new FormState();
        private FormWindowState _windowState = FormWindowState.Normal;
        private ScheduleType _selectedScheduleType = ScheduleType.All;
        private LapStateViewModel _lapStates = new LapStateViewModel();

        private EventReplay _eventReplay = null;
        private int _replayFrameIndex = 0;
        private int _replaySpeed = 1;
        private int _loadingPanelStackCount = 0;
        private object _syncLockObject = new object();

        private bool _isFullScreen = false;
        private bool _isImportedData = false;

        private readonly ILogger<MainForm> _logger = null;
        private readonly ILapTimesRepository _lapTimeRepository = null;
        private readonly ILapAveragesRepository _lapAveragesRepository = null;
        private readonly ILiveFeedRepository _liveFeedRepository = null;
        private readonly ILoopDataRepository _LoopDataRepository = null;
        private readonly IFlagStateRepository _flagStateRepository = null;
        private readonly ISchedulesRepository _raceScheduleRepository = null;
        private readonly IPointsRepository _pointsRepository = null;
        private readonly IPitStopsRepository _pitStopsRepository = null;
        private readonly IOptions<Features> _features = null;
        private readonly IMoversFallersService _moversFallersService = null;
        private readonly IKeyMomentsRepository _keyMomentsRepository = null;

        #endregion

        #region properties

        private UserSettings _userSettings = null;
        private UserSettings UserSettings
        {
            get
            {
                if (_userSettings == null)
                    _userSettings = UserSettingsService.LoadUserSettings();

                return _userSettings;
            }
        }

        #endregion

        #region ctor/load

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
            _moversFallersService = moversFallersService ?? throw new ArgumentNullException(nameof(moversFallersService));
            _features = features ?? throw new ArgumentNullException(nameof(features));
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                var toolbarAndMenuOffset = this.menuStrip1.Height + this.toolStrip1.Height;
                pnlLoading.Location = new Point(0, toolbarAndMenuOffset);
                pnlLoading.Size = new Size(this.Width, this.Height - toolbarAndMenuOffset);
                lblLoading.Location = new Point
                (
                    (pnlLoading.Width / 2) - (lblLoading.Width / 2),
                    (pnlLoading.Height / 2) - (lblLoading.Height / 2)
                );

                pnlLoading.BringToFront();
                pnlLoading.Visible = true;

                DisplayLoadingPanel();

                Application.DoEvents();

                SetFeaturesStatus(_features.Value);

                lblEventName.Text = "";

                lblVersion.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version}";

                await DisplayTodaysScheduleAsync(true);

                if (UserSettings.AutoUpdateEnabledOnStart)
                {
                    await SetAutoUpdateStateAsync(true);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error loading main form");
            }
            finally
            {
                HideLoadingPanel();
            }
        }

        private void SetFeaturesStatus(Features features)
        {
            if (features.EnableDeveloperFeatures)
            {
                localDataToolStripMenuItem.Visible = true;
            }
            else
            {
                localDataToolStripMenuItem.Visible = false;
            }
        }

        #endregion

        #region private [ form events ]

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
                    DumpFormState();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            try
            {
                pnlLoading.Size = new Size(this.Width, this.Height - (this.menuStrip1.Height + this.toolStrip1.Height));
                lblLoading.Location = new Point
                (
                    (pnlLoading.Width / 2) - (lblLoading.Width / 2),
                    (pnlLoading.Height / 2) - (lblLoading.Height / 2)
                );
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion

        #region private [ paint event handlers ]

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

                var segmentColor = lapSegment.FlagState == FlagColors.Green ? FlagUiColors.Green :
                    lapSegment.FlagState == FlagColors.Yellow ? FlagUiColors.Yellow :
                    lapSegment.FlagState == FlagColors.Red ? FlagUiColors.Red :
                    lapSegment.FlagState == FlagColors.White ? FlagUiColors.White :
                    lapSegment.FlagState == FlagColors.HotTrack ? FlagUiColors.HotTrack :
                    lapSegment.FlagState == FlagColors.ColdTrack ? FlagUiColors.ColdTrack :
                    FlagUiColors.Checkered;

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

                var flagState = lapSegment.FlagState == FlagColors.Green ?
                    "Green" : lapSegment.FlagState == FlagColors.Yellow ?
                    "Caution" : lapSegment.FlagState == FlagColors.Red ?
                    "Red" : lapSegment.FlagState == FlagColors.White ?
                    "White" : "Checkered";

                var startLap = lapSegment.StartLapNumber == 0 ? 1 : lapSegment.StartLapNumber;
                var endLap = lapSegment.Laps == 1 ? lapSegment.StartLapNumber : lapSegment.StartLapNumber + lapSegment.Laps;
                var lapsText = endLap == startLap ? $"Lap {startLap + 1}" : $"Laps {startLap} to {endLap}";

                var cautionDetails = String.Empty;

                if (lapSegment.FlagState == FlagColors.Yellow)
                {
                    var flagData = _formState.FlagStates.FirstOrDefault(f => f.LapNumber >= lapSegment.StartLapNumber && f.LapNumber <= lapSegment.StartLapNumber);

                    cautionDetails = flagData != null && (flagData.Comment != null || flagData.Beneficiary != null) ? $"{Environment.NewLine}{flagData.Comment?.ToString()}{Environment.NewLine}Lucky Dog:{flagData.Beneficiary?.ToString().Trim()}" : String.Empty;
                }

                toolTip1.SetToolTip(flagSegmentPB, $"{flagState} {lapsText}{cautionDetails}");

                picGreenYelllowLapIndicator.Controls.Add(flagSegmentPB);
            }
        }

        #endregion

        #region private [ menu/toolbar event handlers ]

        // menu items
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void patchNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayPatchNotesFile();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

        }

        // toolbar buttons - left
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

        private async void truckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetCheckedStates(null);

                _selectedScheduleType = ScheduleType.Trucks;

                await DisplayScheduleScreenAsync(_selectedScheduleType);
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

                await DisplayScheduleScreenAsync(_selectedScheduleType);

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

                await DisplayScheduleScreenAsync(_selectedScheduleType);
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

                await DisplayScheduleScreenAsync(_selectedScheduleType);
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

                await DisplayScheduleScreenAsync(_selectedScheduleType);
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

                await DisplayScheduleScreenAsync(_selectedScheduleType);
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

        private void SetCheckedStates(ToolStripButton buttonClicked)
        {
            btnRaceView.Checked = false;
            btnQualifyingView.Checked = false;
            btnPracticeView.Checked = false;
            btnPitStopsView.Checked = false;

            if (buttonClicked != null)
                buttonClicked.Checked = true;
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

        // toolbar buttons - right
        private void tsbFullScreen_Click(object sender, EventArgs e)
        {
            ToggleFullscreen(!_isFullScreen);
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

        private void tsbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region private [ auto-update ]

        private async void AutoUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                await UpdateDataViewsAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
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

            if (UserSettings.DataDelayInSeconds.HasValue)
            {
                await Task.Delay(UserSettings.DataDelayInSeconds.Value * 1000);
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

        #endregion

        #region private [ ui state ]

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

            Application.DoEvents();

            if (_viewState != ViewState.None)
                await UpdateDataViewsAsync(false);
        }

        private void SetTheme()
        {
            if (UserSettings.UseDarkTheme)
            {
                pnlBottom.BackColor = Color.Black;
                pnlRight.BackColor = Color.Black;
                pnlMain.BackColor = Color.Black;
                pnlSchedules.BackColor = Color.Black;
                pnlPitStops.BackColor = Color.Black;
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
                pnlSchedules.BackColor = Color.White;
                pnlPitStops.BackColor = Color.White;
                lblEventName.ForeColor = Color.Black;
                lblEventName.BackColor = Color.White;
                lblRaceLaps.ForeColor = Color.Black;
                lblRaceLaps.BackColor = Color.White;
                lblStageLaps.ForeColor = Color.Black;
                lblStageLaps.BackColor = Color.White;
                pnlEventInfo.BackColor = Color.WhiteSmoke;
            }
        }

        private void UpdateViewStatusLabel(string viewName)
        {
            lblViewState.Text = $"View: {viewName}";
        }

        private void SetUiView(ViewState viewState)
        {
            try
            {
                DisplayLoadingPanel();

                ClearViewControls();

                SetHeaderStates(viewState);

                SetPanelStates(viewState);

                Application.DoEvents();

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
                    case ViewState.PitStops:
                        DisplayPitStopsViewScreen();
                        break;
                    case ViewState.Schedules:
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                HideLoadingPanel();
            }
        }

        private void ClearViewControls()
        {
            var existingControls = new List<Control>();

            foreach (Control existingControl in pnlBottom.Controls)
            {
                existingControls.Add(existingControl);
            }

            foreach (Control existingControl in pnlRight.Controls)
            {
                existingControls.Add(existingControl);
            }

            pnlBottom.Controls.Clear();
            pnlRight.Controls.Clear();

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

            Application.DoEvents();
        }

        private void DisplayLoadingPanel()
        {
            pnlLoading.BringToFront();
            pnlLoading.Visible = true;

            menuStrip1.Enabled = false;
            toolStrip1.Enabled = false;

            Application.DoEvents();

            lock (_syncLockObject)
            {
                _loadingPanelStackCount += 1;
            }
        }

        private void HideLoadingPanel()
        {
            try
            {
                lock (_syncLockObject)
                {
                    _loadingPanelStackCount -= 1;

                    if (_loadingPanelStackCount <= 0)
                    {
                        pnlLoading.Visible = false;
                        menuStrip1.Enabled = true;
                        toolStrip1.Enabled = true;
                    }
                }

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                pnlLoading.Visible = false;
                menuStrip1.Enabled = true;
                toolStrip1.Enabled = true;
            }
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
            // check to see if controls already loaded
            if (panel.Controls.OfType<LeaderboardGridView>().Count() > 0)
                return;

            LeaderboardGridView leftLeaderboardGridView = new LeaderboardGridView(
              LeaderboardGridView.LeaderboardSides.Left,
              runType)
            {
                SeriesId = _formState.LiveFeed.SeriesId
            };
            panel.Controls.Add(leftLeaderboardGridView);
            leftLeaderboardGridView.Dock = DockStyle.Left;
            leftLeaderboardGridView.BringToFront();
            leftLeaderboardGridView.Width = (int)((panel.Width - 10) / 2);

            AddSplitter(panel, DockStyle.Left);

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

            AddSplitter(panel, DockStyle.Left);
        }

        private void SetPanelStates(ViewState viewState)
        {
            switch (viewState)
            {
                case ViewState.None:
                    pnlMain.Visible = false;
                    pnlRight.Visible = false;
                    pnlBottom.Visible = false;
                    pnlHeader.Visible = false;
                    pnlSchedules.Visible = false;
                    pnlPitStops.Visible = false;
                    break;
                case ViewState.Practice:
                    pnlMain.Visible = true;
                    pnlRight.Visible = UserSettings.PracticeViewRightGrids.Count > 0;
                    pnlBottom.Visible = UserSettings.PracticeViewBottomGrids.Count > 0;
                    pnlHeader.Visible = true;
                    pnlSchedules.Visible = false;
                    pnlPitStops.Visible = false;
                    break;
                case ViewState.Qualifying:
                    pnlMain.Visible = true;
                    pnlRight.Visible = UserSettings.QualifyingViewRightGrids.Count > 0;
                    pnlBottom.Visible = UserSettings.QualifyingViewBottomGrids.Count > 0;
                    pnlHeader.Visible = true;
                    pnlSchedules.Visible = false;
                    pnlPitStops.Visible = false;
                    break;
                case ViewState.Race:
                    pnlMain.Visible = true;
                    pnlRight.Visible = UserSettings.RaceViewRightGrids.Count > 0;
                    pnlBottom.Visible = UserSettings.RaceViewBottomGrids.Count > 0;
                    pnlHeader.Visible = true;
                    pnlSchedules.Visible = false;
                    pnlPitStops.Visible = false;
                    break;
                case ViewState.Schedules:
                    pnlMain.Visible = false;
                    pnlRight.Visible = false;
                    pnlBottom.Visible = false;
                    pnlHeader.Visible = false;
                    pnlPitStops.Visible = false;
                    pnlSchedules.Visible = true;
                    break;
                case ViewState.PitStops:
                    pnlMain.Visible = false;
                    pnlRight.Visible = false;
                    pnlBottom.Visible = false;
                    pnlHeader.Visible = true;
                    pnlSchedules.Visible = false;
                    pnlPitStops.Visible = true;
                    break;
                default:
                    break;
            }

            Application.DoEvents();
        }

        private void SetHeaderStates(ViewState viewState)
        {
            switch (viewState)
            {
                case ViewState.None:
                    lblRaceLaps.Visible = false;
                    lblStageLaps.Visible = false;

                    pnlHeader.Visible = false;
                    pnlEventInfo.Visible = false;
                    picFlagStatus.Visible = false;
                    picGreenYelllowLapIndicator.Visible = false;

                    break;
                case ViewState.Practice:
                    lblRaceLaps.Visible = false;
                    lblStageLaps.Visible = false;

                    pnlHeader.Visible = true;
                    pnlEventInfo.Visible = true;
                    picFlagStatus.Visible = true;
                    picGreenYelllowLapIndicator.Visible = false;

                    break;
                case ViewState.Qualifying:
                    lblRaceLaps.Visible = false;
                    lblStageLaps.Visible = false;

                    pnlHeader.Visible = true;
                    pnlEventInfo.Visible = true;
                    picFlagStatus.Visible = true;
                    picGreenYelllowLapIndicator.Visible = false;

                    break;
                case ViewState.Race:
                    lblRaceLaps.Visible = true;
                    lblStageLaps.Visible = true;

                    pnlHeader.Visible = true;
                    pnlEventInfo.Visible = true;
                    picFlagStatus.Visible = true;
                    picGreenYelllowLapIndicator.Visible = true;


                    break;
                case ViewState.Schedules:
                    lblRaceLaps.Visible = false;
                    lblStageLaps.Visible = false;

                    pnlHeader.Visible = false;
                    pnlEventInfo.Visible = false;
                    picFlagStatus.Visible = false;
                    picGreenYelllowLapIndicator.Visible = false;

                    break;
                case ViewState.PitStops:
                    lblRaceLaps.Visible = true;
                    lblStageLaps.Visible = true;

                    pnlHeader.Visible = true;
                    pnlEventInfo.Visible = true;
                    picFlagStatus.Visible = true;
                    picGreenYelllowLapIndicator.Visible = true;

                    break;
                default:
                    break;
            }

            var headerHeight = 0;
            if (pnlEventInfo.Visible) headerHeight += pnlEventInfo.Height + 1;
            if (picFlagStatus.Visible) headerHeight += picFlagStatus.Height + 1;
            if (picGreenYelllowLapIndicator.Visible) headerHeight += picGreenYelllowLapIndicator.Height + 1;

            pnlHeader.Height = headerHeight;

            Application.DoEvents();
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
                            ViewFactory.GetNLapsGridView(NLapsViewTypes.Best5Laps, settings.BestNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Best10Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsViewTypes.Best10Laps, settings.BestNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Best15Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsViewTypes.Best15Laps, settings.BestNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Last5Laps:
                        AddGridToPanel(panel, ViewFactory.GetNLapsGridView(NLapsViewTypes.Last5Laps, settings.LastNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Last10Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsViewTypes.Last10Laps, settings.LastNLapsDisplayType),
                            dockStyle);
                        break;
                    case GridViewTypes.Last15Laps:
                        AddGridToPanel(
                            panel,
                            ViewFactory.GetNLapsGridView(NLapsViewTypes.Last15Laps, settings.LastNLapsDisplayType),
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

                Application.DoEvents();
            }
        }

        private void DisplayPracticeScreen()
        {
            UpdateViewStatusLabel("Practice");

            /*** Main ***/
            LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Practice);

            /*** Right ***/
            LoadSelectedViews(pnlRight, UserSettings.PracticeViewRightGrids, DockStyle.Top, UserSettings);

            /*** Bottom ***/
            LoadSelectedViews(pnlBottom, UserSettings.PracticeViewBottomGrids, DockStyle.Left, UserSettings);

            SetBottomAndRightZOrder();
        }

        private void DisplayQualifyingScreen()
        {
            UpdateViewStatusLabel("Qualifying");

            /*** main panel ***/
            LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Qualifying);

            /*** Right ***/
            LoadSelectedViews(pnlRight, UserSettings.QualifyingViewRightGrids, DockStyle.Top, UserSettings);

            /*** Bottom ***/
            LoadSelectedViews(pnlBottom, UserSettings.QualifyingViewBottomGrids, DockStyle.Left, UserSettings);

            SetBottomAndRightZOrder();
        }

        private void DisplayRaceScreen()
        {
            try
            {
                UpdateViewStatusLabel("Race");

                this.SuspendLayout();

                /*** main panel ***/
                LoadLeaderboards(pnlMain, LeaderboardGridView.RunTypes.Race);

                /*** Right ***/
                LoadSelectedViews(pnlRight, UserSettings.RaceViewRightGrids, DockStyle.Top, UserSettings);

                /*** Bottom ***/
                LoadSelectedViews(pnlBottom, UserSettings.RaceViewBottomGrids, DockStyle.Left, UserSettings);

                SetBottomAndRightZOrder();
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

        private async Task<bool> DisplayScheduleScreenAsync(ScheduleType scheduleType, bool displayEmptySchedule = true)
        {
            UpdateViewStatusLabel("Schedules");

            if (_viewState != ViewState.Schedules)
                await SetViewStateAsync(ViewState.Schedules);

            var schedule = await GetSeriesScheduleAsync(scheduleType);

            if (!displayEmptySchedule && schedule.Count == 0)
                return false;

            IApiDataView<SeriesEvent> scheduleView;

            if (pnlSchedules.Controls.OfType<ScheduleView>().Count() > 0)
            {
                scheduleView = pnlSchedules.Controls.OfType<ScheduleView>().First();
            }
            else
            {
                scheduleView = new ScheduleView(scheduleType);

                pnlSchedules.Controls.Clear();

                pnlSchedules.Dock = DockStyle.Fill;
                pnlSchedules.Controls.Add((Control)scheduleView);

                ((Control)scheduleView).Dock = DockStyle.Fill;
                ((Control)scheduleView).BackColor = Color.White;
            }

            ((ScheduleView)scheduleView).ScheduleType = scheduleType;
            scheduleView.Data = schedule;

            return true;
        }

        private async Task DisplayTodaysScheduleAsync(bool switchToThisWeekIfEmpty = false)
        {
            _selectedScheduleType = ScheduleType.Today;

            var hasEventsScheduledToday = await DisplayScheduleScreenAsync(_selectedScheduleType, !switchToThisWeekIfEmpty);

            if (!hasEventsScheduledToday && switchToThisWeekIfEmpty)
            {
                _selectedScheduleType = ScheduleType.ThisWeek;

                await DisplayScheduleScreenAsync(_selectedScheduleType);
            }
        }

        private async Task DisplayHistoricalScheduleAsync()
        {
            _selectedScheduleType = ScheduleType.Historical;

            await DisplayScheduleScreenAsync(_selectedScheduleType, true);
        }

        private void DisplayPitStopsViewScreen()
        {
            UpdateViewStatusLabel("Pit Stops");

            IApiDataView<PitStop> pitStopsView;

            if (pnlPitStops.Controls.OfType<PitStopView>().Count() > 0)
            {
                pitStopsView = pnlPitStops.Controls.OfType<PitStopView>().First();
            }
            else
            {
                pitStopsView = new PitStopView()
                {
                    CurrentLap = _formState.LiveFeed.LapNumber,
                    SeriesId = _formState.LiveFeed.SeriesId,
                    RaceId = _formState.LiveFeed.RaceId,
                };

                pnlPitStops.Dock = DockStyle.Fill;
                pnlPitStops.Controls.Add((Control)pitStopsView);

                ((Control)pitStopsView).Dock = DockStyle.Fill;
                ((Control)pitStopsView).BackColor = Color.White;
            }

            pitStopsView.Data = _formState.PitStops;
        }

        private void SetBottomAndRightZOrder()
        {
            var bottomZIndex = pnlBottom.Parent.Controls.GetChildIndex(pnlBottom);
            var rightZIndex = pnlRight.Parent.Controls.GetChildIndex(pnlRight);

            var bottomPanelWidth = pnlBottom.Width;
            var bottomPanelControlsWidth = pnlBottom.Controls.OfType<Control>().Sum(c => c.Width);

            var rightPanelHeight = pnlRight.Height;
            var rightPanelControlsHeight = pnlRight.Controls.OfType<Control>().Sum(c => c.Height);

            if ((rightPanelControlsHeight <= rightPanelHeight) && (bottomPanelControlsWidth <= bottomPanelWidth))
                // Everything fits, no z-order adjustments required.
                return;
            else if ((rightPanelControlsHeight > rightPanelHeight) && (bottomPanelControlsWidth <= bottomPanelWidth))
            {
                // Right panel views height exceed right panel height, bottom panel OK.
                if (rightZIndex < bottomZIndex)
                {
                    pnlRight.Parent.Controls.SetChildIndex(pnlRight, bottomZIndex);
                }
            }
            else if ((rightPanelControlsHeight <= rightPanelHeight) && (bottomPanelControlsWidth > bottomPanelWidth))
            {
                // Bottom panel views width exceed bottom panel width, right panel OK. 
                if (bottomZIndex < rightZIndex)
                {
                    pnlBottom.Parent.Controls.SetChildIndex(pnlBottom, rightZIndex);
                }
            }
            else
            {
                // Views in both panels exceed available space. Favor bottom panel over right panel.
                if (bottomZIndex < rightZIndex)
                {
                    pnlBottom.Parent.Controls.SetChildIndex(pnlBottom, rightZIndex);
                }
            }

            ResizeRightPanelViews();
        }

        private void ResizeRightPanelViews()
        {
            var rightPanelHeight = pnlRight.Height;
            var rightPanelControlsHeight = pnlRight.Controls.OfType<GridViewBase>().Sum(c => c.Height);
            var splitterHeights = pnlRight.Controls.OfType<Splitter>().Sum(c => c.Height);

            var viewHeightOverrun = (rightPanelControlsHeight + splitterHeights) - rightPanelHeight;

            if (viewHeightOverrun > 0)
            {
                // views too tall for panel.
                var gridViewControls = pnlRight.Controls.OfType<GridViewBase>();

                var panelCount = gridViewControls.Count();

                foreach (GridViewBase viewControl in gridViewControls)
                {
                    float proportionalHeight = (float)viewControl.Height / rightPanelControlsHeight;

                    var proportionalAdjustment = (viewHeightOverrun * proportionalHeight) + 1;

                    viewControl.Height -= (int)Math.Round(proportionalAdjustment, 0);
                }
            }
        }

        #endregion

        #region private [ display view data ]

        private async Task UpdateDataViewsAsync(bool refreshData = true)
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
                SetViewData(pnlBottom);
            if (pnlRight.Visible)
                SetViewData(pnlRight);
            if (pnlMain.Visible)
                SetViewData(pnlMain);
            if (pnlSchedules.Visible)
                SetViewData(pnlSchedules);
            if (pnlPitStops.Visible)
                SetViewData(pnlPitStops);

            DisplayHeaderData();
        }

        private void SetViewData(Panel panel)
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

            if (UserSettings.FastestLapsDisplayType == SpeedTimeType.MPH)
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
            IList<GenericGridViewModel> models;

            var displayType = viewType == GridViewTypes.Best5Laps || viewType == GridViewTypes.Best10Laps || viewType == GridViewTypes.Best15Laps ?
                UserSettings.BestNLapsDisplayType : UserSettings.LastNLapsDisplayType;

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

        #endregion

        #region private [ display header data ]

        private void DisplayHeaderData()
        {
            picFlagStatus.BackColor =
                _formState.LiveFeed.FlagState == (int)FlagColors.Green ? FlagUiColors.Green :
                _formState.LiveFeed.FlagState == (int)FlagColors.Yellow ? FlagUiColors.Yellow :
                _formState.LiveFeed.FlagState == (int)FlagColors.Red ? FlagUiColors.Red :
                _formState.LiveFeed.FlagState == (int)FlagColors.White ? FlagUiColors.White :
                _formState.LiveFeed.FlagState == (int)FlagColors.Checkered ? FlagUiColors.Checkered :
                _formState.LiveFeed.FlagState == (int)FlagColors.HotTrack ? FlagUiColors.HotTrack :
                _formState.LiveFeed.FlagState == (int)FlagColors.ColdTrack ? FlagUiColors.ColdTrack :
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

                if ((_viewState == ViewState.Race || _viewState == ViewState.PitStops) && _lapStates.Stage1Laps > 0 && _lapStates.Stage1Laps > 0)
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

            Application.DoEvents();
        }

        private void DisplayEventName(string runName, string seriesName, string trackName, int? stage1Laps = null, int? stage2Laps = null, int? stage3Laps = null)
        {
            var eventDetails = $"{runName} - {seriesName} - {trackName} ";

            var stageDetails = stage1Laps.HasValue && stage2Laps.HasValue && stage3Laps.HasValue ?
                $"({stage1Laps.Value}/{stage2Laps.Value}/{stage3Laps.Value})" :
                String.Empty;

            lblEventName.Text = $"{eventDetails} {stageDetails}".TrimEnd();
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
            FlagColors previousFlagState = FlagColors.Green;

            _lapStates.LapSegments.Clear();

            for (int i = 0; i < _formState.FlagStates.Count; i++)
            {
                if (_formState.FlagStates[i].State != previousFlagState)
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

                    previousFlagState = _formState.FlagStates[i].State;
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
                    FlagState = FlagColors.Checkered
                };

                _lapStates.LapSegments.Add(lastLapSegment);
            }

            picGreenYelllowLapIndicator.Invalidate();
        }

        #endregion

        #region private [ display files ]

        private void DisplayLogFile()
        {
            string currentLogFile = String.Format(LogFileName, DateTime.Now.ToString("yyyyMMdd")); ;

            string logFileDirectory = UserSettings.LogDirectory;

            string logFilePath = Path.Combine(logFileDirectory, currentLogFile);

            if (!File.Exists(logFilePath))
            {
                _logger.LogInformation($"Log file created {DateTime.Now}");
            }

            Process.Start("notepad.exe", logFilePath);
        }

        private void DisplayPatchNotesFile()
        {
            var installFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var patchFilePath = Path.Combine(installFolder, PatchFileName);

            if (File.Exists(patchFilePath))
                Process.Start("notepad.exe", patchFilePath);
        }

        #endregion

        #region private [ dump/load form state ]

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

                await UpdateDataViewsAsync();
            }
        }

        private string DumpFormState()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_formState, Formatting.Indented);

                var fileName = $"Dump {DateTime.Now:yyyy-MM-dd HHmmss.fff}.json";

                var dumpFilePath = Path.Combine(UserSettings.LogDirectory, fileName);

                File.WriteAllText(dumpFilePath, json);

                return fileName;
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        #endregion

        #region private [ fullscreen ]

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
                DisplayUserSettingsDialog();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private async void DisplayUserSettingsDialog()
        {
            var dialog = Program.Services.GetRequiredService<UserSettingsDialog>();

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK && dialog.UserSettingsHasChanges)
            {
                _logger.LogInformation("User settings updated");

                _userSettings = dialog.UserSettings;

                if (pnlMain.Controls.Count > 0)
                    pnlMain.Controls.Clear();

                if (pnlSchedules.Controls.Count > 0)
                    pnlSchedules.Controls.Clear();

                if (pnlPitStops.Controls.Count > 0)
                    pnlPitStops.Controls.Clear();

                var viewState = _viewState;

                await SetViewStateAsync(ViewState.None, true);

                await SetViewStateAsync(viewState, true);

                if (viewState == ViewState.Schedules)
                    await DisplayScheduleScreenAsync(_selectedScheduleType);
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

            await UpdateDataViewsAsync();
        }

        private void LoadStateFromJsonFile(string jsonFileName)
        {
            var json = File.ReadAllText(jsonFileName);

            _formState = JsonConvert.DeserializeObject<FormState>(json);

            File.Delete(jsonFileName);
        }

        private void UpdateReplayLabel(EventReplay replay, int index)
        {
            lblEventReplayStatus.Text = $"Replaying {replay.EventName} {replay.TrackName} {replay.Series} {replay.EventType} {replay.EventDate:yyyy-M-d} [Frame {index} of {replay.Frames.Count - 1}]";
        }

        #endregion

        #region private [ audio/video ]

        private void audioChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayAudioPlayer();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception displaying audio player from main form");
            }
        }

        private void DisplayAudioPlayer()
        {
            var dialog = Program.Services.GetRequiredService<AudioPlayer>();

            dialog.SeriesId = _formState.LiveFeed.SeriesId;

            dialog.Show(this);
        }

        private void inCarCamerasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayVideoPlayer();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception displaying video player from main form");
            }
        }

        private void DisplayVideoPlayer()
        {
            var dialog = Program.Services.GetRequiredService<VideoPlayer>();

            dialog.SeriesId = _formState.LiveFeed.SeriesId;

            dialog.Show(this);
        }

        private void multiViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayMultiView();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception multiple view video player from main form");
            }
        }

        private void DisplayMultiView()
        {
            var multiView = Program.Services.GetRequiredService<MultiView>();

            multiView.SeriesId = _formState.LiveFeed.SeriesId;

            multiView.Show();
        }

        #endregion

        #region private [ exception handling ]

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

        #endregion
    }
}