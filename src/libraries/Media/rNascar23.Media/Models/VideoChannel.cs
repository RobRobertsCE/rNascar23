namespace rNascar23.Media.Models
{
    public class VideoChannel
    {
        public int id { get; set; }
        public int driverID { get; set; }
        public int? postID { get; set; }
        public string title { get; set; }
        public bool driverOverlay { get; set; }
        public string driverOverlayImg { get; set; }
        public string driverOverlayName { get; set; }
        public string stream1 { get; set; }
        public bool stream1Is360 { get; set; }
        public string stream1AssetKey { get; set; }
        public string stream1AssetKeyMobile { get; set; }
        public object stream1IconText { get; set; }
        public string stream1SponsorImage { get; set; }
        public string stream1SponsorName { get; set; }
        public object stream2 { get; set; }
        public bool stream2Is360 { get; set; }
        public object stream2AssetKey { get; set; }
        public object stream2AssetKeyMobile { get; set; }
        public object stream2IconText { get; set; }
        public object stream2SponsorImage { get; set; }
        public object stream2SponsorName { get; set; }
        public object poster { get; set; }
        public string driverManu { get; set; }
        public string driverVehicle { get; set; }
        public object driverImage { get; set; }
    }

}
