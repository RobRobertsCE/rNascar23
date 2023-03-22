using AutoMapper;
using Newtonsoft.Json;
using rNascar23.Data;
using rNascar23.LapTimes.Models;
using rNascar23.LapTimes.Ports;
using rNascar23.Service.LapTimes.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rNascar23.Service.LapTimes.Adapters
{
    internal class LapTimesRepository : JsonDataRepository, ILapTimesRepository
    {
        private readonly IMapper _mapper;

        // https://cf.nascar.com/cacher/2023/2/5314/lap-times.json
        public string Url { get => @"https://cf.nascar.com/cacher/{0}/{1}/{2}/lap-times.json"; }

        public LapTimesRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<LapTimeData> GetLapTimeDataAsync(int seriesId, int eventId)
        {
            var absoluteUrl = BuildUrl(seriesId, eventId);

            var json = await GetAsync(absoluteUrl).ConfigureAwait(false);

            var model = JsonConvert.DeserializeObject<Rootobject>(json);

            var lapTimeData = _mapper.Map<LapTimeData>(model);

            return lapTimeData;
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
