namespace rNascar23.Service.PitStops.Data.Models
{
    public class PitStopModel
    {
        public string vehicle_number { get; set; }
        public string driver_name { get; set; }
        public string vehicle_manufacturer { get; set; }
        public int leader_lap { get; set; }
        public int lap_count { get; set; }
        public int pit_in_flag_status { get; set; }
        public int pit_out_flag_status { get; set; }
        public float pit_in_race_time { get; set; }
        public float pit_out_race_time { get; set; }
        public float total_duration { get; set; }
        public float box_stop_race_time { get; set; }
        public float box_leave_race_time { get; set; }
        public float pit_stop_duration { get; set; }
        public float in_travel_duration { get; set; }
        public float out_travel_duration { get; set; }
        public string pit_stop_type { get; set; }
        public bool left_front_tire_changed { get; set; }
        public bool left_rear_tire_changed { get; set; }
        public bool right_front_tire_changed { get; set; }
        public bool right_rear_tire_changed { get; set; }
        public int previous_lap_time { get; set; }
        public int next_lap_time { get; set; }
        public int pit_in_rank { get; set; }
        public int pit_out_rank { get; set; }
        public int positions_gained_lost { get; set; }
    }
}
