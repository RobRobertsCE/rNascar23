using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Data;
using rNascar23.LapTimes.Models;
using rNascar23.LapTimes.Ports;
using rNascar23.Service.LapTimes.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rNascar23.Service.LapTimes.Adapters
{
    internal class LapAveragesRepository : JsonDataRepository, ILapAveragesRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LapAveragesRepository> _logger;

        // https://cf.nascar.com/cacher/2023/2/5314/lap-times.json
        public string Url { get => @"https://cf.nascar.com/cacher/{0}/{1}/{2}/lap-averages.json"; }

        public LapAveragesRepository(IMapper mapper, ILogger<LapAveragesRepository> logger)
            :base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<LapAverages>> GetLapAveragesAsync(int seriesId, int eventId)
        {
            string json = String.Empty;

            try
            {
                var absoluteUrl = BuildUrl(seriesId, eventId);

                json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                var model = JsonConvert.DeserializeObject<LapAveragesDataModel[]>(json);

                var lapAverages = _mapper.Map<List<LapAverages>>(model[0].Items);

                return lapAverages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reading lap average data: {ex.Message}\r\n\r\njson: {json}\r\n");
            }

            return new List<LapAverages>();
        }

        private string BuildUrl(int seriesId, int eventId)
        {
            return String.Format(Url, DateTime.Now.Year, seriesId, eventId);
        }
    }
}
