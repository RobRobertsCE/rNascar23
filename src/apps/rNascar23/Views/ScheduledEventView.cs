using rNascar23.Sdk.Common;
using rNascar23.Properties;
using rNascar23.Settings;
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
        private UserSettings _settings = null;
        private bool _multiSelected = false;

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
            SetTheme();

            if (_viewModel == null)
                ClearScheduledEvent();
            else
                DisplayScheduledEvent(_viewModel);
        }

        #endregion

        #region private

        private void SetTheme()
        {
            _settings = UserSettingsService.LoadUserSettings();

            Color backColor = Color.Empty;
            Color foreColor = Color.Empty;

            if (_settings.UseDarkTheme)
            {
                backColor = Color.Black;
                foreColor = Color.Gainsboro;
            }
            else
            {
                backColor = Color.White;
                foreColor = Color.Black;
            }

            lblEventDate.BackColor = backColor;
            lblEventTime.BackColor = backColor;
            lblEventName.BackColor = backColor;
            lblTrack.BackColor = backColor;
            lblEventDistance.BackColor = backColor;
            lblTv.BackColor = backColor;
            lblRadio.BackColor = backColor;
            lblSatellite.BackColor = backColor;

            this.BackColor = backColor;

            lblEventDate.ForeColor = foreColor;
            lblEventTime.ForeColor = foreColor;
            lblEventName.ForeColor = foreColor;
            lblTrack.ForeColor = foreColor;
            lblEventDistance.ForeColor = foreColor;
            lblTv.ForeColor = foreColor;
            lblRadio.ForeColor = foreColor;
            lblSatellite.ForeColor = foreColor;
            picTv.BackColor = Color.White;
            picRadio.BackColor = Color.White;
            picSatellite.BackColor = Color.White;
        }

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
            try
            {
                this.SuspendLayout();

                switch (viewModel.Series)
                {
                    case SeriesTypes.Cup:
                        picSeries.Image = Resources.NCS_Small;
                        break;
                    case SeriesTypes.Xfinity:
                        picSeries.Image = Resources.XFinity_Small;
                        break;
                    case SeriesTypes.Truck:
                        picSeries.Image = Resources.CTS_Small;
                        break;
                    default:
                        picSeries.Image = null;
                        break;
                }

                toolTip1.SetToolTip(picSeries, $"{viewModel.Series} Series");

                lblEventDate.Text = viewModel.EventDateTime.ToString("dddd, MMM d yyyy");
                lblEventTime.Text = viewModel.EventDateTime.ToString("h:mm tt");

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
                    case TvTypes.NBCSN:
                        picTv.Image = Resources.NBCSN_logo;
                        break;
                    case TvTypes.CNBC:
                        picTv.Image = Resources.CNBC_logo_small;
                        break;
                    default:
                        picTv.Image = null;
                        break;
                }

                toolTip1.SetToolTip(picTv, viewModel.Tv.ToString());

                switch (viewModel.Radio)
                {
                    case RadioTypes.MRN:
                        picRadio.Image = Resources.MRN_Small;
                        break;
                    case RadioTypes.PRN:
                        picRadio.Image = Resources.PRN_Small;
                        break;
                    case RadioTypes.IMS:
                        picRadio.Image = Resources.IMS_radio_small;
                        break;
                    default:
                        picRadio.Image = null;
                        break;
                }

                toolTip1.SetToolTip(picRadio, viewModel.Radio.ToString());

                switch (viewModel.Satellite)
                {
                    case SatelliteTypes.Sirius:
                        picSatellite.Image = Resources.Sirius_Small;
                        break;
                    default:
                        picSatellite.Image = null;
                        break;
                }

                toolTip1.SetToolTip(picSatellite, viewModel.Satellite.ToString());

                if (viewModel.EventDateTime.Date < DateTime.Now.Date)
                {
                    Color foreColor = Color.Empty;

                    if (_settings.UseDarkTheme)
                    {
                        foreColor = Color.DimGray;
                    }
                    else
                    {
                        foreColor = Color.Silver;
                    }

                    lblEventDate.ForeColor = foreColor;
                    lblEventTime.ForeColor = foreColor;

                    lblEventName.ForeColor = foreColor;
                    lblTrack.ForeColor = foreColor;
                    lblEventDistance.ForeColor = foreColor;
                }
                else
                {
                    Color foreColor = Color.Empty;

                    if (_settings.UseDarkTheme)
                    {
                        foreColor = Color.White;
                    }
                    else
                    {
                        foreColor = Color.Black;
                    }

                    lblEventDate.ForeColor = foreColor;
                    lblEventTime.ForeColor = foreColor;

                    lblEventName.ForeColor = foreColor;
                    lblTrack.ForeColor = foreColor;
                    lblEventDistance.ForeColor = foreColor;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.ResumeLayout();
            }
        }

        #endregion

        #region events

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks the view")]
        public event EventHandler<ViewSelectedEventArgs> ViewSelected;

        private void View_Selected(object sender, EventArgs e)
        {
            if (this.ViewSelected != null)
                this.ViewSelected(this, new ViewSelectedEventArgs()
                {
                    MultiSelect = _multiSelected
                });

            _multiSelected = false;
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

        private void ScheduledEventView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (ModifierKeys & Keys.Control) == Keys.Control)
            {
                //MessageBox.Show("Control key was held down.");
                _multiSelected = true;
            }
        }

        #endregion

        #region classes

        public class ViewSelectedEventArgs : EventArgs
        {
            public bool MultiSelect { get; set; }
        }

        #endregion
    }
}
