namespace rNascar23.Media.Models
{
    public class VideoConfiguration
    {
        public bool live { get; set; }
        public int raceID { get; set; }
        public int defaultDriverID { get; set; }
        public VideoComponent[] data { get; set; }
    }
}
