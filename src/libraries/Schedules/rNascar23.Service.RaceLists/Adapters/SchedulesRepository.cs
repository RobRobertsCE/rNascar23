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

        public SchedulesRepository(IMapper mapper, ILogger<SchedulesRepository> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Url { get => @"https://cf.nascar.com/cacher/2023/race_list_basic.json"; }

        public async Task<SeriesSchedules> GetRaceListAsync()
        {
            try
            {
                var json = await GetAsync(Url).ConfigureAwait(false);

                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<RaceListModel>(json);

                var raceList = _mapper.Map<SeriesSchedules>(model);

                return raceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
