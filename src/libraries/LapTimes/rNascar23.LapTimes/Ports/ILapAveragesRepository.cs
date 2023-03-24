using rNascar23.LapTimes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.LapTimes.Ports
{
    public interface ILapAveragesRepository
    {
        Task<List<LapAverages>> GetLapAveragesAsync(int seriesId, int eventId);
    }
}
