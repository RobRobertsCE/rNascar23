using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using rNascar23.Data;
using rNascar23.Media.Models;
using rNascar23.Media.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rNascar23.Service.Media
{
    public class MediaRepository : JsonDataRepository, IMediaRepository
    {
        #region fields

        private readonly IList<MediaImage> _mediaCache = new List<MediaImage>();
        private readonly ILogger<MediaRepository> _logger;

        #endregion

        #region properties

        // https://cf.nascar.com/config/audio/audio_mapping_1_3.json
        public string AudioUrl { get => @"https://cf.nascar.com/config/audio/audio_mapping_{0}_3.json"; }
        // https://cf.nascar.com/drive/1/configs-ng.json
        public string VideoUrl { get => @"https://cf.nascar.com/drive/{0}/configs-ng.json"; }

        #endregion

        #region ctor

        public MediaRepository(ILogger<MediaRepository> logger)
            :base(logger)
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

        public async Task<AudioConfiguration> GetAudioConfigurationAsync(int seriesId)
        {
            string json = String.Empty;

            try
            {
                var absoluteUrl = BuildAudioUrl(seriesId);

                json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                if (string.IsNullOrEmpty(json))
                    return new AudioConfiguration();

                var model = JsonConvert.DeserializeObject<AudioConfiguration>(json);

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reading audio configuration: {ex.Message}\r\n\r\njson: {json}\r\n");
            }

            return new AudioConfiguration();
        }

        public async Task<VideoConfiguration> GetVideoConfigurationAsync(int seriesId)
        {
            string json = String.Empty;

            try
            {
                var absoluteUrl = BuildVideoUrl(seriesId);

                json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                if (string.IsNullOrEmpty(json))
                    return new VideoConfiguration();

                var model = JsonConvert.DeserializeObject<VideoConfiguration>(json);

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reading video configuration: {ex.Message}\r\n\r\njson: {json}\r\n");
            }

            return new VideoConfiguration();
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

        private string BuildAudioUrl(int seriesId)
        {
            return String.Format(AudioUrl, seriesId);
        }

        private string BuildVideoUrl(int seriesId)
        {
            return String.Format(VideoUrl, seriesId);
        }

        private void ExceptionHandler(Exception ex, string message = "")
        {
            _logger.LogError(ex, message);
        }

        #endregion
    }
}
