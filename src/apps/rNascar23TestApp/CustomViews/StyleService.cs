using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace rNascar23TestApp.CustomViews
{
    public class StyleService : IStyleService
    {
        #region properties

        public static string StylesFile
        {
            get
            {
                return GetCustomStylesFilePath();
            }
        }

        #endregion

        #region fields

        private readonly ILogger<StyleService> _logger = null;

        #endregion

        #region ctor

        public StyleService(ILogger<StyleService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region public

        public IList<GridStyleSettings> GetStyles()
        {
            IList<GridStyleSettings> styleSettings = null;

            try
            {
                styleSettings = LoadStyles();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error reading custom grid settings");
            }

            return styleSettings;
        }

        public GridStyleSettings GetStyle(string styleName)
        {
            GridStyleSettings style = null;

            try
            {
                var styles = GetStyles();

                style = styles.FirstOrDefault(s => s.Name == styleName);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, $"Error reading custom grid setting for style {styleName}");
            }

            return style;
        }

        public void SaveStyles(IList<GridStyleSettings> customStyleSettings)
        {
            try
            {
                SaveCustomStyles(customStyleSettings);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error writing custom grid settings");
            }
        }

        public static string GetCustomStylesFilePath()
        {
            var settings = UserSettingsService.LoadUserSettings();

            string filePath = Path.Combine(settings.DataDirectory, JsonFileHelper.GetStylesDataFile());

            return filePath;
        }

        #endregion

        #region protected

        protected virtual void SaveCustomStyles(IList<GridStyleSettings> customStyleSettings)
        {
            var filePath = GetCustomStylesFilePath();

            var json = JsonConvert.SerializeObject(customStyleSettings, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        protected virtual IList<GridStyleSettings> LoadStyles()
        {
            IList<GridStyleSettings> customStyleSettings;

            var filePath = GetCustomStylesFilePath();

            if (!File.Exists(filePath))
            {
                customStyleSettings = new List<GridStyleSettings>();
            }
            else
            {
                var json = File.ReadAllText(filePath);

                customStyleSettings = JsonConvert.DeserializeObject<List<GridStyleSettings>>(json);
            }

            return customStyleSettings;
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
