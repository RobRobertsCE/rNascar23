using rNascar23.Patches.AppRegistry;
using rNascar23.Patches.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Patcher
{
    public partial class Patcher : Form
    {
        private PatcherService _service = new PatcherService();

        public Version PatchVersion { get; set; }

        public Patcher()
        {
            InitializeComponent();
        }

        public Patcher(Version version)
            : this()
        {
            PatchVersion = version;
        }

        private async void Patcher_Load(object sender, EventArgs e)
        {
            try
            {
                if (PatchVersion == null)
                {
                    throw new ArgumentNullException(nameof(PatchVersion));
                }

                lblCurrentVersion.Text = $"Current version: {RegistryHelper.GetVersion()}";

                lblNewVersion.Text = $"Patch version: {PatchVersion}";

                SetMessage("Starting patch process...");

                _service.InfoMessage += _service_InfoMessage;

                var result = await ApplyPatchAsync(PatchVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnClose.Visible = true;
                btnClose.Enabled = true;
            }
        }

        private void _service_InfoMessage(object sender, string e)
        {
            SetMessage(e);
        }

        private async Task<bool> ApplyPatchAsync(Version patchVersion)
        {
            try
            {
                if (patchVersion == null)
                {
                    SetMessage("No patches available");
                    return false;
                }
                else
                    SetMessage($"Applying patch {patchVersion}");

                var patchResult = await _service.ApplyPatchAsync(patchVersion);

                if (patchResult)
                {
                    SetMessage($"Patch {patchVersion} applied successfully!");
                    return true;
                }
                else
                {
                    SetMessage("An error occurred while applying patch");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }

            return false;
        }
        private void SetMessage(string message)
        {
            lblMessage.Text = message;

            AddMessageToList(message);
        }

        private void AddMessageToList(string message)
        {
            txtMessages.AppendText($"{message}\r\n");
            txtMessages.SelectionStart = txtMessages.TextLength;
            txtMessages.ScrollToCaret();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
