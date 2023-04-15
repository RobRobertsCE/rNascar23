using System;

namespace rNascar23.Service.RaceLists.Data.Models
{
    public class ScheduleModel
    {
        public string event_name { get; set; }
        public string notes { get; set; }
        public DateTime start_time_utc { get; set; }
        public int run_type { get; set; }
    }
}
