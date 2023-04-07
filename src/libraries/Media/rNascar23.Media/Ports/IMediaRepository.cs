using rNascar23.Media.Models;
using System.Threading.Tasks;

namespace rNascar23.Media.Ports
{
    public interface IMediaRepository
    {
        MediaImage GetCarNumberImage(int seriesId, int carNumber);
        Task<byte[]> GetCarNumberImageAsync(int seriesId, int carNumber);
    }
}