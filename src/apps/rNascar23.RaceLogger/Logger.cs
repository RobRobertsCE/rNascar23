﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using rNascar23.Data.Flags.Ports;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.DriverStatistics.Ports;
using rNascar23.LapTimes.Ports;
using rNascar23.PitStops.Ports;
using rNascar23.Points.Ports;
using rNascar23.Schedules.Ports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using rNascar23.LiveFeeds.Models;

namespace rNascar23.RaceLogger
{
    public partial class Logger : Form
    {
        #region consts

        private const string EventsDirectory = "C:\\Users\\Rob\\Documents\\rNascar23\\Events";

        #endregion

        #region fields

        private bool _isRunning = false;
        private DateTime _lastLiveFeedTimestamp = DateTime.MinValue;

        private readonly FormState _formState = new FormState();

        private readonly ILogger<Logger> _logger = null;
        private readonly ILapTimesRepository _lapTimeRepository = null;
        private readonly ILapAveragesRepository _lapAveragesRepository = null;
        private readonly ILiveFeedRepository _liveFeedRepository = null;
        private readonly IDriverStatisticsRepository _driverStatisticsRepository = null;
        private readonly IFlagStateRepository _flagStateRepository = null;
        private readonly ISchedulesRepository _raceScheduleRepository = null;
        private readonly IPointsRepository _pointsRepository = null;
        private readonly IPitStopsRepository _pitStopsRepository = null;

        #endregion

        #region ctor

        public Logger(
            ILogger<Logger> logger,
            ILapTimesRepository lapTimeRepository,
            ILapAveragesRepository lapAveragesRepository,
            ILiveFeedRepository liveFeedRepository,
            IDriverStatisticsRepository driverStatisticsRepository,
            IFlagStateRepository flagStateRepository,
            ISchedulesRepository raceScheduleRepository,
            IPointsRepository pointsRepository,
            IPitStopsRepository pitStopsRepository)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lapTimeRepository = lapTimeRepository ?? throw new ArgumentNullException(nameof(lapTimeRepository));
            _lapAveragesRepository = lapAveragesRepository ?? throw new ArgumentNullException(nameof(lapAveragesRepository));
            _liveFeedRepository = liveFeedRepository ?? throw new ArgumentNullException(nameof(liveFeedRepository));
            _driverStatisticsRepository = driverStatisticsRepository ?? throw new ArgumentNullException(nameof(driverStatisticsRepository));
            _flagStateRepository = flagStateRepository ?? throw new ArgumentNullException(nameof(flagStateRepository));
            _raceScheduleRepository = raceScheduleRepository ?? throw new ArgumentNullException(nameof(raceScheduleRepository));
            _pointsRepository = pointsRepository ?? throw new ArgumentNullException(nameof(pointsRepository));
            _pitStopsRepository = pitStopsRepository ?? throw new ArgumentNullException(nameof(pitStopsRepository));

        }

        #endregion

        #region private [event handlers]

        private async void dataRefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var hasUpdates = await CheckForUpdatesAsync();

                if (hasUpdates)
                    SaveFormState();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion

        #region private

        private void NewEvent(LiveFeed liveFeed)
        {
            lblLastUpdate.Text = DateTime.Now.ToString();

            var seriesName = _formState.LiveFeed.SeriesId == 1 ? "Cup" :
                _formState.LiveFeed.SeriesId == 2 ? "XFinity" :
                _formState.LiveFeed.SeriesId == 3 ? "Truck" :
                "Other Series";

            var runType = _formState.LiveFeed.RunType == 1 ? "Practice" :
                _formState.LiveFeed.RunType == 2 ? "Qualifying" :
                _formState.LiveFeed.RunType == 3 ? "Race" :
                "Other Run Type";

            lblEvent.Text = $"{_formState.LiveFeed.TrackName} {seriesName} {_formState.LiveFeed.RunName} {runType}";
        }

        private async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                picCheckForUpdate.BackColor = Color.LawnGreen;

                _formState.LiveFeed = await _liveFeedRepository.GetLiveFeedAsync();

                if (_formState.LiveFeed.TimeOfDayOs == _lastLiveFeedTimestamp)
                    return false;

                _lastLiveFeedTimestamp = _formState.LiveFeed.TimeOfDayOs;

                NewEvent(_formState.LiveFeed);

                _formState.LapTimes = await _lapTimeRepository.GetLapTimeDataAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

                _formState.FlagStates = await _flagStateRepository.GetFlagStatesAsync();

                _formState.EventStatistics = await _driverStatisticsRepository.GetEventAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

