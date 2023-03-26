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

        public float? Best5LapAverageSpeed()
        {
            return BestSpeedOverNLaps(5);
        }
        public float? Best5LapAverageTime()
        {
            return BestTimeOverNLaps(5);
        }

        public float? Best10LapAverageSpeed()
        {
            return BestSpeedOverNLaps(10);
        }
        public float? Best10LapAverageTime()
        {
            return BestTimeOverNLaps(10);
        }

        public float? Best15LapAverageSpeed()
        {
            return BestSpeedOverNLaps(15);
        }
        public float? Best15LapAverageTime()
        {
            return BestTimeOverNLaps(15);
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

        private float? BestSpeedOverNLaps(int range)
        {
            if (Laps == null || Laps.Count < range)
                return null;
            else
            {
                float maxAverage = -1;

                for (int i = 0; i < Laps.Count - range; i++)
                {
                    var rangeAverage = Laps.Where(l => !string.IsNullOrEmpty(l.LapSpeed) && l.Lap > 0).Skip(i).Take(range).Average(l => float.Parse(l.LapSpeed));

                    if (rangeAverage > maxAverage)
                    {
                        maxAverage = rangeAverage;
                    }
                }

                return (float?)Math.Round(maxAverage, 3);
            }
        }

        private float? BestTimeOverNLaps(int range)
        {
            if (Laps == null || Laps.Count < range)
                return null;
            else
            {
                float minAverage = 9999;

                for (int i = 0; i < Laps.Count - range; i++)
                {
                    var rangeAverage = Laps.Where(l => !string.IsNullOrEmpty(l.LapSpeed)).Skip(i).Take(range).Average(l => float.Parse(l.LapSpeed));

                    if (rangeAverage < minAverage)
                    {
                        minAverage = rangeAverage;
                    }
                }

                return (float?)Math.Round(minAverage, 3);
            }
        }
    }
}
