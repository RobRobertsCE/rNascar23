using rNascar23.DriverStatistics.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.DriverStatistics.Ports
{
    public interface IDriverInfoRepository
    {
        Task<DriverInfo> GetDriverAsync(int id);
        Task<IList<DriverInfo>> GetDriversAsync(bool updateFromCompletedRaces = false);
        Task<IList<DriverInfo>> GetDriversAsync(int seriesId, int raceId, int year);
    }
}
