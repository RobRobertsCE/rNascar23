using rNascar23TestApp.Settings;

namespace rNascar23TestApp
{
    public class UserSettings
    {
        public string DataDirectory { get; set; }
        public string BackupDirectory { get; set; }
        public string LogDirectory { get; set; }
        public SpeedTimeType FastestLapsDisplayType { get; set; }
        public SpeedTimeType LastNLapsDisplayType { get; set; }
        public SpeedTimeType BestNLapsDisplayType { get; set; }
        public SpeedTimeType LeaderboardLastLapDisplayType { get; set; }
        public SpeedTimeType LeaderboardBestLapDisplayType { get; set; }
    }
}
