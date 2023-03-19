using AutoMapper;
using rNascar23.Data;
using rNascar23.RaceLists.Models;
using rNascar23.RaceLists.Ports;
using rNascar23.Service.RaceLists.Data.Models;
using System;

namespace rNascar23.Service.RaceLists.Adapters
{
    internal class RaceListRepository: JsonDataRepository, IRaceListRepository
    {
        private readonly IMapper _mapper;

        public RaceListRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string Url { get => @"https://cf.nascar.com/cacher/2023/race_list_basic.json"; }

        public RaceList GetRaceList()
        {
            try
            {
                var json = Get(Url);

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
