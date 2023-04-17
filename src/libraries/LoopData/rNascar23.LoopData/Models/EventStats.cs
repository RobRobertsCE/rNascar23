namespace rNascar23.LoopData.Models
{
    public class EventStats
    {
        public int race_id { get; set; }
        public string race_name { get; set; }
        public int series_id { get; set; }
        public int sch_laps { get; set; }
        public int act_laps { get; set; }
        public Driver[] drivers { get; set; }
    }
}
