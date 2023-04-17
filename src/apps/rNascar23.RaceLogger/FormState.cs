using rNascar23.Flags.Models;
using rNascar23.LapTimes.Models;
using rNascar23.LiveFeeds.Models;
using rNascar23.LoopData.Models;
using rNascar23.Points.Models;
using rNascar23.Schedules.Models;
using System.Collections.Generic;
using PitStop = rNascar23.PitStops.Models.PitStop;

namespace rNascar23.RaceLogger
{
    internal class FormState
    {
        public LiveFeed LiveFeed { get; set; }
        public IList<FlagState> FlagStates { get; set; }
        public IList<SeriesEvent> SeriesSchedules { get; set; }
        public IList<LapAverages> LapAverages { get; set; }
        public LapTimeData LapTimes { get; set; }
        public EventStats EventStatistics { get; set; }
        public IList<DriverPoints> LivePoints { get; set; }
        public IList<StagePoints2> StagePoints { get; set; }
        public IList<PitStop> PitStops { get; set; }
    }
}