using rNascar23.DriverStatistics.Models;
using System.Threading.Tasks;

namespace rNascar23.DriverStatistics.Ports
{
    public interface IWeekendFeedRepository
    {
        Task<WeekendFeed> GetWeekendFeedAsync(int seriesId, int raceId);
    }
}
