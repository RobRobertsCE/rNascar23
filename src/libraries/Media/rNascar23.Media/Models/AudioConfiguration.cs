using System.Collections.Generic;

namespace rNascar23.Media.Models
{
    public class AudioConfiguration
    {
        public int historical_race_id { get; set; }
        public string race_name { get; set; }
        public int run_type { get; set; }
        public string track_name { get; set; }
        public int series_id { get; set; }
        public List<AudioChannel> audio_config { get; set; }
    }
}
