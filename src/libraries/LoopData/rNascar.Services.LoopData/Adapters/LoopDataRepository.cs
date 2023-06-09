﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Data;
using rNascar23.LoopData.Models;
using rNascar23.LoopData.Ports;
using rNascar23.Service.LoopData.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace rNascar23.Service.LoopData.Adapters
{
    internal class LoopDataRepository : JsonDataRepository, ILoopDataRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LoopDataRepository> _logger;

        public LoopDataRepository(IMapper mapper, ILogger<LoopDataRepository> logger)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // https://cf.nascar.com/loopstats/prod/2023/2/5314.json
        public string Url { get => @"https://cf.nascar.com/loopstats/prod/{0}/{1}/{2}.json"; }

        public async Task<EventStats> GetEventAsync(int seriesId, int raceId)
        {
            var absoluteUrl = BuildUrl(seriesId, raceId);

            var json = await GetAsync(absoluteUrl).ConfigureAwait(false);

            if (string.IsNullOrEmpty(json))
                return new EventStats();

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
