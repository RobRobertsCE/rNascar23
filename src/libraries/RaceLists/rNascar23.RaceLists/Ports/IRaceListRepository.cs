using rNascar23.RaceLists.Models;
using System.Threading.Tasks;

namespace rNascar23.RaceLists.Ports
{
    public interface IRaceListRepository
    {
        Task<RaceList> GetRaceListAsync();
    }
}
