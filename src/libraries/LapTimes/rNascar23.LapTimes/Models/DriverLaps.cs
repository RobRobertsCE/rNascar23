using System;
using System.Collections.Generic;
using System.Globalization;
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
                var lastNLaps = Laps.OrderByDescending(l => l.Lap).Take(lapCount).ToList();

                if (lastNLaps.Any(l => !l.LapTime.HasValue))
                    return null;

                if (lastNLaps.Any(l => l.LapSpeed == "-1"))
                    return null;

                List<float> lapSet = new List<float>();

                for (int i = 0; i < lastNLaps.Count(); i++)
                {
                    float? lapSpeed = TryParseFloat(lastNLaps[i].LapSpeed);

                    if (lapSpeed.HasValue)
                    {
                        lapSet.Add(lapSpeed.Value);
                    }
                    else
                        break;
                }

                if (lapSet.Count == lapCount)
                {
                    try
                    {
                        return lapSet.Average(l => l);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
        }

        private float? BestTimeOverNLaps(int range)
        {
            if (Laps == null || Laps.Count < range)
                return null;
            else
            {
                float minAverage = 9999;

                for (int x = 0; x < Laps.Count - range; x++)
                {
                    List<float> lapSet = new List<float>();

                    for (int y = 0; y < range; y++)
                    {
                        var lap = Laps[x + y];

                        if (lap.LapTime.HasValue)
                        {
                            lapSet.Add(lap.LapTime.Value);
                        }
                        else
                            break;
                    }

                    if (lapSet.Count == range)
                    {
                        var lapSetAverage = lapSet.Average(l => l);

                        if (lapSetAverage < minAverage)
                        {
                            minAverage = lapSetAverage;
                        }
                    }
                }

                return (float?)Math.Round(minAverage, 3);
            }
        }

        private float? BestSpeedOverNLaps(int range)
        {
            if (Laps == null || Laps.Count < range)
                return null;
            else
            {
                float maxAverage = -1;

                for (int x = 0; x < Laps.Count - range; x++)
                {
                    List<float> lapSet = new List<float>();

                    for (int y = 0; y < range; y++)
                    {
                        var lap = Laps[x + y];

                        float lapSpeed;

                        if (float.TryParse(lap.LapSpeed, out lapSpeed))
                        {
                            lapSet.Add(lapSpeed);
                        }
                        else
                            break;
                    }

                    if (lapSet.Count == range)
                    {
                        var lapSetAverage = lapSet.Average(l => l);

                        if (lapSetAverage > maxAverage)
                        {
                            maxAverage = lapSetAverage;
                        }
                    }
                }

                if (maxAverage == -1)
                    return null;
                else
                    return (float?)Math.Round(maxAverage, 3);
            }
        }

        private float? TryParseFloat(string floatString)
        {
            float? parsedFloat = 0;
            float result;

            if (float.TryParse(floatString, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                parsedFloat = result;
            }
            else if (float.TryParse(floatString, out result))
            {
                parsedFloat = result;
            }
            else
                parsedFloat = null;

            return parsedFloat;
        }
    }
}
