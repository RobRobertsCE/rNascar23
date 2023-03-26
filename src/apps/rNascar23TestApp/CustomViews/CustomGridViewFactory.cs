using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23TestApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23TestApp.CustomViews
{
    internal class CustomGridViewFactory
    {
        #region fields

        private readonly ILogger<CustomGridViewFactory> _logger = null;

        #endregion

        #region ctor

        public CustomGridViewFactory(ILogger<CustomGridViewFactory> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                customGridView.Width = customGridSettings.GridWidth;
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
