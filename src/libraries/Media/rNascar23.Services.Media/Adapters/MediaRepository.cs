using Microsoft.Extensions.Logging;
using RestSharp;
using rNascar23.Media.Models;
using rNascar23.Media.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rNascar23.Services.Media
{
    public class MediaRepository : IMediaRepository
    {
        #region fields

        private readonly IList<MediaImage> _mediaCache = new List<MediaImage>();
        private readonly ILogger<MediaRepository> _logger;

        #endregion

        #region ctor

        public MediaRepository(ILogger<MediaRepository> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region public

        public async Task<byte[]> GetCarNumberImageAsync(int seriesId, int carNumber)
        {
            try
            {
                var cached = _mediaCache.FirstOrDefault(
                    c => c.MediaType == MediaTypes.CarNumber &&
                    c.SeriesId == seriesId &&
                    c.CarNumber == carNumber); ;

                if (cached != null)
                {
                    return cached.Image;
                }

                var image = await DownloadCarNumberAsync(seriesId, carNumber);

                if (image != null)
                {
                    _mediaCache.Add(new MediaImage()
                    {
                        MediaType = MediaTypes.CarNumber,
                        SeriesId = seriesId,
                        CarNumber = carNumber,
                        Image = image
                    });

                    return image;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, $"Error loading car number media for series id {seriesId}, car number {carNumber}");
            }

            return null;
        }

        public MediaImage GetCarNumberImage(int seriesId, int carNumber)
        {
            try
            {
                var cached = _mediaCache.FirstOrDefault(
                    c => c.MediaType == MediaTypes.CarNumber &&
                    c.SeriesId == seriesId &&
                    c.CarNumber == carNumber); ;

                if (cached != null)
                {
                    return cached;
                }

                var image = DownloadCarNumber(seriesId, carNumber);

                if (image != null)
                {
                    var mediaImage = new MediaImage()
                    {
                        MediaType = MediaTypes.CarNumber,
                        SeriesId = seriesId,
                        CarNumber = carNumber,
                        Image = image
                    };

                    _mediaCache.Add(mediaImage);

                    return mediaImage;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, $"Error loading car number media for series id {seriesId}, car number {carNumber}");
            }

            return null;
        }

        #endregion

        #region private

        private byte[] DownloadCarNumber(int seriesId, int carNumber)
        {
            // https://cf.nascar.com/data/images/carbadges/1/16.png

            string url = $"https://cf.nascar.com/data/images/carbadges/{seriesId}/{carNumber}.png";

            var client = new RestClient(url);

            var request = new RestRequest(String.Empty, Method.Get);

            var fileBytes = client.DownloadData(request);

            return fileBytes;
        }

        private async Task<byte[]> DownloadCarNumberAsync(int seriesId, int carNumber)
        {
            // https://cf.nascar.com/data/images/carbadges/1/16.png

            string url = $"https://cf.nascar.com/data/images/carbadges/{seriesId}/{carNumber}.png";

            var client = new RestClient(url);

            var request = new RestRequest(String.Empty, Method.Get);

            var fileBytes = await client.DownloadDataAsync(request);

            return fileBytes;
        }

        private void ExceptionHandler(Exception ex, string message = "")
        {
            _logger.LogError(ex, message);
        }

        #endregion
    }
}
