using rNascar23.Properties;
using rNascar23.ViewModels;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class ScheduledEventView : UserControl
    {
        #region fields

        private Color _selectedBoxColor = Color.Gold;
        private int _selectionBoxThickness = 4;

        #endregion

        #region properties

        private ScheduledEventViewModel _viewModel = null;
        public ScheduledEventViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                _viewModel = value;

                if (_viewModel == null)
                    ClearScheduledEvent();
                else
                    DisplayScheduledEvent(_viewModel);
            }
        }
        private bool _selected = false;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;

                this.Invalidate();
            }
        }
        #endregion

        #region ctol/load

        public ScheduledEventView()
        {
            InitializeComponent();
        }

        private void ScheduledEventView_Load(object sender, System.EventArgs e)
        {
            if (_viewModel == null)
                ClearScheduledEvent();
            else
                DisplayScheduledEvent(_viewModel);
        }

        #endregion

        #region private

        private void ClearScheduledEvent()
        {
            picSeries.Image = null;
            lblEventDate.Text = "";
            lblEventTime.Text = "";

            lblEventName.Text = "";
            lblTrack.Text = "";
            lblEventDistance.Text = "";

            picTv.Image = null;
            picRadio.Image = null;
            picSatellite.Image = null;
        }

        private void DisplayScheduledEvent(ScheduledEventViewModel viewModel)
        {
            switch (viewModel.Series)
            {
                case SeriesTypes.Cup:
                    picSeries.Image = Resources.NCS_Small;
                    break;
                case SeriesTypes.XFinity:
                    picSeries.Image = Resources.XFinity_Small;
                    break;
                case SeriesTypes.Truck:
                    picSeries.Image = Resources.CTS_Small;
                    break;
                default:
                    picSeries.Image = null;
                    break;
            }

            lblEventDate.Text = viewModel.EventDateTime.ToLocalTime().ToString("dddd, MMM d");
            lblEventTime.Text = viewModel.EventDateTime.ToLocalTime().ToString("H:mm tt");

            lblEventName.Text = viewModel.EventName;
            lblTrack.Text = $"{viewModel.TrackName} {viewModel.TrackCityState}";
            lblEventDistance.Text = $"{viewModel.EventLaps} Laps/{viewModel.EventMiles} Miles";

            switch (viewModel.Tv)
            {
                case TvTypes.Fox:
                    picTv.Image = Resources.FOX_Small;
                    break;
                case TvTypes.FS1:
                    picTv.Image = Resources.FS1_Small;
                    break;
                case TvTypes.FS2:
                    picTv.Image = Resources.FS2_Small;
                    break;
                case TvTypes.USA:
                    picTv.Image = Resources.USA_Small;
                    break;
                case TvTypes.NBC:
                    picTv.Image = Resources.NBC_Small;
                    break;
                default:
                    picTv.Image = null;
                    break;
            }

            switch (viewModel.Radio)
            {
                case RadioTypes.MRN:
                    picRadio.Image = Resources.MRN_Small;
                    break;
                case RadioTypes.PRN:
                    picRadio.Image = Resources.PRN_Small;
                    break;
                default:
                    picRadio.Image = null;
                    break;
            }

            switch (viewModel.Satellite)
            {
                case SatelliteTypes.Sirius:
                    picSatellite.Image = Resources.Sirius_Small;
                    break;
                default:
                    picSatellite.Image = null;
                    break;
            }

            if (viewModel.EventDateTime.Date < DateTime.Now.Date)
            {
                lblEventDate.ForeColor = Color.Silver;
                lblEventTime.ForeColor = Color.Silver;

                lblEventName.ForeColor = Color.Silver;
                lblTrack.ForeColor = Color.Silver;
                lblEventDistance.ForeColor = Color.Silver;
            }
        }

        #endregion

        #region events

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks the view")]
        public event EventHandler ViewSelected;

        private void View_Selected(object sender, EventArgs e)
        {
            if (this.ViewSelected != null)
                this.ViewSelected(this, e);
        }

        [Browsable(false)]
        private void ScheduledEventView_Paint(object sender, PaintEventArgs e)
        {
            if (_selected)
            {
                using (var myPen = new Pen(_selectedBoxColor, (float)_selectionBoxThickness))
                {
                    var area = new Rectangle(
                        new Point(_selectionBoxThickness - 1, _selectionBoxThickness - 1),
                        new Size(this.Size.Width - (_selectionBoxThickness * 2), this.Size.Height - (_selectionBoxThickness * 2)));

                    e.Graphics.DrawRectangle(myPen, area);
                }
            }
        }

        #endregion
    }
}
