namespace rNascar23.Media.Models
{
    public class AudioChannel
    {
        public int stream_number { get; set; }
        public string driver_number { get; set; }
        public string driver_name { get; set; }
        public string base_url { get; set; }
        public string stream_rtmp { get; set; }
        public string stream_ios { get; set; }
        public bool requiresAuth { get; set; }
        public string source
        {
            get
            {
                return $"{base_url}{stream_ios}";
            }
        }
    }
}
