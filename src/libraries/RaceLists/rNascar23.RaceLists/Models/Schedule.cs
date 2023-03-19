using System;

namespace rNascar23.RaceLists.Models
{
    public class Schedule
    {
        public string EventName { get; set; }
        public string Notes { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public int RunType { get; set; }
    }
}
