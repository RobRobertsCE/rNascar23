using rNascar23.LiveFeeds.Models;

namespace rNascar23.Data.LiveFeeds.Ports
{
    public interface ILiveFeedRepository
    {
        LiveFeed GetLiveFeed();
    }
}
