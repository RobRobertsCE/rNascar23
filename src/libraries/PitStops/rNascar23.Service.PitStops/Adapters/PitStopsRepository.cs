using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Data;
using rNascar23.PitStops.Models;
using rNascar23.PitStops.Ports;
using rNascar23.Service.PitStops.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rNascar23.Service.PitStops.Adapters
{
    internal class PitStopsRepository : JsonDataRepository, IPitStopsRepository
    {
        #region fields

        private readonly IMapper _mapper;
        private readonly ILogger<PitStopsRepository> _logger;

        #endregion

        #region properties

        // https://cf.nascar.com/cacher/live/series_2/5314/live-pit-data.json
        public string Url { get => @"https://cf.nascar.com/cacher/live/series_{0}/{1}/live-pit-data.json"; }

        #endregion

        #region ctor

        public PitStopsRepository(IMapper mapper, ILogger<PitStopsRepository> logger)
          : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region public

        public async Task<IList<PitStop>> GetPitStopsAsync(int seriesId, int raceId)
        {
            IList<PitStop> pitStops = new List<PitStop>();

            string json = string.Empty;

            try
            {
                var absoluteUrl = BuildUrl(seriesId, raceId);

                json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                if (string.IsNullOrEmpty(json))
                    return pitStops;

                var models = JsonConvert.DeserializeObject<IList<PitStopModel>>(json);

                pitStops = _mapper.Map<IList<PitStop>>(models);

                return pitStops;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, $"Error in PitStopRepository. SeriesId: {seriesId}, RaceId: {raceId}", json);
            }

            return pitStops;
        }

        public async Task<IList<PitStop>> GetPitStopsAsync(int seriesId, int raceId, int? startLap, int? endLap = null, int? carNumber = null)
        {
            IList<PitStop> pitStops = new List<PitStop>();

            string json = string.Empty;

            try
            {
                var absoluteUrl = BuildUrl(seriesId, raceId);

                json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                if (string.IsNullOrEmpty(json))
                    return pitStops;

                var models = JsonConvert.DeserializeObject<IList<PitStopModel>>(json);

                IEnumerable<PitStopModel> filteredModels = models;

                if (carNumber.HasValue)
                    filteredModels = filteredModels.Where(p => p.vehicle_number == carNumber.Value.ToString());

                if (startLap.HasValue)
                    filteredModels = filteredModels.Where(p => p.lap_count >= startLap.Value);

                if (endLap.HasValue)
                    filteredModels = filteredModels.Where(p => p.lap_count <= endLap.Value);

                pitStops = _mapper.Map<IList<PitStop>>(filteredModels.ToList());

                return pitStops;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, $"Error in PitStopRepository. SeriesId: {seriesId}, RaceId: {raceId}", json);
            }

            return pitStops;
        }

        #endregion

        #region private

        private string BuildUrl(int seriesId, int eventId)
        {
            return String.Format(Url, seriesId, eventId);
        }

        private void ExceptionHandler(Exception ex, string message, string json)
        {
            _logger.LogError(ex, $"{message}\r\n\r\njson: {json}");
        }

        #endregion
    }
}
