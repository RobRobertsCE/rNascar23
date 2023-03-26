﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace rNascar23TestApp.CustomViews
{
    public class CustomViewSettingsService
    {
        #region consts

        private const string CustomViewFolderName = "CustomViews";
        private const string CustomViewFileName = "CustomViews.json";

        #endregion

        #region properties

        public static string CustomViewsFile
        {
            get
            {
                return GetCustomViewFilePath();
            }
        }

        #endregion

        #region fields

        private readonly ILogger<CustomViewSettingsService> _logger = null;

        #endregion

        #region ctor

        public CustomViewSettingsService(ILogger<CustomViewSettingsService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region public

        public IList<GridSettings> GetCustomViewSettings()
        {
            IList<GridSettings> customGridSettings = null;

            try
            {
                customGridSettings = LoadCustomViews();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error reading custom grid settings");
            }

            return customGridSettings;
        }

        public void SaveCustomViewSettings(IList<GridSettings> customGridSettings)
        {
            try
            {
                SaveCustomViews(customGridSettings);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error writing custom grid settings");
            }
        }

        #endregion

        #region protected

        protected virtual void SaveCustomViews(IList<GridSettings> customGridSettings)
        {
            var filePath = GetCustomViewFilePath();

            var json = JsonConvert.SerializeObject(customGridSettings, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        protected virtual IList<GridSettings> LoadCustomViews()
        {
            IList<GridSettings> customGridSettings;

            var filePath = GetCustomViewFilePath();

            if (!File.Exists(filePath))
            {
                customGridSettings = new List<GridSettings>();
            }
            else
            {
                var json = File.ReadAllText(filePath);

                customGridSettings = JsonConvert.DeserializeObject<List<GridSettings>>(json);
            }

            return customGridSettings;
        }

        protected static string GetCustomViewFilePath()
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string customViewsFolderPath = Path.Combine(assemblyPath, CustomViewFolderName);

            if (!Directory.Exists(customViewsFolderPath))
            {
                Directory.CreateDirectory(customViewsFolderPath);
            }

            string customViewsFilePath = Path.Combine(customViewsFolderPath, CustomViewFileName);

            return customViewsFilePath;
        }

        #endregion

        #region private

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