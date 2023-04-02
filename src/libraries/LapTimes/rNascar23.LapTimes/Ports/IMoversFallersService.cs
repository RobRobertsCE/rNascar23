using rNascar23.LapTimes.Models;
using System.Collections.Generic;

namespace rNascar23.LapTimes.Ports
{
    public interface IMoversFallersService
    {
        IList<PositionChange> GetDriverPositionChanges(LapTimeData lapTimeData);
    }
}
