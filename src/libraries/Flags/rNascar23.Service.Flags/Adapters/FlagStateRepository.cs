using AutoMapper;
using rNascar23.Data;
using rNascar23.Data.Flags.Ports;
using rNascar23.Flags.Models;
using rNascar23.Service.Flags.Data.Models;
using rNascar23.Service.Flags.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace rNascar23.Service.Flags.Adapters
{
    internal class FlagStateRepository : JsonDataRepository, IFlagStateRepository
    {
        private readonly IMapper _mapper;

        public FlagStateRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string Url { get => @"https://cf.nascar.com/live/feeds/live-flag-data.json"; }

        public IList<FlagState> GetFlagStates()
        {
            try
            {
                // var json = System.IO.File.ReadAllText(@"C:\Users\Rob\source\repos\rNascar23\src\libraries\FlagStates\rNascar23.FlagStates\Examples\example-flag-data.json");

                //var model = Newtonsoft.Json.JsonConvert.DeserializeObject<FlagStateListModel>(json);

                //var flagStates = _mapper.Map<List<FlagState>>(model.FlagStates);

                var json = Get(Url);

                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<FlagStateModel[]>(json);

                var flagStates = _mapper.Map<List<FlagState>>(model);

                return flagStates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
