using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Data;
using rNascar23.LiveFeeds.Models;
using rNascar23.LiveFeeds.Ports;
using rNascar23.Service.LiveFeeds.Data.Models;
using System;
using System.Threading.Tasks;

namespace rNascar23.Service.LiveFeeds.Adapters
{
    internal class WeekendFeedRepository : JsonDataRepository, IWeekendFeedRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<WeekendFeedRepository> _logger;

        public WeekendFeedRepository(IMapper mapper, ILogger<WeekendFeedRepository> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // https://cf.nascar.com/cacher/2023/1/5274/weekend-feed.json
        public string Url { get => @"https://cf.nascar.com/cacher/{0}/{1}/{2}/weekend-feed.json"; }
        
        public async Task<WeekendFeed> GetWeekendFeedAsync(int seriesId, int raceId)
        {
            var absoluteUrl = BuildUrl(seriesId, raceId);

            var json = await GetAsync(absoluteUrl).ConfigureAwait(false);

            if (string.IsNullOrEmpty(json))
                return new WeekendFeed();

            var model = JsonConvert.DeserializeObject<WeekendFeedModel>(json);

            var weekendFeed = _mapper.Map<WeekendFeed>(model);

            return weekendFeed;
        }

        private string BuildUrl(int seriesId, int raceId)
        {
            return String.Format(Url, DateTime.Now.Year, seriesId, raceId);
        }
    }
}
