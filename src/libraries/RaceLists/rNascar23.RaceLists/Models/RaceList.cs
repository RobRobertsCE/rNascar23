using System.Collections.Generic;
using System.Text;

namespace rNascar23.RaceLists.Models
{
    public class RaceList
    {
        public IList<Series> CupSeries { get; set; } = new List<Series>();
        public IList<Series> XfinitySeries { get; set; } = new List<Series>();
        public IList<Series> TruckSeries { get; set; } = new List<Series>();
    }
}
