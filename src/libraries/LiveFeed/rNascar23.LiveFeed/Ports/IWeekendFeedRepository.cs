using rNascar23.LiveFeeds.Models;
using System.Threading.Tasks;

namespace rNascar23.LiveFeeds.Ports
{
    public interface IWeekendFeedRepository
    {
        Task<WeekendFeed> GetWeekendFeedAsync(int seriesId, int raceId);
    }
}
