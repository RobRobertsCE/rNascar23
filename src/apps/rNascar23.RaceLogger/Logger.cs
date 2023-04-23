using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Common;
using rNascar23.Data.Flags.Ports;
using rNascar23.Data.LiveFeeds.Ports;
using rNascar23.LapTimes.Ports;
using rNascar23.LiveFeeds.Models;
using rNascar23.LoopData.Ports;
using rNascar23.PitStops.Ports;
using rNascar23.Points.Ports;
using rNascar23.Schedules.Ports;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.RaceLogger
{
    public partial class Logger : Form
    {
        #region consts

        private const string EventsDirectory = "C:\\Users\\Rob\\Documents\\rNascar23\\Events";
        private const int MaxNumberOfLines = 50;
        private const string LogFileName = "rNascar23.Logger.Log.{0}.txt";
        private const bool LineByLineComparison = false;

        #endregion

        #region fields

        private bool _isRunning = false;
        private DateTime _lastLiveFeedTimestamp = DateTime.MinValue;

        private string _previousState = String.Empty;
        private readonly FormState _formState = new FormState();

        private readonly ILogger<Logger> _logger = null;
        private readonly ILapTimesRepository _lapTimeRepository = null;
        private readonly ILapAveragesRepository _lapAveragesRepository = null;
        private readonly ILiveFeedRepository _liveFeedRepository = null;
        private readonly ILoopDataRepository _driverStatisticsRepository = null;
        private readonly IFlagStateRepository _flagStateRepository = null;
        private readonly ISchedulesRepository _raceScheduleRepository = null;
        private readonly IPointsRepository _pointsRepository = null;
        private readonly IPitStopsRepository _pitStopsRepository = null;

        #endregion

        #region ctor/load

        public Logger(
            ILogger<Logger> logger,
            ILapTimesRepository lapTimeRepository,
            ILapAveragesRepository lapAveragesRepository,
            ILiveFeedRepository liveFeedRepository,
            ILoopDataRepository driverStatisticsRepository,
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

        private void Logger_Load(object sender, EventArgs e)
        {
            WriteMessage("App started");
        }

        #endregion

        #region private [event handlers]

        private async void dataRefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var hasUpdates = await CheckForUpdatesAsync();

                if (hasUpdates)
                    SaveFormStateToFile();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception in update timer");
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleUpdateTimerState();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Exception setting update timer state.");
            }
        }

        private void btnLogFile_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayLogFile();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #endregion

        #region private

        private async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                WriteMessage("Checking for updates");

                picCheckForUpdate.BackColor = Color.LawnGreen;

                _formState.LiveFeed = await _liveFeedRepository.GetLiveFeedAsync();

                var liveFeedTimestamp = DateTime.Parse(_formState.LiveFeed.TimeOfDayOs);

                if (liveFeedTimestamp == _lastLiveFeedTimestamp)
                    return false;

                var timeSinceLastUpdate = _lastLiveFeedTimestamp == DateTime.MinValue ?
                    new TimeSpan() :
                    liveFeedTimestamp.Subtract(_lastLiveFeedTimestamp);

                WriteMessage($"Update found. Elapsed Time: {_formState.LiveFeed.ElapsedTime}. Time since last update: {timeSinceLastUpdate.ToString("mm\\.ss")} (mm.ss).");

                _lastLiveFeedTimestamp = liveFeedTimestamp;

                if (!DataHasChanges(_formState))
                {
                    WriteMessage("No data changes");
                    return false;
                }

                UpdateFound(_formState.LiveFeed);

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
                ExceptionHandler(ex, "Exception checking for updates");
            }
            finally
            {
                picCheckForUpdate.BackColor = Color.White;
            }

            return false;
        }

        private bool DataHasChanges(FormState formState)
        {
            bool hasChanges = false;

            try
            {
                dataRefreshTimer.Enabled = false;

                // normalize
                var newStateJson = JsonConvert.SerializeObject(formState);

                var tempState = JsonConvert.DeserializeObject<FormState>(newStateJson);

                tempState.LiveFeed.TimeOfDayOs = DateTime.MinValue;
                tempState.LiveFeed.ElapsedTime = 0;
                tempState.LiveFeed.TimeOfDay = 0;

                var tempStateJson = JsonConvert.SerializeObject(tempState, Formatting.Indented);

                // compare
                if (string.Compare(tempStateJson, _previousState) == 0)
                {
                    hasChanges = false;
                }

                var previousLines = _previousState.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var newLines = tempStateJson.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (previousLines.Length != newLines.Length)
                {
                    hasChanges = true;
                }

                if (LineByLineComparison && hasChanges == false)
                {
                    var minLines = previousLines.Length <= newLines.Length ? previousLines.Length : newLines.Length;

                    for (int i = 0; i < minLines; i++)
                    {
                        var previousLine = previousLines[i];
                        var newLine = newLines[i];

                        if (!previousLine.Equals(newLine, StringComparison.Ordinal))
                        {
                            WriteMessage($"UNEQUAL: {previousLine} -> {newLine}");
                            hasChanges = true;
                        }
                    }
                }

                _previousState = tempStateJson;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dataRefreshTimer.Enabled = true;
            }

            return hasChanges;
        }

        private void UpdateFound(LiveFeed liveFeed)
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

        private void SaveFormStateToFile()
        {
            try
            {
                var seriesName = _formState.LiveFeed.SeriesId == 1 ? "Cup" :
                    _formState.LiveFeed.SeriesId == 2 ? "XFinity" :
                    _formState.LiveFeed.SeriesId == 3 ? "Truck" :
                    "Other Series";

                var runType = _formState.LiveFeed.RunType == 1 ? "Practice" :
                    _formState.LiveFeed.RunType == 2 ? "Qualifying" :
                    _formState.LiveFeed.RunType == 3 ? "Race" :
                    "Other Run Type";

                var sanitizedRunName = _formState.LiveFeed.RunName.Replace(".", "_").Replace("\\", "_").Replace("/", "_");

                var eventDirectoryName = $"{DateTime.Now.ToString("yyyy-M-d")}.{_formState.LiveFeed.TrackName}-{seriesName}-{sanitizedRunName}-{runType}-{_formState.LiveFeed.RunId}";

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
                ExceptionHandler(ex, "Exception serializing form state to file");
            }
        }

        public void CompressFile(string path)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionHandler(ex, $"Exception compressing file: {path}");
            }
        }

        public void ToggleUpdateTimerState()
        {
            if (_isRunning)
            {
                WriteMessage("Stopping update timer");
                btnStartStop.Text = "Start";
                dataRefreshTimer.Enabled = false;
                picOnOff.BackColor = Color.White;
            }
            else
            {
                WriteMessage("Starting update timer");
                btnStartStop.Text = "Stop";
                dataRefreshTimer.Enabled = true;
                picOnOff.BackColor = Color.LawnGreen;
            }

            _isRunning = !_isRunning;
        }

        private void WriteMessage(string message)
        {
            TrimMessagesLines();

            var formattedMessageLine = $"{DateTime.Now} {message}{Environment.NewLine}";

            txtMessages.AppendText(formattedMessageLine);
        }

        private void TrimMessagesLines()
        {
            var lines = (from item in txtMessages.Text.Split('\n') select item.Trim());

            if (lines.Count() < MaxNumberOfLines)
                return;

            var newlines = lines.Skip(lines.Count() - MaxNumberOfLines);

            txtMessages.Text = string.Join(Environment.NewLine, newlines.ToArray());
        }

        private void DisplayLogFile()
        {
            string currentLogFile = String.Format(LogFileName, DateTime.Now.ToString("yyyyMMdd")); ;

            var settings = UserSettingsService.LoadUserSettings();

            string logFileDirectory = settings.LogDirectory;

            string logFilePath = Path.Combine(logFileDirectory, currentLogFile);

            if (!File.Exists(logFilePath))
            {
                _logger.LogInformation($"Log file created {DateTime.Now}");
            }

            Process.Start("Notepad++.exe", logFilePath);
        }

        private void ExceptionHandler(Exception ex, string message = "")
        {
            try
            {
                WriteMessage(ex.Message);

                string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

                _logger.LogError(ex, errorMessage);
            }
            catch (Exception)
            {
                WriteMessage(ex.ToString());
            }
        }

        #endregion
    }
}
