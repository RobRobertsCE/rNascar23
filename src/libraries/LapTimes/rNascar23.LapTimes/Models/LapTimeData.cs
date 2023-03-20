using System.Collections.Generic;

namespace rNascar23.LapTimes.Models
{
    public class LapTimeData
    {
        public IList<DriverLaps> Drivers { get; set; } = new List<DriverLaps>();
        public IList<LapFlag> LapFlags { get; set; } = new List<LapFlag>();

    }
}
