using Microsoft.Extensions.Logging;
using rNascar23.Common;
using rNascar23.DriverStatistics.Ports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23TestApp.Dialogs
{
    public partial class UserSettingsDialog : Form
    {
        #region private fields

        private string _previousRootDirectory = String.Empty;
        private readonly ILogger<UserSettingsDialog> _logger = null;
        private readonly IDriverInfoRepository _driverRepository = null;

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

        public UserSettingsDialog(ILogger<UserSettingsDialog> logger, IDriverInfoRepository driverRepository)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _driverRepository = driverRepository ?? throw new ArgumentNullException(nameof(driverRepository));
        }

        private async void UserSettingsDialog_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadFavoriteDriversAsync();

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

            DisplayFavoriteDrivers(settings.FavoriteDrivers);
        }

        private void DisplayFavoriteDrivers(IList<string> favoriteDrivers)
        {
            for (int i = 0; i < lstFavoriteDrivers.Items.Count; i++)
            {
                var listItem = lstFavoriteDrivers.Items[i];

                if (favoriteDrivers.Contains(listItem.ToString()))
                {
                    lstFavoriteDrivers.SetItemChecked(i, true);
                }
            }
        }

        private async Task LoadFavoriteDriversAsync()
        {
            var driverList = await _driverRepository.GetDriversAsync();

            lstFavoriteDrivers.Items.Clear();

            foreach (var driver in driverList.OrderBy(d => d.Name))
            {
                lstFavoriteDrivers.Items.Add(driver);
            }
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

            _userSettings.FavoriteDrivers = new List<string>();

            for (int i = 0; i < lstFavoriteDrivers.CheckedItems.Count; i++)
            {
                var listItem = lstFavoriteDrivers.CheckedItems[i];

                _userSettings.FavoriteDrivers.Add(listItem.ToString());
            }
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
