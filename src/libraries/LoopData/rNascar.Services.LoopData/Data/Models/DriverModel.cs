namespace rNascar23.Service.LoopData.Data.Models
{
    public class DriverModel
    {
        public int driver_id { get; set; }
        public int start_ps { get; set; }
        public int mid_ps { get; set; }
        public int ps { get; set; }
        public int closing_ps { get; set; }
        public int closing_laps_diff { get; set; }
        public int best_ps { get; set; }
        public int worst_ps { get; set; }
        public float avg_ps { get; set; }
        public int passes_gf { get; set; }
        public int passing_diff { get; set; }
        public int passed_gf { get; set; }
        public int quality_passes { get; set; }
        public int fast_laps { get; set; }
        public int top15_laps { get; set; }
        public int lead_laps { get; set; }
        public int laps { get; set; }
        public float rating { get; set; }
    }
}
