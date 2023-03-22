using rNascar23.DriverStatistics.Models;
using rNascar23.Flags.Models;
using rNascar23.LapTimes.Models;
using rNascar23.LiveFeeds.Models;
using rNascar23.RaceLists.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rNascar23TestApp.ViewModels
{
    internal class FormState
    {
        public LiveFeed LiveFeed { get; set; }
        public IList<FlagState> FlagStates { get; set; }
        public IList<Series> SeriesSchedules { get; set; }
        public LapTimeData LapTimes { get; set; }
        public EventStats EventStatistics { get; set; }

        public Series CurrentSeriesRace { get; set; }
    }
}
