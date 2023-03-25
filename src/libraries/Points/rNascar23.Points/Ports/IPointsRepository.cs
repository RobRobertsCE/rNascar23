using rNascar23.Points.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.Points.Ports
{
    public interface IPointsRepository
    {
        Task<IList<DriverPoints>> GetDriverPoints(int raceId, int seriesId);
        Task<IList<Stage>> GetStagePoints(int raceId, int seriesId);
    }
}
