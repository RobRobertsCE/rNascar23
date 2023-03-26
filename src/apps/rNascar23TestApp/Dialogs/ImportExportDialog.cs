using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.Screens;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace rNascar23TestApp.Dialogs
{
    public partial class ImportExportDialog : Form
    {
        #region enums

        public enum DataTypes
        {
            CustomViews,
            Screens,
            Styles
        }

        public enum ActionTypes
        {
            Import,
            Export
        }

        #endregion

        #region fields

        private readonly ILogger<ImportExportDialog> _logger = null;
        private readonly ICustomViewSettingsService _customViewService = null;
        private readonly IScreenService _screenService = null;
        private readonly IStyleService _styleService = null;
        private IList<INamedItem> _sourceList = null;
        private IList<INamedItem> _targetList = null;
        private bool _loading = false;
        private bool _sourceMatchesTarget = false;

        #endregion

        #region properties

        public DataTypes DataType { get; set; } = DataTypes.CustomViews;
        public ActionTypes ActionType { get; set; } = ActionTypes.Import;

        public string ImportFile { get; set; }
        public string ExportFile { get; set; }

        #endregion

        #region ctor/load
        public ImportExportDialog(
            ILogger<ImportExportDialog> logger,
            ICustomViewSettingsService customViewService,
            IScreenService screenService,
            IStyleService styleService)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customViewService = customViewService ?? throw new ArgumentNullException(nameof(customViewService));
            _screenService = screenService ?? throw new ArgumentNullException(nameof(screenService));
            _styleService = styleService ?? throw new ArgumentNullException(nameof(styleService));
        }

        private void ImportExportDialog_Load(object sender, EventArgs e)
        {
            try
            {
                _loading = true;

                SetFormTitle();

                SetImportExportState();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                _loading = false;
            }
        }

        #endregion

        #region private

        private void SetFormTitle()
        {
            var dataTypeName = Regex.Replace(DataType.ToString(), "(\\B[A-Z])", " $1");

            this.Text = $"{ActionType} {dataTypeName}";
        }

        private void SetImportExportState()
        {
            if (ActionType == ActionTypes.Import)
            {
                grpOverwriteMethod.Visible = true;
                btnImportExportSelected.Text = "Import Selected";

                lstTarget.Enabled = false;
                btnRemoveSelected.Visible = false;

                if (String.IsNullOrEmpty(ImportFile))
                    throw new ArgumentException("Import File value cannot be blank");

                if (!File.Exists(ImportFile))
                    throw new ArgumentException("Import File does not exist");

                _targetList = LoadTargetList();

                DisplayTargetList(_targetList);

                _sourceList = LoadSource(ImportFile);

                DisplaySourceList(_sourceList);
            }
            else if (ActionType == ActionTypes.Export)
            {
                grpOverwriteMethod.Visible = false;
                btnImportExportSelected.Text = "Export Selected";

                lstTarget.Enabled = true;
                lstTarget.SelectedIndexChanged += LstTarget_SelectedIndexChanged;
                btnRemoveSelected.Visible = true;
                btnAccept.Enabled = false;

                if (String.IsNullOrEmpty(ExportFile))
                    throw new ArgumentException("Export File value cannot be blank");

                _sourceList = LoadTargetList();

                DisplaySourceList(_sourceList);

                _targetList = new List<INamedItem>();
            }
        }

        private void LstTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_loading)
                    return;

                TargetItemSelected();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private IList<INamedItem> LoadSource(string sourceFile)
        {
            IList<INamedItem> items = null;

            var json = File.ReadAllText(ImportFile);

            switch (DataType)
            {
                case DataTypes.CustomViews:
                    var incomingCustomViews = JsonConvert.DeserializeObject<IList<GridSettings>>(json);

                    items = incomingCustomViews.Cast<INamedItem>().ToList();

                    break;
                case DataTypes.Screens:
                    var incomingScreens = JsonConvert.DeserializeObject<IList<ScreenDefinition>>(json);

                    items = incomingScreens.Cast<INamedItem>().ToList();

                    break;
                case DataTypes.Styles:
                    var incomingStyles = JsonConvert.DeserializeObject<IList<GridStyleSettings>>(json);

                    items = incomingStyles.Cast<INamedItem>().ToList();

                    break;
                default:
                    break;
            }

            return items;
        }

        private void DisplaySourceList(IList<INamedItem> items)
        {
            lstSource.DataSource = null;

            lstSource.DisplayMember = "Name";
            lstSource.ValueMember = "Name";

            lstSource.DataSource = items.OrderBy(i => i.Name).ToList();

            lstSource.SelectedIndex = -1;
        }

        private IList<INamedItem> LoadTargetList()
        {
            IList<INamedItem> items = null;

            switch (DataType)
            {
                case DataTypes.CustomViews:
                    items = _customViewService.GetCustomViewSettings().Cast<INamedItem>().ToList();

                    break;
                case DataTypes.Screens:
                    items = _screenService.GetScreenDefinitions().Cast<INamedItem>().ToList();

                    break;
                case DataTypes.Styles:
                    items = _styleService.GetStyles().Cast<INamedItem>().ToList(); ;

                    break;
                default:
                    break;
            }

            return items;
        }

        private void DisplayTargetList(IList<INamedItem> items)
        {
            lstTarget.DataSource = null;

            lstTarget.DisplayMember = "Name";
            lstTarget.ValueMember = "Name";

            lstTarget.DataSource = items.OrderBy(i => i.Name).ToList();

            lstTarget.SelectedIndex = -1;

            if (ActionType == ActionTypes.Export)
                btnAccept.Enabled = (items.Count > 0);
        }

        private void lstSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_loading)
                    return;

                SourceItemSelected();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void SourceItemSelected()
        {
            btnImportExportSelected.Enabled = lstSource.SelectedItems.Count > 0;

            foreach (var sourceItem in lstSource.SelectedItems)
            {
                var sourceNamedItem = sourceItem as INamedItem;

                _sourceMatchesTarget = _targetList.Any(t => t.Name == sourceNamedItem.Name);

                if (_sourceMatchesTarget)
                    break;
            }

            grpOverwriteMethod.Enabled = _sourceMatchesTarget;
        }

        private void TargetItemSelected()
        {
            btnRemoveSelected.Enabled = lstTarget.SelectedItems.Count > 0;
        }

        private void btnImportSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActionType == ActionTypes.Import)
                    ImportSelectedItems();
                else if (ActionType == ActionTypes.Export)
                    ExportSelectedItems();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void ImportSelectedItems()
        {
            IList<INamedItem> selectedItems = new List<INamedItem>();

            foreach (var sourceItem in lstSource.SelectedItems)
            {
                var sourceNamedItem = sourceItem as INamedItem;

                selectedItems.Add(sourceNamedItem);
            }

            foreach (var sourceNamedItem in selectedItems)
            {
                _sourceMatchesTarget = _targetList.Any(t => t.Name == sourceNamedItem.Name);

                if (_sourceMatchesTarget)
                {
                    if (rbOverwrite.Checked)
                    {
                        ImportSourceToTargetList(sourceNamedItem.Name, sourceNamedItem.Name, true);
                    }
                    else
                    {
                        var newName = $"{sourceNamedItem.Name}(1)";

                        ImportSourceToTargetList(sourceNamedItem.Name, newName, false);
                    }
                }
                else
                {
                    ImportSourceToTargetList(sourceNamedItem.Name, sourceNamedItem.Name, false);
                }
            }
        }

        private void ExportSelectedItems()
        {
            IList<INamedItem> selectedItems = new List<INamedItem>();

            foreach (var sourceItem in lstSource.SelectedItems)
            {
                var sourceNamedItem = sourceItem as INamedItem;

                selectedItems.Add(sourceNamedItem);
            }

            foreach (var sourceNamedItem in selectedItems)
            {
                ExportSourceToTargetList(sourceNamedItem.Name);
            }
        }

        private void ImportSourceToTargetList(string sourceName, string targetName, bool overwrite = false)
        {
            var itemToImport = _sourceList.FirstOrDefault(s => s.Name == sourceName);

            if (overwrite)
            {
                var itemToOverwrite = _targetList.FirstOrDefault(s => s.Name == targetName);

                _targetList.Remove(itemToOverwrite);
            }

            if (sourceName != targetName)
            {
                itemToImport.Name = targetName;
            }

            _targetList.Add(itemToImport);

            _sourceList.Remove(itemToImport);

            DisplayTargetList(_targetList);

            DisplaySourceList(_sourceList);
        }

        private void ExportSourceToTargetList(string sourceName)
        {
            var itemToImport = _sourceList.FirstOrDefault(s => s.Name == sourceName);

            _targetList.Add(itemToImport);

            _sourceList.Remove(itemToImport);

            DisplayTargetList(_targetList);

            DisplaySourceList(_sourceList);
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveSelectedItems();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void RemoveSelectedItems()
        {
            IList<INamedItem> selectedItems = new List<INamedItem>();

            foreach (var selectedItem in lstTarget.SelectedItems)
            {
                var targetNamedItem = selectedItem as INamedItem;

                selectedItems.Add(targetNamedItem);
            }

            foreach (var selectedItem in selectedItems)
            {
                RemoveItemFromTargetList(selectedItem.Name);
            }
        }

        private void RemoveItemFromTargetList(string sourceName)
        {
            var itemToRemove = _targetList.FirstOrDefault(s => s.Name == sourceName);

            _targetList.Remove(itemToRemove);

            _sourceList.Add(itemToRemove);

            DisplayTargetList(_targetList);

            DisplaySourceList(_sourceList);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (ActionType == ActionTypes.Import)
                    SaveImportChanges();
                else if (ActionType == ActionTypes.Export)
                    SaveExportChanges();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void SaveImportChanges()
        {
            switch (DataType)
            {
                case DataTypes.CustomViews:
                    _customViewService.SaveCustomViewSettings(_targetList.Cast<GridSettings>().ToList());

                    break;
                case DataTypes.Screens:
                    _screenService.SaveScreenDefinitions(_targetList.Cast<ScreenDefinition>().ToList());

                    break;
                case DataTypes.Styles:
                    _styleService.SaveStyles(_targetList.Cast<GridStyleSettings>().ToList());

                    break;
                default:
                    break;
            }

            MessageBox.Show(this, "Items have been imported", "Items Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveExportChanges()
        {
            string json = string.Empty;

            switch (DataType)
            {
                case DataTypes.CustomViews:
                    json = JsonConvert.SerializeObject(_targetList.Cast<GridSettings>().ToList());

                    break;
                case DataTypes.Screens:
                    json = JsonConvert.SerializeObject(_targetList.Cast<ScreenDefinition>().ToList());

                    break;
                case DataTypes.Styles:
                    json = JsonConvert.SerializeObject(_targetList.Cast<GridStyleSettings>().ToList());

                    break;
                default:
                    break;
            }

            File.WriteAllText(ExportFile, json);

            var fileName = Path.GetFileName(ExportFile);

            MessageBox.Show(this, $"Items have been saved to {fileName}", "Items Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region [ exception handling ]

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
            var messageText = String.IsNullOrEmpty(message) ? ex.Message : $"{message}: {ex.Message}";

            DisplayMessage(messageText, "Error");

            if (logMessage)
            {
                string errorMessage = String.IsNullOrEmpty(message) ? ex.Message : message;

                _logger.LogError(ex, errorMessage);
            }
        }
        private void DisplayMessage(string message, string caption = "")
        {
            MessageBox.Show(this, message, caption, MessageBoxButtons.OK);
        }

        #endregion
    }
}
