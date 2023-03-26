using Microsoft.Extensions.Logging;
using rNascar23TestApp.Settings;
using System;
using System.IO;
using System.Windows.Forms;

namespace rNascar23TestApp.Dialogs
{
    public partial class UserSettingsDialog : Form
    {
        #region private fields

        private string _previousRootDirectory = String.Empty;
        private readonly ILogger<UserSettingsDialog> _logger = null;

        #endregion

        #region private properties

        private UserSettings _userSettings = null;
        public UserSettings UserSettings
        {
            get
            {
                return _userSettings;
            }
            set
            {
                _userSettings = value;
            }
        }

        #endregion

        #region ctor/load

        public UserSettingsDialog(ILogger<UserSettingsDialog> logger)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private void UserSettingsDialog_Load(object sender, EventArgs e)
        {
            try
            {
                if (_userSettings == null)
                {
                    _userSettings = UserSettingsService.LoadUserSettings();
                }

                DisplayUserSettings(_userSettings);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion

        #region private

        private void DisplayUserSettings(UserSettings settings)
        {
            txtDataDirectory.Text = settings.DataDirectory;
            txtBackupDirectory.Text = settings.BackupDirectory;
            txtLogDirectory.Text = settings.LogDirectory;
            rbFastestLapsTime.Checked = settings.FastestLapsDisplayType == SpeedTimeType.Seconds;
            rbLastNLapsTime.Checked = settings.LastNLapsDisplayType == SpeedTimeType.Seconds;
            rbBestNLapsTime.Checked = settings.BestNLapsDisplayType == SpeedTimeType.Seconds;
            rbLeaderboardLastLapTime.Checked = settings.LeaderboardLastLapDisplayType == SpeedTimeType.Seconds;
            rbLeaderboardBestLapTime.Checked = settings.LeaderboardBestLapDisplayType == SpeedTimeType.Seconds;
        }

        private void btnDataDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateDirectory(txtDataDirectory, "Data");
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void btnBackupDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateDirectory(txtBackupDirectory, "Backup");
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void btnLogDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateDirectory(txtLogDirectory, "Log");
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void UpdateDirectory(TextBox control, string name)
        {
            var selectedDirectory = SelectDirectory(name);

            if (!String.IsNullOrEmpty(selectedDirectory))
            {
                control.Text = selectedDirectory;
            }
        }

        private string SelectDirectory(string name)
        {
            var rootDirectory = String.IsNullOrEmpty(_previousRootDirectory) ?
                UserSettingsService.GetDefaultRootDirectory() :
                _previousRootDirectory;

            var dialog = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
                Description = $"Select a folder for your {name} files",
                SelectedPath = rootDirectory
            };

            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                var parentDirectoryInfo = Directory.GetParent(dialog.SelectedPath);

                if (parentDirectoryInfo != null)
                    _previousRootDirectory = parentDirectoryInfo.FullName;

                return dialog.SelectedPath;
            }
            else
                return String.Empty;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateUserSettings();

                UserSettingsService.SaveUserSettings(_userSettings);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void UpdateUserSettings()
        {
            _userSettings.BackupDirectory = txtBackupDirectory.Text;
            _userSettings.DataDirectory = txtDataDirectory.Text;
            _userSettings.LogDirectory = txtLogDirectory.Text;

            _userSettings.FastestLapsDisplayType = rbFastestLapsTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.LastNLapsDisplayType = rbLastNLapsTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.BestNLapsDisplayType = rbBestNLapsTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.LeaderboardLastLapDisplayType = rbLeaderboardLastLapTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.LeaderboardBestLapDisplayType = rbLeaderboardBestLapTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
        }

        #endregion

        #region private [ exception handlers ]

        private void ExceptionHandler(Exception ex)
        {
            ExceptionHandler(ex, String.Empty, true);
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
