using System.Collections.Generic;

namespace rNascar23.Service.LiveFeeds.Data.Models
{
    internal class KeyMomentModel
    {
        public int FlagState { get; set; }
        public string Note { get; set; }
        public int NoteID { get; set; }
        //public int[] DriverIDs { get; set; }
    }

    internal class KeyMomentModelList
    {
        public IDictionary<int, KeyMomentModel[]> Laps { get; set; }
    }
}
