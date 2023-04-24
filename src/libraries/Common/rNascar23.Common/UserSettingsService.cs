using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace rNascar23.Common
{
    public class UserSettingsService
    {
        #region consts

        private const string DefaultRootDirectory = "rNascar23\\";
        private const string DefaultBackupDirectory = "Backups\\";
        private const string DefaultDataDirectory = "Data\\";
        private const string DefaultLogDirectory = "Logs\\";
        private const string UserSettingsFileName = "UserSettings.json";

        #endregion

        #region public

        public static UserSettings LoadUserSettings()
        {
            var rootDirectory = GetDefaultRootDirectory();

            var userSettingsFilePath = Path.Combine(rootDirectory, UserSettingsFileName);

            if (!File.Exists(userSettingsFilePath))
                return GetDefaultUSerSettings();
            else
            {
                var json = File.ReadAllText(userSettingsFilePath);

                if (String.IsNullOrEmpty(json))
                    return GetDefaultUSerSettings();
                else
                {
                    var userSettings = JsonConvert.DeserializeObject<UserSettings>(json);

                    EnsureDirectoryExists(userSettings.BackupDirectory);
                    EnsureDirectoryExists(userSettings.DataDirectory);
                    EnsureDirectoryExists(userSettings.LogDirectory);

                    if (userSettings == null)
                        return GetDefaultUSerSettings();
                    else
                        return userSettings;
                }
            }
        }

        public static void SaveUserSettings(UserSettings userSettings)
        {
            if (userSettings == null)
                throw new ArgumentNullException(nameof(userSettings));

            EnsureDirectoryExists(userSettings.BackupDirectory);
            EnsureDirectoryExists(userSettings.DataDirectory);
            EnsureDirectoryExists(userSettings.LogDirectory);

            var json = JsonConvert.SerializeObject(userSettings, Formatting.Indented);

            var rootDirectory = GetDefaultRootDirectory();

            var userSettingsFilePath = Path.Combine(rootDirectory, UserSettingsFileName);

            File.WriteAllText(userSettingsFilePath, json);
        }

        public static string GetDefaultRootDirectory()
        {
            var myDocumentsDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\";

            var defaultRootDirectoryPath = Path.Combine(myDocumentsDirectory, DefaultRootDirectory);

            if (!Directory.Exists(defaultRootDirectoryPath))
            {
                Directory.CreateDirectory(defaultRootDirectoryPath);
            }

            return defaultRootDirectoryPath;
        }

        private static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static UserSettings GetDefaultUSerSettings()
        {
            var rootDirectory = GetDefaultRootDirectory();

            var userSettings = new UserSettings()
            {
                LogDirectory = Path.Combine(rootDirectory, DefaultLogDirectory),
                DataDirectory = Path.Combine(rootDirectory, DefaultDataDirectory),
                BackupDirectory = Path.Combine(rootDirectory, DefaultBackupDirectory)
            };

            EnsureDirectoryExists(userSettings.BackupDirectory);
            EnsureDirectoryExists(userSettings.DataDirectory);
            EnsureDirectoryExists(userSettings.LogDirectory);

            userSettings.RaceViewBottomGrids = new List<int>()
            {
                12,
                7,
                8,
                9,
                4,
                5,
                6
            };
            userSettings.RaceViewRightGrids = new List<int>()
            {
                1,
                10,
                11,
                2
            };
            userSettings.QualifyingViewBottomGrids = new List<int>()
            {
                12,
                1
            };
            userSettings.QualifyingViewRightGrids = new List<int>();
            userSettings.PracticeViewBottomGrids = new List<int>()
            {
                12,
                7,
                8,
                9,
                4,
                5,
                6
            };
            userSettings.PracticeViewRightGrids = new List<int>()
            {
                1
            };

            SaveUserSettings(userSettings);

            return userSettings;
        }

        #endregion
    }
}
