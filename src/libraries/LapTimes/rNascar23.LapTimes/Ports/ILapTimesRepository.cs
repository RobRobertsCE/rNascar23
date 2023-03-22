using rNascar23.LapTimes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.LapTimes.Ports
{
    public interface ILapTimesRepository
    {
        Task<LapTimeData> GetLapTimeDataAsync(int seriesId, int eventId);
        Task<LapTimeData> GetLapTimeDataAsync(int seriesId, int eventId, int driverId);
    }
}
