using rNascar23.Schedules.Models;
using System.Threading.Tasks;

namespace rNascar23.Schedules.Ports
{
    public interface ISchedulesRepository
    {
        Task<SeriesSchedules> GetRaceListAsync(int? year = null);
    }
}
