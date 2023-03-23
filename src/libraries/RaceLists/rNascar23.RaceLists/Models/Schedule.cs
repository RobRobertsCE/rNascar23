using System;

namespace rNascar23.RaceLists.Models
{
    public class Schedule
    {
        public string EventName { get; set; }
        public string Notes { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime StartTimeLocal
        {
            get
            {
                return StartTimeUtc.ToLocalTime();
            }
        }
        public int RunType { get; set; }
        public string Description
        {
            get
            {
                return RunType == 0 ? String.Empty :
                    RunType == 1 ? "Practice" :
                    RunType == 2 ? "Qualifying" :
                    RunType == 3 ? "Race" :
                    "Unknown";
            }
        }
    }
}
