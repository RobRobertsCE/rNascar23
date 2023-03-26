using Newtonsoft.Json;
using System;
using System.IO;

namespace rNascar23TestApp.Settings
{
    internal class UserSettingsService
    {
        #region consts

        private const string DefaultRootDirectory = "rNascar23\\";
        private const string DefaultBackupDirectory = "Backups\\";
        private const string DefaultDataDirectory = "Data\\";
        private const string DefaultLogDirectory = "Logs\\";
        private const string UserSettingsFileName = "UserSettings.json";

        #endregion

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

            return new UserSettings()
            {
                LogDirectory = Path.Combine(rootDirectory, DefaultLogDirectory),
                DataDirectory = Path.Combine(rootDirectory, DefaultDataDirectory),
                BackupDirectory = Path.Combine(rootDirectory, DefaultBackupDirectory)
            };
        }
    }
}
