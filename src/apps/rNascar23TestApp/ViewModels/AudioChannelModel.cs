namespace rNascar23.ViewModels
{
    public class AudioChannelModel
    {
        #region properties

        public int StreamNumber { get; set; }
        public string DriverNumber { get; set; }
        public string Name { get; set; }
        public string Source
        {
            get
            {
                return $"{BaseUrl}{Stream_IOS}";
            }
        }
        public string Stream_RTMP { get; set; }
        public string Stream_IOS { get; set; }
        public string BaseUrl { get; set; }

        #endregion

        #region ctor

        public AudioChannelModel()
        {
            BaseUrl = @"https://driveaudio.akamaized.net/hls/live/";
        }

        #endregion
    }
}
