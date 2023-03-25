using AutoMapper;
using Microsoft.Extensions.Logging;
using rNascar23.Data;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.Service.LiveFeeds.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace rNascar23.Service.LiveFeeds.Adapters
{
    public class LiveFeedRepository : JsonDataRepository, ILiveFeedRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LiveFeedRepository> _logger;

        public LiveFeedRepository(IMapper mapper, ILogger<LiveFeedRepository> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Url { get => @"https://cf.nascar.com/live/feeds/live-feed.json"; }

        public async Task<LiveFeed> GetLiveFeedAsync()
        {
            try
            {
                var json = await GetAsync(Url).ConfigureAwait(false);

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
