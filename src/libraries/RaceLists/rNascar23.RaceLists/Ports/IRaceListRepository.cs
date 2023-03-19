using rNascar23.RaceLists.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rNascar23.RaceLists.Ports
{
    public interface IRaceListRepository
    {
        RaceList GetRaceList();
    }
}
