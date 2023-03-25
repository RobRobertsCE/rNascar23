using AutoMapper;
using Microsoft.Extensions.Logging;
using rNascar23.Data;
using rNascar23.RaceLists.Models;
using rNascar23.RaceLists.Ports;
using rNascar23.Service.RaceLists.Data.Models;
using System;
using System.Threading.Tasks;

namespace rNascar23.Service.RaceLists.Adapters
{
    internal class RaceListRepository: JsonDataRepository, IRaceListRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RaceListRepository> _logger;

        public RaceListRepository(IMapper mapper, ILogger<RaceListRepository> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Url { get => @"https://cf.nascar.com/cacher/2023/race_list_basic.json"; }

        public async Task<RaceList> GetRaceListAsync()
        {
            try
            {
                var json = await GetAsync(Url).ConfigureAwait(false);

                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<RaceListModel>(json);

                var raceList = _mapper.Map<RaceList>(model);

                return raceList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
