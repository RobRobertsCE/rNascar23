using AutoMapper;
using Microsoft.Extensions.Logging;
using rNascar23.Data;
using rNascar23.Schedules.Models;
using rNascar23.Schedules.Ports;
using rNascar23.Service.RaceLists.Data.Models;
using System;
using System.Threading.Tasks;

namespace rNascar23.Service.RaceLists.Adapters
{
    internal class SchedulesRepository : JsonDataRepository, ISchedulesRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SchedulesRepository> _logger;
        private ScheduleCache _cache = null;
        private TimeSpan _cacheDuration = new TimeSpan(0, 15, 0);

        public SchedulesRepository(IMapper mapper, ILogger<SchedulesRepository> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Url { get => @"https://cf.nascar.com/cacher/{0}/race_list_basic.json"; }

        public async Task<SeriesSchedules> GetRaceListAsync(int? year = null)
        {
            try
            {
                if (_cache != null)
                {
                    if (DateTime.Now.Subtract(_cache.Timestamp) < _cacheDuration)
                        return _cache.SeriesSchedules;
                }

                var absoluteUrl = BuildUrl(year);

                var json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                if (string.IsNullOrEmpty(json))
                    return new SeriesSchedules();

                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<SeriesEventModel>(json);

                var seriesSchedules = _mapper.Map<SeriesSchedules>(model);

                foreach (var seriesEvent in seriesSchedules.AllSeries)
                {
                    foreach (var seriesEventActivity in seriesEvent.Schedule)
                    {
                        seriesEventActivity.SeriesId = seriesEvent.SeriesId;
                    }
                }

                _cache = new ScheduleCache()
                {
                    Timestamp = DateTime.Now,
                    SeriesSchedules = seriesSchedules
                };

                return seriesSchedules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception reading schedules");
            }

            return null;
        }

        private string BuildUrl(int? year = null)
        {
            var queryYear = year.HasValue ? year.Value : DateTime.Now.Year;

            return String.Format(Url, queryYear);
        }

        private class ScheduleCache
        {
            public DateTime Timestamp { get; set; }
            public SeriesSchedules SeriesSchedules { get; set; }
        }
    }
}
