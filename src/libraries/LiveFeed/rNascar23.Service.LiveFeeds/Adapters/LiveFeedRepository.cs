using AutoMapper;
using rNascar23.Data;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.Service.LiveFeeds.Data.Models;
using System;
using System.Linq;

namespace rNascar23.Service.LiveFeeds.Adapters
{
    public class LiveFeedRepository : JsonDataRepository, ILiveFeedRepository
    {
        private readonly IMapper _mapper;

        public LiveFeedRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string Url { get => @"https://cf.nascar.com/live/feeds/live-feed.json"; }

        public LiveFeed GetLiveFeed()
        {
            try
            {
                var json = Get(Url);

                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<LiveFeedModel>(json);

                var liveFeed = _mapper.Map<LiveFeed>(model);

                foreach (var vehicle in liveFeed.Vehicles)
                {
                    if (vehicle.pit_stops.Count > 0)
                    {
                        var lastStop = vehicle.pit_stops.OrderBy(p => p.pit_in_leader_lap).LastOrDefault();
                        vehicle.last_pit = lastStop.pit_in_leader_lap;
                    }
                }

                return liveFeed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
