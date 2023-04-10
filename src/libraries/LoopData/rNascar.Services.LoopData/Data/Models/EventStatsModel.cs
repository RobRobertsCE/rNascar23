namespace rNascar23.Service.LoopData.Data.Models
{
    public class EventStatsModel
    {
        public int race_id { get; set; }
        public string race_name { get; set; }
        public int series_id { get; set; }
        public int sch_laps { get; set; }
        public int act_laps { get; set; }
        public DriverModel[] drivers { get; set; }
    }
}
