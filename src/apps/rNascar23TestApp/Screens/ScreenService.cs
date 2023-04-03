using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace rNascar23TestApp.Screens
{
    public class ScreenService : IScreenService
    {
        #region properties

        public static string ScreenDefinitionsFile
        {
            get
            {
                return GetScreenFilePath();
            }
        }

        #endregion

        #region fields

        private readonly ILogger<ScreenService> _logger = null;

        #endregion

        #region ctor

        public ScreenService(ILogger<ScreenService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region public

        public IList<ScreenDefinition> GetScreenDefinitions()
        {
            IList<ScreenDefinition> screenDefinitions = null;

            try
            {
                screenDefinitions = LoadScreens();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error reading screen definitions");
            }

            return screenDefinitions;
        }

        public void SaveScreenDefinitions(IList<ScreenDefinition> screenDefinitions)
        {
            try
            {
                SaveScreens(screenDefinitions);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error writing screen definitions");
            }
        }

        public static string GetScreenFilePath()
        {
            var settings = UserSettingsService.LoadUserSettings();

            string filePath = Path.Combine(settings.DataDirectory, JsonFileHelper.GetScreensDataFile());

            return filePath;
        }

        #endregion

        #region protected

        protected virtual void SaveScreens(IList<ScreenDefinition> screenDefinitions)
        {
            var filePath = GetScreenFilePath();

            var json = JsonConvert.SerializeObject(screenDefinitions, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        protected virtual IList<ScreenDefinition> LoadScreens()
        {
            IList<ScreenDefinition> screenDefinitions;

            var filePath = GetScreenFilePath();

            if (!File.Exists(filePath))
            {
                screenDefinitions = new List<ScreenDefinition>();
            }
            else
            {
                var json = File.ReadAllText(filePath);

                screenDefinitions = JsonConvert.DeserializeObject<List<ScreenDefinition>>(json);
            }

            return screenDefinitions;
        }

        #endregion

        #region private

        private void ExceptionHandler(Exception ex, string message = "")
        {
            ExceptionHandler(ex, message, true);
        }
        private void ExceptionHandler(Exception ex, string message = "", bool logMessage = false)
        {
            MessageBox.Show(ex.Message);
            if (logMessage)
            {
                string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

                _logger.LogError(ex, errorMessage);
            }
        }

        #endregion
    }
}
