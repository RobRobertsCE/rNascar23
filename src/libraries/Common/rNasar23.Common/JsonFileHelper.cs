using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace rNascar23.Common
{
    public static class JsonFileHelper
    {
        #region consts

        public const string CustomViewsPrefix = "CustomViews";
        public const string DriverInfoPrefix = "DriverInfo";
        public const string StylesPrefix = "Styles";
        public const string ScreensPrefix = "Screens";
        public const string BackupSuffix = "Backup";
        public const string DataFileExtension = "json";
        public const string BackupTitle = "Backup";
        public const string ImportTitle = "Import";
        public const string ExportTitle = "Export";

        #endregion

        #region public

        public static string GetDataFileExtension()
        {
            return $".{DataFileExtension}";
        }

        public static string GetDriverInfoDataFile()
        {
            return $"{DriverInfoPrefix}.{DataFileExtension}";
        }
        public static string GetScreensDataFile()
        {
            return $"{ScreensPrefix}.{DataFileExtension}";
        }
        public static string GetStylesDataFile()
        {
            return $"{StylesPrefix}.{DataFileExtension}";
        }
        public static string GetCustomViewsDataFile()
        {
            return $"{CustomViewsPrefix}.{DataFileExtension}";
        }

        public static string GetScreensBackupFileFormat()
        {
            return FormatWithTimestamp(ScreensPrefix, BackupSuffix);
        }
        public static string GetStylesBackupFileFormat()
        {
            return FormatWithTimestamp(StylesPrefix, BackupSuffix);
        }
        public static string GetCustomViewsBackupFileFormat()
        {
            return FormatWithTimestamp(CustomViewsPrefix, BackupSuffix);
        }

        public static string GetScreensExportFileFormat()
        {
            return FormatWithTimestamp(ScreensPrefix, ExportTitle);
        }
        public static string GetStylesExportFileFormat()
        {
            return FormatWithTimestamp(StylesPrefix, ExportTitle);
        }
        public static string GetCustomViewsExportFileFormat()
        {
            return FormatWithTimestamp(CustomViewsPrefix, ExportTitle);
        }

        public static string GetScreensBackupFileFilter()
        {
            return GetBackupFileFilter(ScreensPrefix);
        }
        public static string GetStylesBackupFileFilter()
        {
            return GetBackupFileFilter(StylesPrefix);
        }
        public static string GetCustomViewsBackupFileFilter()
        {
            return GetBackupFileFilter(CustomViewsPrefix);
        }

        public static string GetScreensExportFileFilter()
        {
            return GetExportFileFilter(ScreensPrefix);
        }
        public static string GetStylesExportFileFilter()
        {
            return GetExportFileFilter(StylesPrefix);
        }
        public static string GetCustomViewsExportFileFilter()
        {
            return GetExportFileFilter(CustomViewsPrefix);
        }

        public static SaveFileDialog CustomViewsBackupFileDialog()
        {
            return CustomViewsSaveBackupFileDialog(BackupTitle);
        }
        public static SaveFileDialog CustomViewsExportFileDialog()
        {
            return CustomViewsSaveExportFileDialog(ExportTitle);
        }
        public static OpenFileDialog CustomViewsImportFileDialog()
        {
            return CustomViewsOpenFileDialog(ImportTitle);
        }

        public static SaveFileDialog ScreensBackupFileDialog()
        {
            return ScreensSaveBackupFileDialog(BackupTitle);
        }
        public static SaveFileDialog ScreensExportFileDialog()
        {
            return ScreensSaveExportFileDialog(ExportTitle);
        }
        public static OpenFileDialog ScreensImportFileDialog()
        {
            return ScreensOpenFileDialog(ImportTitle);
        }

        public static SaveFileDialog StylesBackupFileDialog()
        {
            return StylesSaveBackupFileDialog(BackupTitle);
        }
        public static SaveFileDialog StylesExportFileDialog()
        {
            return StylesSaveExportFileDialog(ExportTitle);
        }
        public static OpenFileDialog StylesImportFileDialog()
        {
            return StylesOpenFileDialog(ImportTitle);
        }

        #endregion

        #region private

        private static SaveFileDialog CustomViewsSaveBackupFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(CustomViewsPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new SaveFileDialog()
            {
                FileName = JsonFileHelper.GetCustomViewsBackupFileFormat(),
                Filter = JsonFileHelper.GetCustomViewsBackupFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }
        private static SaveFileDialog CustomViewsSaveExportFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(CustomViewsPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new SaveFileDialog()
            {
                FileName = JsonFileHelper.GetCustomViewsExportFileFormat(),
                Filter = JsonFileHelper.GetCustomViewsExportFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }
        private static OpenFileDialog CustomViewsOpenFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(CustomViewsPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new OpenFileDialog()
            {
                FileName = JsonFileHelper.GetCustomViewsBackupFileFormat(),
                Filter = JsonFileHelper.GetCustomViewsBackupFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }

        private static SaveFileDialog ScreensSaveBackupFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(ScreensPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new SaveFileDialog()
            {
                FileName = JsonFileHelper.GetScreensBackupFileFormat(),
                Filter = JsonFileHelper.GetScreensBackupFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }
        private static SaveFileDialog ScreensSaveExportFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(ScreensPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new SaveFileDialog()
            {
                FileName = JsonFileHelper.GetScreensExportFileFormat(),
                Filter = JsonFileHelper.GetScreensExportFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }
        private static OpenFileDialog ScreensOpenFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(ScreensPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new OpenFileDialog()
            {
                FileName = JsonFileHelper.GetScreensBackupFileFormat(),
                Filter = JsonFileHelper.GetScreensBackupFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }

        private static SaveFileDialog StylesSaveBackupFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(StylesPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new SaveFileDialog()
            {
                FileName = JsonFileHelper.GetStylesBackupFileFormat(),
                Filter = JsonFileHelper.GetStylesBackupFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }
        private static SaveFileDialog StylesSaveExportFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(StylesPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new SaveFileDialog()
            {
                FileName = JsonFileHelper.GetStylesExportFileFormat(),
                Filter = JsonFileHelper.GetStylesExportFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }
        private static OpenFileDialog StylesOpenFileDialog(string actionType)
        {
            var userSettings = UserSettingsService.LoadUserSettings();

            var fileDescription = Regex.Replace(StylesPrefix, "(\\B[A-Z])", " $1");

            var title = $"{actionType} {fileDescription}";
            return new OpenFileDialog()
            {
                FileName = JsonFileHelper.GetStylesBackupFileFormat(),
                Filter = JsonFileHelper.GetStylesBackupFileFilter(),
                FilterIndex = 1,
                DefaultExt = JsonFileHelper.GetDataFileExtension(),
                InitialDirectory = userSettings.BackupDirectory,
                Title = title
            };
        }

        private static string GetBackupFileFormat(string fileType)
        {
            return FormatWithTimestamp(fileType, BackupSuffix);
        }

        private static string FormatWithTimestamp(string filePrefix = "", string fileSuffix = "")
        {
            var prefix = String.IsNullOrEmpty(filePrefix) ? "" : $"{filePrefix} ";
            var suffix = String.IsNullOrEmpty(fileSuffix) ? "" : $" {fileSuffix}";

            return $"{prefix}{DateTime.Now:yyyy-MM-dd H-mm-tt}{suffix}.{DataFileExtension}";
        }

        private static string GetBackupFileFilter(string fileType)
        {
            var fileDescription = Regex.Replace(fileType, "(\\B[A-Z])", " $1");

            return $"{fileDescription} backup|{fileType}*{BackupSuffix}.{DataFileExtension}|{fileDescription} export|{fileType}*{ExportTitle}.{DataFileExtension}|All Files|*.*";
        }
        private static string GetExportFileFilter(string fileType)
        {
            var fileDescription = Regex.Replace(fileType, "(\\B[A-Z])", " $1");

            return $"{fileDescription} file|{fileType}*{ExportTitle}.{DataFileExtension}|All Files (*.*)|*.*";
        }

        #endregion
    }
}
