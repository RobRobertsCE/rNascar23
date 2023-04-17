using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Common;
using rNascar23.Data;
using rNascar23.LiveFeeds.Ports;
using rNascar23.LoopData.Models;
using rNascar23.LoopData.Ports;
using rNascar23.Schedules.Ports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace rNascar23.Service.LoopData.Adapters
{
    internal class DriverInfoRepository : JsonDataRepository, IDriverInfoRepository
    {
        #region consts

        private const string DataFileName = "DriverInfo.json";

        #endregion

        #region fields

        private readonly IMapper _mapper;
        private readonly ILogger<DriverInfoRepository> _logger;
        private readonly IWeekendFeedRepository _weekendFeedRepository = null;
        private readonly ISchedulesRepository _scheduleRepository = null;

        #endregion

        #region ctor

        public DriverInfoRepository(
            IMapper mapper,
            ILogger<DriverInfoRepository> logger,
            IWeekendFeedRepository weekendFeedRepository,
            ISchedulesRepository scheduleRepository)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _weekendFeedRepository = weekendFeedRepository ?? throw new ArgumentNullException(nameof(weekendFeedRepository));
            _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
        }

        #endregion

        #region public

        public async Task<DriverInfo> GetDriverAsync(int id)
        {
            DriverInfo model = null;

            try
            {
                var driverData = LoadDriverData();

                List<DriverInfo> data = driverData.ToList();

                if (data.Count == 0)
                {
                    await LoadDriversFromScheduleAsync(data);
                }

                model = data.FirstOrDefault(d => d.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading driver info for driverId {id}");
            }

            return model;
        }

        public async Task<IList<DriverInfo>> GetDriversAsync(bool updateFromCompletedRaces = false)
        {
            List<DriverInfo> models = null;

            try
            {
                var driverData = LoadDriverData();

                models = driverData.ToList();

                if (models.Count == 0 || updateFromCompletedRaces)
                {
                    await LoadDriversFromScheduleAsync(models);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading driver info for all drivers");
            }

            return models;
        }

        public async Task<IList<DriverInfo>> GetDriversAsync(int seriesId, int raceId, int year)
        {
            List<DriverInfo> eventDrivers = new List<DriverInfo>();

            try
            {
                var driverData = LoadDriverData();

                List<DriverInfo> data = driverData.ToList();

                eventDrivers = await GetDriversFromEventAsync(seriesId, raceId, year);

                MergeDrivers(data, eventDrivers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading driver info for seriesId {seriesId}, raceId {raceId}, year {year}");
            }

            return eventDrivers;
        }

        #endregion

        #region private

        private async Task LoadDriversFromScheduleAsync(List<DriverInfo> data)
        {
            var raceLists = await _scheduleRepository.GetRaceListAsync();

            foreach (var race in raceLists.TruckSeries.Where(t => t.DateScheduled.Date < DateTime.Now.Date))
            {
                var eventDrivers = await GetDriversFromEventAsync(race.SeriesId, race.RaceId, race.RaceSeason);

                MergeDrivers(data, eventDrivers);
            }
            foreach (var race in raceLists.XfinitySeries.Where(t => t.DateScheduled.Date < DateTime.Now.Date))
            {
                var eventDrivers = await GetDriversFromEventAsync(race.SeriesId, race.RaceId, race.RaceSeason);

                MergeDrivers(data, eventDrivers);
            }
            foreach (var race in raceLists.CupSeries.Where(t => t.DateScheduled.Date < DateTime.Now.Date))
            {
                var eventDrivers = await GetDriversFromEventAsync(race.SeriesId, race.RaceId, race.RaceSeason);

                MergeDrivers(data, eventDrivers);
            }
        }

        private void MergeDrivers(List<DriverInfo> data, List<DriverInfo> models)
        {
            var driversToAdd = new List<DriverInfo>();

            foreach (var model in models.ToList())
            {
                if (!data.Any(d => d.Id == model.Id))
                {
                    driversToAdd.Add(model);
                }
            }

            if (driversToAdd.Count > 0)
            {
                data.AddRange(driversToAdd);

                SaveDriverInfo(data);
            }
        }

        private async Task<List<DriverInfo>> GetDriversFromEventAsync(int seriesId, int raceId, int year)
        {
            List<DriverInfo> models = new List<DriverInfo>();

            var weekendData = await _weekendFeedRepository.GetWeekendFeedAsync(seriesId, raceId);

            var raceResults = weekendData.weekend_race.FirstOrDefault();

            if (raceResults == null)
            {
                var runResults = weekendData.weekend_runs.FirstOrDefault();

                if (runResults == null)
                    return models;
                else
                {
                    foreach (var runResult in runResults.results)
                    {
                        var model = new DriverInfo()
                        {
                            Id = runResult.driver_id,
                            Name = runResult.driver_name
                        };

                        models.Add(model);
                    }
                }
            }
            else
            {
                foreach (var runResult in raceResults.results)
                {
                    var model = new DriverInfo()
                    {
                        Id = runResult.driver_id,
                        Name = runResult.driver_fullname
                    };

                    models.Add(model);
                }
            }

            return models;
        }

        private IList<DriverInfo> LoadDriverData()
        {
            var fileName = BuildDataFilePath();

            if (!File.Exists(fileName))
            {
                return new List<DriverInfo>();
            }

            var json = File.ReadAllText(fileName);

            if (string.IsNullOrEmpty(json))
            {
                return new List<DriverInfo>();
            }

            var data = JsonConvert.DeserializeObject<List<DriverInfo>>(json);

            return data;
        }

        private void SaveDriverInfo(IList<DriverInfo> models)
        {
            var fileName = BuildDataFilePath();

            var json = JsonConvert.SerializeObject(models, Formatting.Indented);

            File.WriteAllText(fileName, json);
        }

        private string BuildDataFilePath()
        {
            var fileName = JsonFileHelper.GetDriverInfoDataFile();

            var settings = UserSettingsService.LoadUserSettings();

            return Path.Combine(settings.DataDirectory, fileName);
        }

        #endregion
    }
}
