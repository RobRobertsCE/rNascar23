using System.Collections.Generic;
using System.Text;

namespace rNascar23.Schedules.Models
{
    public class SeriesSchedules
    {
        public IList<SeriesEvent> CupSeries { get; set; } = new List<SeriesEvent>();
        public IList<SeriesEvent> XfinitySeries { get; set; } = new List<SeriesEvent>();
        public IList<SeriesEvent> TruckSeries { get; set; } = new List<SeriesEvent>();
    }
}
