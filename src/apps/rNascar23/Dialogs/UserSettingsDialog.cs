using Microsoft.Extensions.Logging;
using rNascar23.Common;
using rNascar23.LoopData.Ports;
using rNascar23.Properties;
using rNascar23.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.Dialogs
{
    public partial class UserSettingsDialog : Form
    {
        #region private fields

        private string _previousRootDirectory = String.Empty;
        private readonly ILogger<UserSettingsDialog> _logger = null;
        private readonly IDriverInfoRepository _driverRepository = null;
        private Font _overrideFont = null;

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

                LoadAvailableViews();

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
            chkUseGraphicalNumbers.Checked = settings.UseGraphicalCarNumbers;
            chkUseDarkTheme.Checked = settings.UseDarkTheme;
            chkAutoUpdateEnabledOnStartup.Checked = settings.AutoUpdateEnabledOnStart;
            chkDelayDataUpdates.Checked = settings.DataDelayInSeconds.HasValue;
            if (settings.DataDelayInSeconds.HasValue)
            {
                txtDataDelay.Text = settings.DataDelayInSeconds.Value.ToString();
            }

            if (_userSettings.UseLowScreenResolutionSizes)
            {
                chkLowRes.Checked = true;

                _overrideFont = new Font(
                    _userSettings.OverrideFontName,
                    _userSettings.OverrideFontSize.GetValueOrDefault(10),
                    (FontStyle)_userSettings.OverrideFontStyle);
            }

            DisplayOverrideFont(_overrideFont);

            DisplayFavoriteDrivers(settings.FavoriteDrivers);

            DisplaySelectedViews(settings);
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

        private void DisplaySelectedViews(UserSettings settings)
        {
            SelectViews(settings.RaceViewBottomGrids, chkRaceViewsBottom);
            SelectViews(settings.RaceViewRightGrids, chkRaceViewsRight);
            SelectViews(settings.QualifyingViewBottomGrids, chkQualifyingViewsBottom);
            SelectViews(settings.QualifyingViewRightGrids, chkQualifyingViewsRight);
            SelectViews(settings.PracticeViewBottomGrids, chkPracticeViewsBottom);
            SelectViews(settings.PracticeViewRightGrids, chkPracticeViewsRight);
        }

        public void SelectViews(IList<int> selectedViews, CheckedListBox list)
        {
            for (int i = 0; i < list.Items.Count; i++)
            {
                list.SetItemChecked(i, false);
            }

            foreach (int selectedViewType in selectedViews)
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    var viewDetail = list.Items[i] as ViewDetails;

                    if (viewDetail.ViewType == (GridViewTypes)selectedViewType)
                    {
                        list.SetItemChecked(i, true);
                    }
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

        private void LoadAvailableViews()
        {
            var viewDetails = BuildViewDetailsList();

            foreach (var viewDetail in viewDetails)
            {
                chkRaceViewsBottom.Items.Add(viewDetail);
                chkQualifyingViewsBottom.Items.Add(viewDetail);
                chkPracticeViewsBottom.Items.Add(viewDetail);

                if (viewDetail.ViewType != GridViewTypes.Flags)
                {
                    chkRaceViewsRight.Items.Add(viewDetail);
                    chkQualifyingViewsRight.Items.Add(viewDetail);
                    chkPracticeViewsRight.Items.Add(viewDetail);
                }
            }
        }

        private List<ViewDetails> BuildViewDetailsList()
        {
            var views = new List<ViewDetails>();

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.DriverPoints,
                Name = "Driver Points",
                Description = "Displays the driver points standings for the active series"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.StagePoints,
                Name = "Stage Points",
                Description = "Displays the stage points earned by drivers in the active series"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Flags,
                Name = "Flags",
                Description = "Displays the Green and Yellow flags for the race or session, along with the reason for the caution and lucky dog"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.FastestLaps,
                Name = "Fastest Laps",
                Description = "Displays overall fastest laps run during a session or race"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Movers,
                Name = "Movers",
                Description = "Displays the drivers with the best position change since the last flag"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Fallers,
                Name = "Fallers",
                Description = "Displays the drivers with the worst position change since the last flag"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.LapLeaders,
                Name = "Lap Leaders",
                Description = "Displays the drivers who have led laps in the active race"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Last5Laps,
                Name = "Last 5 Laps",
                Description = "Displays the drivers who have the fastest average laps over the last 5 laps"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Last10Laps,
                Name = "Last 10 Laps",
                Description = "Displays the drivers who have the fastest average laps over the last 10 laps"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Last15Laps,
                Name = "Last 15 Laps",
                Description = "Displays the drivers who have the fastest average laps over the last 15 laps"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Best5Laps,
                Name = "Best 5 Laps",
                Description = "Displays the drivers who have the fastest 5 lap average for the session or race"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Best10Laps,
                Name = "Best 10 Laps",
                Description = "Displays the drivers who have the fastest 10 lap average for the session or race"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.Best15Laps,
                Name = "Best 15 Laps",
                Description = "Displays the drivers who have the fastest 15 lap average for the session or race"
            });

            views.Add(new ViewDetails()
            {
                ViewType = GridViewTypes.KeyMoments,
                Name = "Key Moments",
                Description = "Displays notes about key moments that happen during the session or race"
            });

            return views.ToList();
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
                if (!UpdateUserSettings())
                    return;

                UserSettingsService.SaveUserSettings(_userSettings);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private bool UpdateUserSettings()
        {
            if (chkDelayDataUpdates.Checked)
            {
                if (String.IsNullOrEmpty(txtDataDelay.Text))
                    _userSettings.DataDelayInSeconds = null;
                else if (int.TryParse(txtDataDelay.Text, out int dataDelay))
                {
                    if (dataDelay < 0)
                    {
                        MessageBox.Show("Data delay value must be greater than zero");
                        return false;
                    }
                    else if (dataDelay == 0)
                        _userSettings.DataDelayInSeconds = null;
                    else
                        _userSettings.DataDelayInSeconds = dataDelay;
                }
                else
                {
                    MessageBox.Show("Data delay value must be numeric");
                    return false;
                }
            }
            else
                _userSettings.DataDelayInSeconds = null;

            if (chkLowRes.Checked)
            {
                _userSettings.UseLowScreenResolutionSizes = true;
                _userSettings.OverrideFontName = _overrideFont.Name;
                _userSettings.OverrideFontSize = _overrideFont.Size;
                _userSettings.OverrideFontStyle = (int)_overrideFont.Style;
            }
            else
            {
                _userSettings.UseLowScreenResolutionSizes = false;
                _userSettings.OverrideFontName = String.Empty;
                _userSettings.OverrideFontSize = null;
                _userSettings.OverrideFontStyle = null;
            }

            _userSettings.BackupDirectory = txtBackupDirectory.Text;
            _userSettings.DataDirectory = txtDataDirectory.Text;
            _userSettings.LogDirectory = txtLogDirectory.Text;

            _userSettings.FastestLapsDisplayType = rbFastestLapsTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.LastNLapsDisplayType = rbLastNLapsTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.BestNLapsDisplayType = rbBestNLapsTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.LeaderboardLastLapDisplayType = rbLeaderboardLastLapTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.LeaderboardBestLapDisplayType = rbLeaderboardBestLapTime.Checked ? SpeedTimeType.Seconds : SpeedTimeType.MPH;
            _userSettings.UseGraphicalCarNumbers = chkUseGraphicalNumbers.Checked;
            _userSettings.UseDarkTheme = chkUseDarkTheme.Checked;
            _userSettings.AutoUpdateEnabledOnStart = chkAutoUpdateEnabledOnStartup.Checked;

            _userSettings.FavoriteDrivers = new List<string>();

            for (int i = 0; i < lstFavoriteDrivers.CheckedItems.Count; i++)
            {
                var listItem = lstFavoriteDrivers.CheckedItems[i];

                _userSettings.FavoriteDrivers.Add(listItem.ToString());
            }

            UpdateUserSelectedViews();

            return true;
        }

        private void UpdateUserSelectedViews()
        {
            _userSettings.RaceViewBottomGrids = ReadSelectedViews(chkRaceViewsBottom);
            _userSettings.RaceViewRightGrids = ReadSelectedViews(chkRaceViewsRight);
            _userSettings.QualifyingViewBottomGrids = ReadSelectedViews(chkQualifyingViewsBottom);
            _userSettings.QualifyingViewRightGrids = ReadSelectedViews(chkQualifyingViewsRight);
            _userSettings.PracticeViewBottomGrids = ReadSelectedViews(chkPracticeViewsBottom);
            _userSettings.PracticeViewRightGrids = ReadSelectedViews(chkPracticeViewsRight);
        }

        public IList<int> ReadSelectedViews(CheckedListBox list)
        {
            IList<int> selectedViews = new List<int>();

            for (int i = 0; i < list.CheckedItems.Count; i++)
            {
                var viewDetail = list.CheckedItems[i] as ViewDetails;

                selectedViews.Add((int)viewDetail.ViewType);
            }

            return selectedViews;
        }

        private void chkViewDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ckhListBox = sender as CheckedListBox;

            if (ckhListBox == null || ckhListBox.SelectedItem == null)
                lblHelp.Text = "";
            else
            {
                var viewDetail = ckhListBox.SelectedItem as ViewDetails;

                lblHelp.Text = viewDetail.Description;
            }
        }

        private void chkDelayDataUpdates_CheckStateChanged(object sender, EventArgs e)
        {
            txtDataDelay.Enabled = chkDelayDataUpdates.Checked;
            lblDataDelay.Enabled = chkDelayDataUpdates.Checked;
        }

        private void chkLowRes_CheckStateChanged(object sender, EventArgs e)
        {
            btnSetFont.Enabled = chkLowRes.Checked;

            if (!chkLowRes.Checked)
                DisplayOverrideFont(null);
        }

        private void btnSetFont_Click(object sender, EventArgs e)
        {
            try
            {
                SelectOverrideFont();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void SelectOverrideFont()
        {
            var dialog = new FontDialog();

            if (_overrideFont != null)
                dialog.Font = _overrideFont;

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _overrideFont = dialog.Font;
            }

            DisplayOverrideFont(_overrideFont);
        }

        private void DisplayOverrideFont(Font overrideFont)
        {
            if (overrideFont == null)
            {
                lblOverrideFont.Text = String.Empty;
                lblOverrideFont.Font = lblOverrideFont.Parent.Font;
            }
            else
            {
                lblOverrideFont.Text = $"{overrideFont.Name}; Size = {overrideFont.Size}; Style = {overrideFont.Style}";
                lblOverrideFont.Font = overrideFont;
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

        #region classes

        private class ViewDetails
        {
            public GridViewTypes ViewType { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        #endregion
    }
}
