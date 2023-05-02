using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using rNascar23.Sdk.Common;
using rNascar23.CustomViews;
using rNascar23.Sdk.LiveFeeds.Ports;
using rNascar23.Sdk.LoopData.Ports;
using rNascar23.Sdk.Schedules.Models;
using rNascar23.Sdk.Schedules.Ports;
using rNascar23.Settings;
using rNascar23.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class ScheduleView : UserControl, IApiDataView<SeriesEvent>
    {
        #region fields

        private readonly IDriverInfoRepository _driverInfoRepository = null;
        private readonly IWeekendFeedRepository _weekendFeedRepository = null;
        private readonly ISchedulesRepository _raceScheduleRepository = null;
        private readonly ILogger<ScheduleView> _logger = null;
        private bool _useDarkTheme = false;
        private Color _alternateRowBackColor = Color.Gainsboro;

        #endregion

        #region properties

        public ApiSources ApiSource => ApiSources.Vehicles;
        public string Title => "Schedule";

        private IList<SeriesEvent> _data = new List<SeriesEvent>();
        public IList<SeriesEvent> Data
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
        public string Description { get; set; }
        private ScheduleType _scheduleType;
        public ScheduleType ScheduleType
        {
            get
            {
                return _scheduleType;
            }
            set
            {
                _scheduleType = value;

                SetTitle(ScheduleType);

                pnlHistoricalDataSelection.Visible = (_scheduleType == ScheduleType.Historical);
            }
        }

        #endregion

        #region ctor/load

        public ScheduleView()
            : this(ScheduleType.Unknown)
        {
        }

        public ScheduleView(ScheduleType scheduleType)
        {
            InitializeComponent();

            ScheduleType = scheduleType;

            this.Height = 225;

            _driverInfoRepository = Program.Services.GetRequiredService<IDriverInfoRepository>();
            _weekendFeedRepository = Program.Services.GetRequiredService<IWeekendFeedRepository>();
            _raceScheduleRepository = Program.Services.GetRequiredService<ISchedulesRepository>();
            _logger = Program.Services.GetRequiredService<ILogger<ScheduleView>>();
        }

        private void ScheduleView_Load(object sender, EventArgs e)
        {
            SetTitle(ScheduleType);

            SetTheme();

            PopulateSeasonSelector();
        }

        #endregion

        #region private

        private void PopulateSeasonSelector()
        {
            cboYear.DataSource = null;

            var seasons = new List<SeasonSelection>();

            for (int i = 2015; i <= DateTime.Now.Year; i++)
            {
                seasons.Add(new SeasonSelection()
                {
                    Year = i
                });
            }

            cboYear.DisplayMember = "Year";
            cboYear.ValueMember = "Year";
            cboYear.DataSource = seasons.ToList();

            cboYear.SelectedIndex = 0;
        }

        private void SetTheme()
        {
            var settings = UserSettingsService.LoadUserSettings();
            _useDarkTheme = settings.UseDarkTheme;

            Color backColor = Color.Empty;
            Color foreColor = Color.Empty;

            if (settings.UseDarkTheme)
            {
                backColor = Color.Black;
                foreColor = Color.Gainsboro;
                _alternateRowBackColor = Color.FromArgb(16, 16, 16);
            }
            else
            {
                backColor = Color.White;
                foreColor = Color.Black;
                _alternateRowBackColor = Color.Gainsboro;
            }

            tabEventScheduleResults.BackColor = backColor;
            flpScheduledEvents.BackColor = backColor;
            pnlEventWinnerAndComments.BackColor = backColor;
            lvSchedule.BackColor = backColor;
            lvResults.BackColor = backColor;

            pnlRight.BackColor = backColor;
            tlpCompletedEventDetails.BackColor = backColor;
            lblComments.BackColor = backColor;

            this.BackColor = backColor;

            tabEventScheduleResults.ForeColor = foreColor;
            pnlRight.ForeColor = foreColor;
            lvSchedule.ForeColor = foreColor;
            lvResults.ForeColor = foreColor;
            tlpCompletedEventDetails.ForeColor = foreColor;
            lblComments.ForeColor = foreColor;
        }

        private void SetTitle(ScheduleType scheduleType)
        {
            switch (scheduleType)
            {
                case ScheduleType.Cup:
                    lblTitle.Text = "NASCAR Cup Series Schedule";
                    break;
                case ScheduleType.Xfinity:
                    lblTitle.Text = "NASCAR Xfinity Series Schedule";
                    break;
                case ScheduleType.Trucks:
                    lblTitle.Text = "NASCAR Craftsman Truck Series Schedule";
                    break;
                case ScheduleType.All:
                    lblTitle.Text = "Schedule (All Series)";
                    break;
                case ScheduleType.ThisWeek:
                    lblTitle.Text = "This Week's Schedule";
                    break;
                case ScheduleType.NextWeek:
                    lblTitle.Text = "Next Week's Schedule";
                    break;
                case ScheduleType.Today:
                    lblTitle.Text = "Today's Schedule";
                    break;
                default:
                    lblTitle.Text = "Schedule";
                    break;
            }
        }

        private void SetDataSource<T>(IList<T> values)
        {
            try
            {
                var viewModels = BuildViewModels((IList<SeriesEvent>)values);

                flpScheduledEvents.SuspendLayout();

                var viewArray = flpScheduledEvents.Controls.OfType<ScheduledEventView>().ToArray();

                if (viewArray.Length > viewModels.Count)
                {
                    for (int x = viewModels.Count; x < viewArray.Length; x++)
                    {
                        viewArray[x].Visible = false;
                    }
                }

                for (int i = 0; i < viewModels.Count; i++)
                {
                    var viewModel = viewModels[i];
                    ScheduledEventView view = null;

                    if (viewArray.Length >= i + 1)
                    {
                        // control exists
                        view = viewArray[i];
                        view.Visible = true;
                    }
                    else
                    {
                        view = new ScheduledEventView();
                        flpScheduledEvents.Controls.Add(view);

                        view.ViewSelected += View_ViewSelected;
                    }

                    if (flpScheduledEvents.Controls.OfType<ScheduledEventView>().Count() == 1)
                    {
                        view = flpScheduledEvents.Controls.OfType<ScheduledEventView>().ToList()[0];
                        flpScheduledEvents.Width = view.Width + 28;
                    }

                    view.ViewModel = viewModel;
                }

                // Auto-display first event schedule if any
                if (flpScheduledEvents.Controls.OfType<ScheduledEventView>().Count() >= 1)
                {
                    var view = flpScheduledEvents.Controls.OfType<ScheduledEventView>().ToList()[0];

                    View_ViewSelected(view, new ScheduledEventView.ViewSelectedEventArgs());
                }
            }
            catch (Exception ex) { ExceptionHandler(ex); }
            finally
            {
                flpScheduledEvents.ResumeLayout();
            }
        }

        private IList<ScheduledEventViewModel> BuildViewModels(IList<SeriesEvent> seriesEvents)
        {
            IList<ScheduledEventViewModel> scheduledEvents = new List<ScheduledEventViewModel>();

            foreach (var seriesEvent in seriesEvents.OrderBy(e => e.DateScheduled))
            {
                var viewModel = new ScheduledEventViewModel()
                {
                    RaceId = seriesEvent.RaceId,
                    EventName = seriesEvent.RaceName,
                    TrackName = seriesEvent.TrackName,
                    TrackCityState = "",
                    EventLaps = seriesEvent.ScheduledLaps.ToString(),
                    EventMiles = seriesEvent.ScheduledDistance.ToString(),
                    EventDateTime = seriesEvent.RaceDate,
                    Series = seriesEvent.SeriesId,
                    Tv = seriesEvent.TelevisionBroadcaster == "FOX" ? TvTypes.Fox :
                        seriesEvent.TelevisionBroadcaster == "FS1" ? TvTypes.FS1 :
                        seriesEvent.TelevisionBroadcaster == "FS2" ? TvTypes.FS2 :
                        seriesEvent.TelevisionBroadcaster == "NBC" ? TvTypes.NBC :
                        seriesEvent.TelevisionBroadcaster == "NBCSN" ? TvTypes.NBCSN :
                        seriesEvent.TelevisionBroadcaster == "CNBC" ? TvTypes.CNBC :
                        seriesEvent.TelevisionBroadcaster == "USA" ? TvTypes.USA :
                        TvTypes.Unknown,
                    Radio = seriesEvent.RadioBroadcaster == "MRN" ? RadioTypes.MRN :
                        seriesEvent.RadioBroadcaster == "PRN" ? RadioTypes.PRN :
                        seriesEvent.RadioBroadcaster == "IMS" ? RadioTypes.IMS :
                        RadioTypes.Unknown,
                    Satellite = seriesEvent.SatelliteRadioBroadcaster == "SIRIUSXM" ? SatelliteTypes.Sirius :
                        SatelliteTypes.Unknown
                };

                scheduledEvents.Add(viewModel);
            }

            return scheduledEvents;
        }

        /* historical */
        private async void btnDisplayHistoricalData_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboYear.SelectedItem == null)
                    return;

                var selectedSeason = cboYear.SelectedItem as SeasonSelection;

                var seriesSchedules = await _raceScheduleRepository.GetSchedulesAsync(selectedSeason.Year);

                IEnumerable<SeriesEvent> schedules = new List<SeriesEvent>();

                if (chkCup.Checked)
                {
                    schedules = seriesSchedules.CupSeries;
                }
                if (chkXfinity.Checked)
                {
                    schedules = schedules.Concat(seriesSchedules.XfinitySeries);
                }
                if (chkTruck.Checked)
                {
                    schedules = schedules.Concat(seriesSchedules.TruckSeries);
                }

                Data = schedules.ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void seriesSelection_CheckChanged(object sender, EventArgs e)
        {
            btnDisplayHistoricalData.Enabled = chkCup.Checked || chkXfinity.Checked || chkTruck.Checked;
        }

        /* view selection */
        private async void View_ViewSelected(object sender, ScheduledEventView.ViewSelectedEventArgs e)
        {
            try
            {
                var selectedView = sender as ScheduledEventView;

                var selectedViewModel = selectedView.ViewModel;

                if (!e.MultiSelect)
                {
                    foreach (ScheduledEventView view in flpScheduledEvents.Controls.OfType<ScheduledEventView>())
                    {
                        view.Selected = false;
                    }
                }

                selectedView.Selected = true;

                ClearEventResults();

                ClearEventDetails();

                ClearEventActivities();

                IEnumerable<EventActivity> compositeSchedule = new List<EventActivity>();

                foreach (ScheduledEventView view in flpScheduledEvents.
                    Controls.
                    OfType<ScheduledEventView>().
                    Where(v => v.Selected == true))
                {
                    var compositeViewModel = view.ViewModel;

                    var seriesEvent = _data.FirstOrDefault(d => d.RaceId == compositeViewModel.RaceId);

                    compositeSchedule = compositeSchedule.Concat(seriesEvent.EventActivities);
                }

                DisplayEventSchedule(compositeSchedule.ToArray(), true);

                if (!e.MultiSelect)
                {
                    var seriesEvent = _data.FirstOrDefault(d => d.RaceId == selectedViewModel.RaceId);

                    await DisplayEventDetailsAsync(seriesEvent);

                    await DisplayEventResultsAsync(seriesEvent);
                }
            }
            catch (Exception ex) { ExceptionHandler(ex); }
        }

        /* Event Details (bottom) */
        private async Task DisplayEventDetailsAsync(SeriesEvent seriesEvent)
        {
            try
            {
                pnlEventWinnerAndComments.SuspendLayout();

                if (seriesEvent.WinnerDriverId == null)
                {
                    pnlEventWinnerAndComments.Visible = false;

                    lvResults.Items.Clear();

                    tabEventScheduleResults.SelectedIndex = 0;
                }
                else
                {
                    pnlEventWinnerAndComments.Visible = true;

                    var raceWinningDriver = await GetDriverNameAsync(seriesEvent.WinnerDriverId.GetValueOrDefault(0));

                    var poleWinningDriver = await GetDriverNameAsync(seriesEvent.PoleWinnerDriverId);

                    lblWinner.Text = $"{raceWinningDriver}";

                    lblLeaders.Text = $"{seriesEvent.NumberOfLeaders}";
                    lblLeadChanges.Text = $"{seriesEvent.NumberOfLeadChanges}";
                    lblCautions.Text = $"{seriesEvent.NumberOfCautions}";
                    lblCautionLaps.Text = $"{seriesEvent.NumberOfCautionLaps}";
                    lblPoleWinner.Text = $"{poleWinningDriver}";
                    lblPoleSpeed.Text = $"{seriesEvent.PoleWinnerSpeed.ToString("N3")}";
                    lblAverageSpeed.Text = $"{seriesEvent.AverageSpeed.ToString("N3")}";
                    lblMargin.Text = $"{seriesEvent.MarginOfVictory}";
                    lblRaceTime.Text = $"{seriesEvent.TotalRaceTime}";
                    lblCarsInField.Text = $"{seriesEvent.NumberOfCarsInField}";

                    lblComments.Text = seriesEvent.RaceComments;

                    tabEventScheduleResults.SelectedIndex = 1;
                }
            }
            catch (Exception ex) { ExceptionHandler(ex); }
            finally
            {
                pnlEventWinnerAndComments.ResumeLayout();
            }
        }
        private void ClearEventDetails()
        {
            pnlEventWinnerAndComments.Visible = false;

            lvResults.Items.Clear();

            tabEventScheduleResults.SelectedIndex = 0;
        }

        /* Event Results (tab control) */
        private async Task DisplayEventResultsAsync(SeriesEvent seriesEvent)
        {
            try
            {
                SeriesTypes seriesId = seriesEvent.SeriesId;
                int raceId = seriesEvent.RaceId;

                lvResults.BeginUpdate();

                lvResults.Items.Clear();

                var results = await GetEventResultsAsync(seriesId, raceId);

                int i = 0;

                foreach (var result in results)
                {
                    var lvi = new ListViewItem(result.FinishingPosition.ToString());

                    lvi.SubItems.Add(result.CarNumber);
                    lvi.SubItems.Add(result.Driver);
                    lvi.SubItems.Add(result.Hometown);
                    lvi.SubItems.Add(result.Vehicle);
                    lvi.SubItems.Add(result.StartingPosition.ToString());
                    lvi.SubItems.Add(result.LapsCompleted.ToString());
                    lvi.SubItems.Add(result.FinishingStatus);
                    lvi.SubItems.Add(result.LapsLed.ToString());
                    lvi.SubItems.Add(result.PointsEarned.ToString());
                    lvi.SubItems.Add(result.PlayoffPointsEarned.ToString());
                    lvi.SubItems.Add(result.Sponsor);
                    lvi.SubItems.Add(result.Owner);
                    lvi.SubItems.Add(result.CrewChief);

                    lvi.Tag = result;

                    if (i % 2 == 0)
                    {
                        lvi.BackColor = _alternateRowBackColor;
                    }

                    i++;

                    lvResults.Items.Add(lvi);
                }
            }
            catch (Exception ex) { ExceptionHandler(ex); }
            finally
            {
                lvResults.EndUpdate();
            }
        }
        private async Task<string> GetDriverNameAsync(int driverId)
        {
            var driver = await _driverInfoRepository.GetDriverAsync(driverId);

            return driver == null ? "None" : driver.Name;
        }
        private void ClearEventResults()
        {
            try
            {
                lvResults.BeginUpdate();

                lvResults.Items.Clear();
            }
            catch (Exception ex) { ExceptionHandler(ex); }
            finally
            {
                lvResults.EndUpdate();
            }
        }
        private async Task<IList<EventResult>> GetEventResultsAsync(SeriesTypes seriesId, int raceId)
        {
            IList<EventResult> eventResults = new List<EventResult>();

            int year;

            if (ScheduleType == ScheduleType.Historical)
            {
                var selectedSeason = cboYear.SelectedItem as SeasonSelection;

                year = selectedSeason == null ? DateTime.Now.Year : selectedSeason.Year;
            }
            else
            {
                year = DateTime.Now.Year;
            }

            var weekendFeed = await _weekendFeedRepository.GetWeekendFeedAsync(seriesId, raceId, year);

            var weekendRace = weekendFeed.WeekendRaces.FirstOrDefault();

            var weekendRaceResults = weekendRace.
                Results.
                Where(r => r.FinishingPosition > 0).
                OrderBy(r => r.FinishingPosition);

            foreach (Sdk.LiveFeeds.Models.Result driverResult in weekendRaceResults)
            {
                var eventResult = new EventResult()
                {
                    FinishingPosition = driverResult.FinishingPosition,
                    CarNumber = driverResult.CarNumber,
                    Driver = driverResult.DriverFullName,
                    Vehicle = driverResult.CarModel,
                    Hometown = driverResult.DriverHometown,
                    Sponsor = driverResult.Sponsor,
                    Owner = driverResult.OwnerFullName,
                    FinishingStatus = driverResult.FinishingStatus,
                    CrewChief = driverResult.CrewChiefFullName,
                    LapsLed = driverResult.LapsLed,
                    LapsCompleted = driverResult.LapsCompleted,
                    PointsEarned = driverResult.PointsEarned,
                    PlayoffPointsEarned = driverResult.PlayoffPointsEarned,
                    StartingPosition = driverResult.StartingPosition,
                };

                eventResults.Add(eventResult);
            }

            return eventResults;
        }

        /* Event Activities (tab control)  */
        private void DisplayEventSchedule(EventActivity[] schedule, bool isCompositeSchedule = false)
        {
            try
            {
                lvSchedule.SuspendLayout();

                lvSchedule.Items.Clear();

                int lviNextActivityIdx = -1;

                foreach (var item in schedule.OrderBy(s => s.StartTimeLocal))
                {
                    var groupHeader = item.StartTimeLocal.Date.ToString("dddd, MMMM d yyyy");

                    var group = lvSchedule.Groups.Cast<ListViewGroup>()
                        .FirstOrDefault(g => g.Header == groupHeader);

                    if (group == null)
                    {
                        group = new ListViewGroup(groupHeader);
                        lvSchedule.Groups.Add(group);
                    }

                    var lvi = new ListViewItem();

                    lvi.UseItemStyleForSubItems = false;

                    if (item.SeriesId > 0 && (int)item.SeriesId < 4)
                        lvi.ImageIndex = ((int)item.SeriesId - 1);
                    else
                        lvi.ImageIndex = -1;

                    lvi.SubItems.Add(item.StartTimeLocal.ToShortTimeString());
                    lvi.SubItems.Add(item.EventName);
                    lvi.SubItems.Add(item.Notes);
                    lvi.SubItems.Add(item.Description);

                    lvi.Group = group;

                    foreach (ListViewItem.ListViewSubItem subItem in lvi.SubItems)
                    {
                        if (item.StartTimeLocal < DateTime.Now)
                        {
                            if (_useDarkTheme)
                                subItem.ForeColor = Color.Gray;
                            else
                                subItem.ForeColor = Color.DimGray;
                        }
                        else
                        {
                            if (lviNextActivityIdx == -1)
                                lviNextActivityIdx = lvSchedule.Items.Count;

                            if (_useDarkTheme)
                                subItem.ForeColor = Color.White;
                            else
                                subItem.ForeColor = Color.Black;
                        }
                    }

                    lvSchedule.Items.Add(lvi);
                }

                if (lvSchedule.Items.Count > lviNextActivityIdx + 6)
                {
                    lviNextActivityIdx += 6;
                }
                else
                {
                    lviNextActivityIdx = lvSchedule.Items.Count - 1;
                }

                lvSchedule.Items[lviNextActivityIdx].EnsureVisible();
            }
            catch (Exception ex) { ExceptionHandler(ex); }
            finally
            {
                lvSchedule.ResumeLayout(true);
            }
        }
        private void ClearEventActivities()
        {
            try
            {
                lvSchedule.SuspendLayout();

                lvSchedule.Items.Clear();
            }
            catch (Exception ex) { ExceptionHandler(ex); }
            finally
            {
                lvSchedule.ResumeLayout(true);
            }
        }

        /* paint */
        private void tabEventScheduleResults_DrawItem(object sender, DrawItemEventArgs e)
        {
            // This event is called once for each tab button in your tab control

            // First paint the background with a color based on the current tab

            // e.Index is the index of the tab in the TabPages collection.
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(Color.RoyalBlue), e.Bounds);
                    break;
                case 1:
                    e.Graphics.FillRectangle(new SolidBrush(Color.Blue), e.Bounds);
                    break;
                default:
                    break;
            }

            // Then draw the current tab button text 
            Rectangle paddedBounds = e.Bounds;
            paddedBounds.Inflate(-2, -4);
            using (var tabFont = new Font("Segoe UI", 11, FontStyle.Bold))
            {
                e.Graphics.DrawString(
                    tabEventScheduleResults.TabPages[e.Index].Text,
                    tabFont,
                    SystemBrushes.HighlightText,
                    paddedBounds);
            }
        }

        private void ExceptionHandler(Exception ex, string message = "")
        {
            string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

            _logger.LogError(ex, errorMessage);
        }

        #endregion

        #region classes

        private class EventResult
        {
            public int FinishingPosition { get; set; }
            public string CarNumber { get; set; }
            public string Driver { get; set; }
            public string Vehicle { get; set; }
            public string Hometown { get; set; }
            public string Sponsor { get; set; }
            public string Owner { get; set; }
            public string CrewChief { get; set; }
            public string FinishingStatus { get; set; }
            public int StartingPosition { get; set; }
            public int LapsLed { get; set; }
            public int LapsCompleted { get; set; }
            public int PointsEarned { get; set; }
            public int PlayoffPointsEarned { get; set; }
        }

        private class SeasonSelection
        {
            public int Year { get; set; }
        }

        #endregion
    }
}
