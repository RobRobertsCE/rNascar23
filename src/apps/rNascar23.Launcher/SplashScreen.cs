using rNascar23.Patches.AppRegistry;
using rNascar23.Patches.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Launcher
{
    public partial class SplashScreen : Form
    {
        private PatcherService _service = new PatcherService();

        public Version PatchVersion { get; set; } = null;

        public SplashScreen()
        {
            InitializeComponent();
        }

        private async void SplashScreen_Load(object sender, EventArgs e)
        {
            lblUpdatePrompt.Text = $"Checking for updates...";

            lblVersion.Text = $"Version {RegistryHelper.GetVersion()}";

            PatchVersion = await CheckForUpdatesAsync();

            if (PatchVersion != null)
            {
                DisplayPatchPrompt(PatchVersion);
            }
            else
            {
                lblUpdatePrompt.Text = $"Your application is up to date!";

                ContinueWithoutUpdating();
            }
        }

        private async Task<Version> CheckForUpdatesAsync()
        {
            try
            {
                return await _service.CheckForAvailablePatchesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }

        private void DisplayPatchPrompt(Version version)
        {
            btnUpdate.Visible = true;
            btnNo.Visible = true;

            lblUpdatePrompt.Text = $"There is an update available for the application.\r\nWould you like to update to version {version} now?";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            PatchVersion = null;
            this.Close();
        }

        private void ContinueWithoutUpdating()
        {
            timCloseForm.Enabled = true;
        }

        private void timCloseForm_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
