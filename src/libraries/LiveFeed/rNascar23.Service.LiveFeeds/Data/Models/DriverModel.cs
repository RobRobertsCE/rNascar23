namespace rNascar23.Service.LiveFeeds.Data.Models
{
    public class DriverModel
    {
        public int driver_id { get; set; }
        public string full_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public bool is_in_chase { get; set; }
    }
}
