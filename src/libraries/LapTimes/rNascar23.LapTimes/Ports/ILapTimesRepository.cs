using rNascar23.LapTimes.Models;
using System.Collections.Generic;

namespace rNascar23.LapTimes.Ports
{
    public interface ILapTimesRepository
    {
        LapTimeData GetLapTimeData(int seriesId, int eventId);
        LapTimeData GetLapTimeData(int seriesId, int eventId, int driverId);
    }
}
