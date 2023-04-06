using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.CustomViews
{
    internal class CustomGridViewFactory : ICustomGridViewFactory
    {
        #region fields

        private readonly ILogger<CustomGridViewFactory> _logger = null;
        private readonly IStyleService _styleService = null;
        private IList<GridStyleSettings> _styles = new List<GridStyleSettings>();

        #endregion

        #region ctor

        public CustomGridViewFactory(ILogger<CustomGridViewFactory> logger,
            IStyleService styleService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _styleService = styleService ?? throw new ArgumentNullException(nameof(styleService));

            _styles = _styleService.GetStyles();
        }

        #endregion

        #region public

        public IList<GridView> GetCustomGridViews(IList<GridSettings> customGridSettings)
        {
            IList<GridView> customGridViews = new List<GridView>();

            try
            {
                foreach (GridSettings customGridSetting in customGridSettings.Where(g => g.Enabled))
                {
                    var customGridView = GetCustomGridView(customGridSetting);

                    customGridViews.Add(customGridView);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error building custom grid views");
            }

            return customGridViews;
        }

        public GridView GetCustomGridView(GridSettings customGridSettings)
        {
            GridView customGridView = null;

            try
            {
                customGridView = new GridView();

                customGridView.CustomGridName = customGridSettings.Name;
                customGridView.Description = customGridSettings.Description;
                customGridView.TitleLabel.Text = customGridSettings.Title;
                customGridView.TitleLabel.ForeColor = customGridSettings.TitleForeColor;
                customGridView.TitleLabel.BackColor = customGridSettings.TitleBackColor;
                if (customGridSettings.GridWidth.HasValue)
                    customGridView.DataGridView.Width = customGridSettings.GridWidth.Value;
                if (customGridSettings.GridHeight.HasValue)
                    customGridView.DataGridView.Height = customGridSettings.GridHeight.Value;
                if (customGridSettings.ViewWidth.HasValue)
                    customGridView.Width = customGridSettings.ViewWidth.Value;
                if (customGridSettings.ViewHeight.HasValue)
                    customGridView.Height = customGridSettings.ViewHeight.Value;
                customGridView.Grid.RowHeadersVisible = !customGridSettings.HideRowSelector;

                foreach (var columnSettings in customGridSettings.Columns.OrderBy(c => c.Index))
                {
                    DataGridViewColumn dgvc = new DataGridViewTextBoxColumn();

                    dgvc.DisplayIndex = columnSettings.DisplayIndex;
                    dgvc.DataPropertyName = columnSettings.DataProperty;
                    dgvc.Visible = columnSettings.Visible;
                    if (dgvc.Visible == true)
                    {
                        dgvc.Width = columnSettings.Width;
                    }
                    dgvc.HeaderText = columnSettings.HeaderTitle;

                    customGridView.Grid.Columns.Add(dgvc);
                }

                customGridView.Settings = customGridSettings;

                //var style = _styles.FirstOrDefault(s => s.Name == customGridView.Settings.Style);

                //if (style != null)
                //{
                //    GridStyleHelper.ApplyGridStyleSettings(customGridView.Grid, style);
                //}
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, "Error building custom grid view");
            }

            return customGridView;
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
