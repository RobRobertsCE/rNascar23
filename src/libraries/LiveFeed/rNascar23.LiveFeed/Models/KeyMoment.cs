using System.Collections.Generic;

namespace rNascar23.LiveFeeds.Models
{
    public class KeyMoment
    {
        public int LapNumber { get; set; }
        public int FlagState { get; set; }
        public string Note { get; set; }
        public int NoteID { get; set; }
        public int[] DriverIDs { get; set; }
    }

    public class KeyMomentsList
    {
        public IDictionary<int, KeyMoment> Laps { get; set; }
    }
}
