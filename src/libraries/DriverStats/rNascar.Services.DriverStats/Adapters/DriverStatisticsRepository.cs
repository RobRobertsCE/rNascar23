using AutoMapper;
using Newtonsoft.Json;
using rNascar23.Service.DriverStatistics.Data.Models;
using rNascar23.Data;
using rNascar23.DriverStatistics.Models;
using rNascar23.DriverStatistics.Ports;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace rNascar23.Service.DriverStatistics.Adapters
{
    internal class DriverStatisticsRepository : JsonDataRepository, IDriverStatisticsRepository
    {
        private readonly IMapper _mapper;

        public DriverStatisticsRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        // https://cf.nascar.com/loopstats/prod/2023/2/5314.json
        public string Url { get => @"https://cf.nascar.com/loopstats/prod/{0}/{1}/{2}.json"; }

        public async Task<EventStats> GetEventAsync(int seriesId, int raceId)
        {
            var absoluteUrl = BuildUrl(seriesId, raceId);

            var json = await GetAsync(absoluteUrl).ConfigureAwait(false);

            var model = JsonConvert.DeserializeObject<EventStatsModel[]>(json);

            var raceStats = model.FirstOrDefault();

            var eventStats = _mapper.Map<EventStats>(raceStats);

            return eventStats;
        }

        private string BuildUrl(int seriesId, int raceId)
        {
            return String.Format(Url, DateTime.Now.Year, seriesId, raceId);
        }
    }
}
