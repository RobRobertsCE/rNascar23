using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Data;
using rNascar23.LiveFeeds.Models;
using rNascar23.LiveFeeds.Ports;
using rNascar23.Service.LiveFeeds.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rNascar23.Service.LiveFeeds.Adapters
{
    internal class KeyMomentsRepository : JsonDataRepository, IKeyMomentsRepository
    {
        private readonly ILogger<KeyMomentsRepository> _logger;

        // https://cf.nascar.com/cacher/2023/2/5314/lap-notes.json
        public string Url { get => @"https://cf.nascar.com/cacher/{0}/{1}/{2}/lap-notes.json"; }

        public KeyMomentsRepository(ILogger<KeyMomentsRepository> logger)
            : base(logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<KeyMoment>> GetKeyMomentsAsync(int seriesId, int raceId, int? year = null)
        {
            try
            {
                var absoluteUrl = BuildUrl(seriesId, raceId, year);

                var json = await GetAsync(absoluteUrl).ConfigureAwait(false);

                if (string.IsNullOrEmpty(json))
                    return new List<KeyMoment>();

                var model = JsonConvert.DeserializeObject<KeyMomentModelList>(json);

                var keyMomentsList = new List<KeyMoment>();

                foreach (KeyValuePair<int, KeyMomentModel[]> lapModel in model.Laps)
                {
                    for (int i = 0; i < lapModel.Value.Length; i++)
                    {
                        keyMomentsList.Add(new KeyMoment()
                        {
                            LapNumber = lapModel.Key,
                            Note = lapModel.Value[i].Note,
                            NoteID = lapModel.Value[i].NoteID,
                            FlagState = lapModel.Value[i].FlagState,
                        });
                    }
                }

                return keyMomentsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception reading KeyMoments");
            }

            return new List<KeyMoment>();
        }

        private string BuildUrl(int seriesId, int raceId, int? year = null)
        {
            return String.Format(Url, year.GetValueOrDefault(DateTime.Now.Year), seriesId, raceId);
        }
    }
}
