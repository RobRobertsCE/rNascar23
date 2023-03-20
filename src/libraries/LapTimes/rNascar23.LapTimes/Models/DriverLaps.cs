using System;
using System.Collections.Generic;
using System.Linq;

namespace rNascar23.LapTimes.Models
{
    public class DriverLaps
    {
        public string Number { get; set; }
        public string FullName { get; set; }
        public string Manufacturer { get; set; }
        public int RunningPos { get; set; }
        public int NASCARDriverID { get; set; }
        public IList<LapDetails> Laps { get; set; } = new List<LapDetails>();

        public float? AverageTimeLast15Laps()
        {
            return AverageTimeLastNLaps(15);
        }
        public float? AverageSpeedLast15Laps()
        {
            return AverageSpeedLastNLaps(15);
        }

        public float? AverageTimeLast10Laps()
        {
            return AverageTimeLastNLaps(10);
        }
        public float? AverageSpeedLast10Laps()
        {
            return AverageSpeedLastNLaps(10);
        }

        public float? AverageTimeLast5Laps()
        {
            return AverageTimeLastNLaps(5);
        }
        public float? AverageSpeedLast5Laps()
        {
            return AverageSpeedLastNLaps(5);
        }

        private float? AverageTimeLastNLaps(int lapCount)
        {
            if (Laps == null || Laps.Count < lapCount)
                return null;
            else
            {
                var lastNLaps = Laps.OrderByDescending(l => l.Lap).Take(lapCount);

                if (lastNLaps.Any(l => !l.LapTime.HasValue))
                    return null;

                return lastNLaps.Average(l => l.LapTime.Value);
            }
        }

        private float? AverageSpeedLastNLaps(int lapCount)
        {
            if (Laps == null || Laps.Count < lapCount)
                return null;
            else
            {
                var lastNLaps = Laps.OrderByDescending(l => l.Lap).Take(lapCount);

                if (lastNLaps.Any(l => !l.LapTime.HasValue))
                    return null;

                return (float?)Math.Round(lastNLaps.Average(l => float.Parse(l.LapSpeed)), 3);
            }
        }
    }
}