                if (_formState.EventStatistics != null && _formState.EventStatistics.drivers != null)
                {
                    foreach (var driverStats in _formState.EventStatistics?.drivers)
                    {
                        var liveFeedDriver = _formState.LiveFeed.Vehicles.FirstOrDefault(v => v.driver.DriverId == driverStats.DriverId);

                        if (liveFeedDriver != null)
                            driverStats.DriverName = liveFeedDriver.driver.FullName;
                    }
                }

                _formState.LapAverages = await _lapAveragesRepository.GetLapAveragesAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);

                _formState.LivePoints = await _pointsRepository.GetDriverPoints(_formState.LiveFeed.RaceId, _formState.LiveFeed.SeriesId);

                _formState.StagePoints = await _pointsRepository.GetStagePoints(_formState.LiveFeed.RaceId, _formState.LiveFeed.SeriesId);

                _formState.PitStops = await _pitStopsRepository.GetPitStopsAsync(_formState.LiveFeed.SeriesId, _formState.LiveFeed.RaceId);


                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                picCheckForUpdate.BackColor = Color.White;
            }

            return false;
        }

        private void SaveFormState()
        {
            try
            {
                picWriteFile.BackColor = Color.LawnGreen;

                var seriesName = _formState.LiveFeed.SeriesId == 1 ? "Cup" :
                    _formState.LiveFeed.SeriesId == 2 ? "XFinity" :
                    _formState.LiveFeed.SeriesId == 3 ? "Truck" :
                    "Other Series";

                var runType = _formState.LiveFeed.RunType == 1 ? "Practice" :
                    _formState.LiveFeed.RunType == 2 ? "Qualifying" :
                    _formState.LiveFeed.RunType == 3 ? "Race" :
                    "Other Run Type";

                var eventDirectoryName = $"{DateTime.Now.ToString("yyyy-dd-M")}.{_formState.LiveFeed.TrackName}-{seriesName}-{_formState.LiveFeed.RunName}-{runType}-{_formState.LiveFeed.RunId}";

                var eventDirectoryPath = Path.Combine(EventsDirectory, eventDirectoryName);

                if (!Directory.Exists(eventDirectoryPath))
                {
                    Directory.CreateDirectory(eventDirectoryPath);
                }

                var eventDirectoryInfo = new DirectoryInfo(eventDirectoryPath);

                var zipFiles = eventDirectoryInfo.GetFiles("*.gz");

                var fileName = $"{zipFiles.Length + 1}-{_formState.LiveFeed.SeriesId}-{_formState.LiveFeed.RaceId}-{_formState.LiveFeed.RunType}-{_formState.LiveFeed.RunId}-{DateTime.Now.ToString("HH-mm-ss")}.json";

                var filePath = Path.Combine(eventDirectoryPath, fileName);

                var json = JsonConvert.SerializeObject(_formState, Formatting.Indented);

                File.WriteAllText(filePath, json);

                CompressFile(filePath);

                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                picWriteFile.BackColor = Color.White;
            }
        }

        public void CompressFile(string path)
        {
            FileStream sourceFile = File.OpenRead(path);
            FileStream destinationFile = File.Create(path + ".gz");

            byte[] buffer = new byte[sourceFile.Length];
            sourceFile.Read(buffer, 0, buffer.Length);

            using (GZipStream output = new GZipStream(destinationFile,
                CompressionMode.Compress))
            {
                WriteMessage($"Compressing {Path.GetFileName(sourceFile.Name)} to {Path.GetFileName(destinationFile.Name)}.");

                output.Write(buffer, 0, buffer.Length);
            }

            // Close the files.
            sourceFile.Close();
            destinationFile.Close();
        }

        private void ExceptionHandler(Exception ex)
        {
            ExceptionHandler(ex, String.Empty, true);
        }
        private void ExceptionHandler(Exception ex, string message = "")
        {
            ExceptionHandler(ex, message, true);
        }
        private void ExceptionHandler(Exception ex, string message = "", bool logMessage = false)
        {
            WriteMessage(ex.Message);

            if (logMessage)
            {
                string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

                _logger.LogError(ex, errorMessage);
            }
        }

        private void WriteMessage(string message)
        {
            txtMessages.AppendText($"{DateTime.Now} {message}");
            txtMessages.AppendText(Environment.NewLine);
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isRunning)
                {
                    btnStartStop.Text = "Start";
                    dataRefreshTimer.Enabled = false;
                    picOnOff.BackColor = Color.White;
                }
                else
                {
                    btnStartStop.Text = "Stop";
                    dataRefreshTimer.Enabled = true;
                    picOnOff.BackColor = Color.LawnGreen;
                }

                _isRunning = !_isRunning;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion
    }
}
