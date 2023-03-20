using rNascar23.Flags.Models;
using System.Collections.Generic;

namespace rNascar23.Data.Flags.Ports
{
    public interface IFlagStateRepository
    {
        IList<FlagState> GetFlagStates();
    }
}
