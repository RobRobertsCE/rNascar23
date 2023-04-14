using AutoMapper;
using Microsoft.Extensions.Logging;
using rNascar23.Data;
using rNascar23.Schedules.Models;
using rNascar23.Schedules.Ports;
using rNascar23.Service.RaceLists.Data.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace rNascar23.Service.RaceLists.Adapters
{
    internal class SchedulesRepository : JsonDataRepository, ISchedulesRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SchedulesRepository> _logger;

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
                var absoluteUrl = BuildUrl(year);

                var json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                if (string.IsNullOrEmpty(json))
                    return new SeriesSchedules();

                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<SeriesEventModel>(json);

                var raceList = _mapper.Map<SeriesSchedules>(model);

                return raceList;
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
    }
}
