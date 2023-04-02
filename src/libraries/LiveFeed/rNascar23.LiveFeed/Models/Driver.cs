namespace rNascar23.LiveFeeds.Models
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsInChase { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }
}
