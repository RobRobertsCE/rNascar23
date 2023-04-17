using System.Collections.Generic;

namespace rNascar23.Common
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
        public IList<string> FavoriteDrivers { get; set; } = new List<string>();
        public bool UseGraphicalCarNumbers { get; set; } = false;
        public bool UseDarkTheme { get; set; } = false;
        public bool AutoUpdateEnabledOnStart { get; set; } = true;
        public bool UseLowScreenResolutionSizes { get; set; } = true;
        public string OverrideFontName { get; set; }
        public float? OverrideFontSize { get; set; }
        public int? OverrideFontStyle { get; set; }
        public IList<int> RaceViewBottomGrids { get; set; } = new List<int>();
        public IList<int> RaceViewRightGrids { get; set; } = new List<int>();
        public IList<int> QualifyingViewBottomGrids { get; set; } = new List<int>();
        public IList<int> QualifyingViewRightGrids { get; set; } = new List<int>();
        public IList<int> PracticeViewBottomGrids { get; set; } = new List<int>();
        public IList<int> PracticeViewRightGrids { get; set; } = new List<int>();
        public int? DataDelayInSeconds { get; set; } = null;
    }
}
