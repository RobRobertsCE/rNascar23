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

        public static string GetDriverInfoDataFile()
        {
            return $"{DriverInfoPrefix}.{DataFileExtension}";
        }

        #endregion
    }
}
