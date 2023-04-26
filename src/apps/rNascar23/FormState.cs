using rNascar23.Sdk.Flags.Models;
using rNascar23.Sdk.LapTimes.Models;
using rNascar23.Sdk.LiveFeeds.Models;
using rNascar23.Sdk.LoopData.Models;
using rNascar23.Sdk.Points.Models;
using rNascar23.Sdk.Schedules.Models;
using System.Collections.Generic;
using PitStop = rNascar23.Sdk.PitStops.Models.PitStop;

namespace rNascar23
{
    internal class FormState
    {
        public LiveFeed LiveFeed { get; set; }
        public IEnumerable<FlagState> FlagStates { get; set; }
        public IEnumerable<SeriesEvent> SeriesSchedules { get; set; }
        public IEnumerable<LapAverages> LapAverages { get; set; }
        public LapTimeData LapTimes { get; set; }
        public EventLoopData EventStatistics { get; set; }
        public IEnumerable<DriverPoints> LivePoints { get; set; }
        public IEnumerable<StagePointsDetails> StagePoints { get; set; }
        public IEnumerable<PitStop> PitStops { get; set; }
        public IEnumerable<KeyMoment> KeyMoments { get; set; }

        public SeriesEvent CurrentSeriesRace { get; set; }
    }
}
