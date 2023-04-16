using rNascar23.LiveFeeds.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.LiveFeeds.Ports
{
    public interface IKeyMomentsRepository
    {
        Task<IList<KeyMoment>> GetKeyMomentsAsync(int seriesId, int raceId, int? year = null);
    }
}
