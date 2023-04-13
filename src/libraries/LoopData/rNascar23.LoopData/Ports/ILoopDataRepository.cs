using rNascar23.LoopData.Models;
using System.Threading.Tasks;

namespace rNascar23.LoopData.Ports
{
    public interface ILoopDataRepository
    {
        Task<EventStats> GetEventAsync(int seriesId, int raceId);
    }
}
