using rNasar23.Common;
using rNascar23.Properties;
using rNascar23.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class DriverPitStopView : UserControl
    {
        #region fields

        private Color _selectedBoxColor = Color.Gold;
        private int _selectionBoxThickness = 4;

        #endregion

        #region properties

        private DriverPitStopViewModel _viewModel = null;
        public DriverPitStopViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                _viewModel = value;

                DisplayDriverPitStop(_viewModel);
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

        #region ctor

        public DriverPitStopView()
        {
            InitializeComponent();
        }

        #endregion

        #region private

        private void ClearDisplay()
        {
            lblRunningPosition.Text = "";
            lblDriver.Text = "";
            lblPitLap.Text = "";
            lblPositionIn.Text = "";
            lblPositionOut.Text = "";
            lblDelta.Text = "";
            lblPitStopTime.Text = "";
            picTires.Image = null;
        }

        private void DisplayDriverPitStop(DriverPitStopViewModel viewModel)
        {
            if (viewModel == null)
            {
                ClearDisplay();

                return;
            }

            lblRunningPosition.Text = viewModel.RunningPosition.ToString();
            lblDriver.Text = viewModel.DriverName;
            lblPitLap.Text = viewModel.PitOnLap.ToString();
            lblPositionIn.Text = viewModel.PositionIn.ToString();
            lblPositionOut.Text = viewModel.PositionOut.ToString();
            lblDelta.Text = viewModel.PositionDelta.ToString();
            if (viewModel.PositionDelta < 0)
            {
                lblDelta.ForeColor = Color.Red;
            }
            else if (viewModel.PositionDelta > 0)
            {
                lblDelta.ForeColor = Color.Green;
            }
            else
            {
                lblDelta.ForeColor = Color.Black;
            }

            lblPitStopTime.Text = viewModel.PitStopTime.ToString("N2");

            if (viewModel.Changes == PitStopChanges.Other)
            {
                picTires.Image = Resources.TransparentPixel;
            }
            else if (viewModel.Changes == PitStopChanges.LeftSide)
            {
                picTires.Image = Resources.LeftTires;
            }
            else if (viewModel.Changes == PitStopChanges.RightSide)
            {
                picTires.Image = Resources.RightTires;
            }
            else if (viewModel.Changes == PitStopChanges.FourTires)
            {
                picTires.Image = Resources.FourTires;
            }
            else
            {
                picTires.Image = Resources.TransparentPixel;
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
        private void View_Paint(object sender, PaintEventArgs e)
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
