using rNascar23.Flags.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.Data.Flags.Ports
{
    public interface IFlagStateRepository
    {
        Task<IList<FlagState>> GetFlagStatesAsync();
    }
}
