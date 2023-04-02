using System;
using System.Windows.Forms;

namespace rNascar23TestApp.Dialogs
{
    public partial class NewScreenNameDialog : Form
    {
        #region properties

        private string _screenName = String.Empty;
        public string ScreenName
        {
            get
            {
                return _screenName;
            }
            set
            {
                _screenName = value;
            }
        }

        #endregion

        #region ctor/load

        public NewScreenNameDialog(string currentScreenName)
            : this()
        {
            _screenName = currentScreenName;
        }

        public NewScreenNameDialog()
        {
            InitializeComponent();
        }

        private void NewScreenNameDialog_Load(object sender, EventArgs e)
        {
            txtScreenName.Text = _screenName;
        }

        #endregion

        #region private

        private void txtScreenName_TextChanged(object sender, EventArgs e)
        {
            _screenName = txtScreenName.Text;

            btnAccept.Enabled = !String.IsNullOrEmpty(txtScreenName.Text);
        }

        #endregion
    }
}
