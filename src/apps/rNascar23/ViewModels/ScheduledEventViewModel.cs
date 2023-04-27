using rNascar23.Sdk.Common;
using System;

namespace rNascar23.ViewModels
{
    public class ScheduledEventViewModel
    {
        public int RaceId { get; set; }
        public string EventName { get; set; }
        public string TrackName { get; set; }
        public string TrackCityState { get; set; }
        public string EventLaps { get; set; }
        public string EventMiles { get; set; }
        public DateTime EventDateTime { get; set; }
        public SeriesTypes Series { get; set; }
        public TvTypes Tv { get; set; }
        public RadioTypes Radio { get; set; }
        public SatelliteTypes Satellite { get; set; }
    }
}
