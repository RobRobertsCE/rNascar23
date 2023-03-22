using rNascar23.LiveFeeds.Models;
using System.Threading.Tasks;

namespace rNascar23.Data.LiveFeeds.Ports
{
    public interface ILiveFeedRepository
    {
        Task<LiveFeed> GetLiveFeedAsync();
    }
}
