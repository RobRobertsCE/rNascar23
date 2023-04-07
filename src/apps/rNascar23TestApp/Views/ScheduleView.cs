using Microsoft.Extensions.DependencyInjection;
using rNascar23.DriverStatistics.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.Schedules.Models;
using rNascar23.CustomViews;
using rNascar23.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using rNascar23.Common;

namespace rNascar23.Views
{
    public partial class ScheduleView : UserControl, IApiDataView<SeriesEvent>
    {
        #region fields

        private readonly IDriverInfoRepository _driverInfoRepository = null;

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
        public ScheduleType ScheduleType { get; set; }

        #endregion

        #region ctor/load

        public ScheduleView()
            : this(ScheduleType.Unknown)
        {
            InitializeComponent();

            this.Height = 225;
        }

        public ScheduleView(ScheduleType scheduleType)
        {
            InitializeComponent();

            ScheduleType = scheduleType;

            this.Height = 225;

            _driverInfoRepository = Program.Services.GetRequiredService<IDriverInfoRepository>();
        }

        private void ScheduleView_Load(object sender, EventArgs e)
        {
            SetTitle(ScheduleType);

            SetTheme();
        }

        #endregion

        #region private

        private void SetTheme()
        {
            var settings = UserSettingsService.LoadUserSettings();

            Color backColor = Color.Empty;
            Color foreColor = Color.Empty;

            if (settings.UseDarkTheme)
            {
                backColor = Color.Black;
                foreColor = Color.Gainsboro;
            }
            else
            {
                backColor = Color.White;
                foreColor = Color.Black;
            }

            flpScheduledEvents.BackColor = backColor;
            pnlEventWinnerAndComments.BackColor = backColor;
            lvSchedule.BackColor = backColor;

            pnlRight.BackColor = backColor;
            tlpCompletedEventDetails.BackColor = backColor;
            lblComments.BackColor = backColor;

            this.BackColor = backColor;

            pnlRight.ForeColor = foreColor;
            lvSchedule.ForeColor = foreColor;
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
            var viewModels = BuildViewModels((IList<SeriesEvent>)values);

            for (int i = 0; i < viewModels.Count; i++)
            {
                var viewModel = viewModels[i];
                ScheduledEventView view = null;

                if (flpScheduledEvents.Controls.OfType<ScheduledEventView>().Count() >= i + 1)
                {
                    // control exists
                    view = flpScheduledEvents.Controls.OfType<ScheduledEventView>().ToList()[i];
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

                view.Selected = true;

                View_ViewSelected(view, EventArgs.Empty);
            }
        }

        private async void View_ViewSelected(object sender, EventArgs e)
        {
            var selectedView = sender as ScheduledEventView;

            var selectedViewModel = selectedView.ViewModel;

            var seriesEvent = _data.FirstOrDefault(d => d.RaceId == selectedViewModel.RaceId);

            DisplayEventSchedule(seriesEvent.Schedule);

            await DisplayEventDetailsAsync(seriesEvent);

            foreach (ScheduledEventView view in flpScheduledEvents.Controls.OfType<ScheduledEventView>())
            {
                view.Selected = false;
            }

            selectedView.Selected = true;
        }

        private async Task DisplayEventDetailsAsync(SeriesEvent seriesEvent)
        {
            if (seriesEvent.WinnerDriverId == null)
            {
                pnlEventWinnerAndComments.Visible = false;
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
            }
        }

        private async Task<string> GetDriverNameAsync(int driverId)
        {
            var driver = await _driverInfoRepository.GetDriverAsync(driverId);

            return driver == null ? "None" : driver.Name;
        }

        private void DisplayEventSchedule(Schedule[] schedule)
        {
            try
            {
                lvSchedule.SuspendLayout();

                lvSchedule.Items.Clear();

                foreach (var item in schedule.OrderBy(s => s.StartTimeLocal))
                {
                    var groupHeader = item.StartTimeLocal.Date.ToString("dddd, MMMM d");

                    var group = lvSchedule.Groups.Cast<ListViewGroup>()
                        .FirstOrDefault(g => g.Header == groupHeader);

                    if (group == null)
                    {
                        group = new ListViewGroup(groupHeader);
                        lvSchedule.Groups.Add(group);
                    }

                    var lvi = new ListViewItem();

                    lvi.SubItems.Add(item.StartTimeLocal.ToShortTimeString());
                    lvi.SubItems.Add(item.EventName);
                    lvi.SubItems.Add(item.Notes);
                    lvi.SubItems.Add(item.Description);

                    lvi.Group = group;

                    lvSchedule.Items.Add(lvi);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                lvSchedule.ResumeLayout(true);
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
                    TrackCityState = "-",
                    EventLaps = seriesEvent.ScheduledLaps.ToString(),
                    EventMiles = seriesEvent.ScheduledDistance.ToString(),
                    EventDateTime = seriesEvent.RaceDate,
                    Series = seriesEvent.SeriesId == 1 ? SeriesTypes.Cup :
                        seriesEvent.SeriesId == 2 ? SeriesTypes.XFinity :
                        seriesEvent.SeriesId == 3 ? SeriesTypes.Truck :
                        SeriesTypes.Unknown,
                    Tv = seriesEvent.TelevisionBroadcaster == "FOX" ? TvTypes.Fox :
                        seriesEvent.TelevisionBroadcaster == "FS1" ? TvTypes.FS1 :
                        seriesEvent.TelevisionBroadcaster == "FS2" ? TvTypes.FS2 :
                        seriesEvent.TelevisionBroadcaster == "NBC" ? TvTypes.NBC :
                        seriesEvent.TelevisionBroadcaster == "USA" ? TvTypes.USA :
                        TvTypes.Unknown,
                    Radio = seriesEvent.RadioBroadcaster == "MRN" ? RadioTypes.MRN :
                        seriesEvent.RadioBroadcaster == "PRN" ? RadioTypes.PRN :
                        RadioTypes.Unknown,
                    Satellite = seriesEvent.SatelliteRadioBroadcaster == "SIRIUSXM" ? SatelliteTypes.Sirius :
                        SatelliteTypes.Unknown
                };

                scheduledEvents.Add(viewModel);
            }

            return scheduledEvents;
        }

        #endregion
    }
}
