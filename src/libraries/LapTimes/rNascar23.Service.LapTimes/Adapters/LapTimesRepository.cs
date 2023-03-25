using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Data;
using rNascar23.LapTimes.Models;
using rNascar23.LapTimes.Ports;
using rNascar23.Service.LapTimes.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace rNascar23.Service.LapTimes.Adapters
{
    internal class LapTimesRepository : JsonDataRepository, ILapTimesRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LapTimesRepository> _logger;

        // https://cf.nascar.com/cacher/2023/2/5314/lap-times.json
        public string Url { get => @"https://cf.nascar.com/cacher/{0}/{1}/{2}/lap-times.json"; }

        public LapTimesRepository(IMapper mapper, ILogger<LapTimesRepository> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LapTimeData> GetLapTimeDataAsync(int seriesId, int eventId)
        {
            string json = String.Empty;

            try
            {
                var absoluteUrl = BuildUrl(seriesId, eventId);

                json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                if (json.Contains("<Error>"))
                {
                    var errorObject = (Error)new XmlSerializer(typeof(Error)).Deserialize(new StringReader(json));

                    _logger.LogInformation($"Error reading lap time data: {errorObject.Message}");

                    return new LapTimeData();
                }
                else
                {
                    var model = JsonConvert.DeserializeObject<Rootobject>(json);

                    var lapTimeData = _mapper.Map<LapTimeData>(model);

                    return lapTimeData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reading lap time data: {ex.Message}\r\n\r\njson: {json}\r\n");
            }

            return new LapTimeData();
        }

        public async Task<LapTimeData> GetLapTimeDataAsync(int seriesId, int eventId, int driverId)
        {
            var absoluteUrl = BuildUrl(seriesId, eventId);

            var json = await GetAsync(absoluteUrl);

            var model = JsonConvert.DeserializeObject<Rootobject>(json);

            var lapTimeData = _mapper.Map<LapTimeData>(model);

            var driverLapTimeData = new LapTimeData()
            {
                LapFlags = lapTimeData.LapFlags,
                Drivers = lapTimeData.Drivers.Where(d => d.NASCARDriverID == driverId).ToList(),
            };

            return driverLapTimeData;
        }

        private string BuildUrl(int seriesId, int eventId)
        {
            return String.Format(Url, DateTime.Now.Year, seriesId, eventId);
        }
    }
}
