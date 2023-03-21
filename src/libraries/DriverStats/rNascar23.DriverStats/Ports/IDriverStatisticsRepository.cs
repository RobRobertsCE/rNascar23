using rNascar23.DriverStatistics.Models;
using System.Threading.Tasks;

namespace rNascar23.DriverStatistics.Ports
{
    public interface IDriverStatisticsRepository
    {
        Task<EventStats> GetEventAsync(int seriesId, int raceId);
    }
}
