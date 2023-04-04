using rNascar23.PitStops.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.PitStops.Ports
{
    public interface IPitStopsRepository
    {
        Task<IList<PitStop>> GetPitStopsAsync(int seriesId, int raceId);
        Task<IList<PitStop>> GetPitStopsAsync(int seriesId, int raceId, int? startLap, int? endLap = null, int? carNumber = null);
    }
}
