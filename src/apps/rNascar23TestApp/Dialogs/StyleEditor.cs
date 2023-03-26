using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using rNascar23TestApp.CustomViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace rNascar23TestApp.Dialogs
{
    public partial class StyleEditor : Form
    {
        #region consts

        private const string SaveButtonText = "Update";
        private const string EditButtonText = "Edit";
        private const string CancelButtonText = "Cancel";
        private const string CloseButtonText = "Save All && Close";

        #endregion

        #region enums

        private enum ViewStates
        {
            SingleEdit,
            Loading,
            Ready,
            Viewing,
            Editing,
            Adding,
            Deleting
        }

        #endregion

        #region fields

        private IList<SampleData> _sampleData = new List<SampleData>()
        {
            new SampleData() { Position = 1, Name = "John Smith", Value = 195.23F},
            new SampleData() { Position = 2, Name = "Fred Jones", Value = 194.85F},
            new SampleData() { Position = 3, Name = "Bill Anderson", Value = 193.19F}
        };
        private ViewStates _viewState = ViewStates.Loading;
        private readonly ILogger<StyleEditor> _logger = null;
        private readonly IStyleService _styleService = null;
        private IList<GridStyleSettings> _styles = new List<GridStyleSettings>();
        private EditableGridStyleSettings _editableGridStyleSettings = null;


        private readonly ICustomViewSettingsService _settingsService = null;

        #endregion

        #region properties

        public GridStyleSettings GridStyleSettings { get; set; }

        #endregion

        #region ctor/load

        public StyleEditor(ILogger<StyleEditor> logger,
            IStyleService styleService,
            ICustomViewSettingsService settingsService)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _styleService = styleService ?? throw new ArgumentNullException(nameof(styleService));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        private void GridStyleDialog_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _sampleData.ToList();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (GridStyleSettings != null)
            {
                DisplaySelectedStyle();

                SetViewState(ViewStates.SingleEdit);
            }
            else
            {
                SetViewState(ViewStates.Loading);

                _styles = LoadStyles();

                PopulateStylesList(_styles);

                SetViewState(ViewStates.Ready);
            }
        }

        #endregion

        #region private [ view state ]

        private void SetViewState(ViewStates newViewState)
        {
            if (_viewState == ViewStates.SingleEdit)
                return;

            switch (newViewState)
            {
                case ViewStates.SingleEdit:
                    SetSingleEditViewState();
                    break;
                case ViewStates.Loading:
                    SetLoadingViewState();
                    break;
                case ViewStates.Ready:
                    SetReadyViewState();
                    break;
                case ViewStates.Viewing:
                    SetViewingViewState();
                    break;
                case ViewStates.Editing:
                    SetEditViewState();
                    break;
                case ViewStates.Adding:
                    SetEditViewState();
                    break;
                case ViewStates.Deleting:
                    break;
                default:
                    break;
            }

            _viewState = newViewState;
        }

        private void SetSingleEditViewState()
        {
            pnlSelection.Visible = false;
            dataGridView1.Enabled = true;
            propertyGrid1.Enabled = true;

            btnEditSave.Visible = true;
            btnEditSave.Text = SaveButtonText;
            btnEditSave.Enabled = true;

            btnNewStyle.Visible = false;
            btnCopyStyle.Visible = false;
            btnDeleteStyle.Visible = false;

            btnCancelClose.Visible = true;
            btnCancelClose.Text = CancelButtonText;
            btnCancelClose.Enabled = true;

            btnDiscardAndClose.Visible = true;
            btnDiscardAndClose.Enabled = true;
        }

        private void SetLoadingViewState()
        {
            pnlSelection.Visible = true;
            pnlSelection.Enabled = false;
            dataGridView1.Enabled = false;
            propertyGrid1.Enabled = false;

            btnEditSave.Visible = true;
            btnEditSave.Text = EditButtonText;
            btnEditSave.Enabled = false;

            btnNewStyle.Visible = true;
            btnNewStyle.Enabled = false;

            btnCopyStyle.Visible = true;
            btnCopyStyle.Enabled = false;

            btnDeleteStyle.Visible = true;
            btnDeleteStyle.Enabled = false;

            btnCancelClose.Visible = true;
            btnCancelClose.Text = CloseButtonText;
            btnDeleteStyle.Enabled = true;

            btnDiscardAndClose.Visible = true;
            btnDiscardAndClose.Enabled = false;
        }

        private void SetReadyViewState()
        {
            pnlSelection.Visible = true;
            pnlSelection.Enabled = true;
            dataGridView1.Enabled = false;
            propertyGrid1.Enabled = false;

            btnEditSave.Visible = true;
            btnEditSave.Text = EditButtonText;
            btnEditSave.Enabled = false;

            btnNewStyle.Visible = true;
            btnNewStyle.Enabled = true;

            btnCopyStyle.Visible = true;
            btnCopyStyle.Enabled = false;

            btnDeleteStyle.Visible = true;
            btnDeleteStyle.Enabled = false;

            btnCancelClose.Visible = true;
            btnCancelClose.Text = CloseButtonText;
            btnCancelClose.Enabled = true;

            btnDiscardAndClose.Visible = true;
            btnDiscardAndClose.Enabled = true;
        }

        private void SetEditViewState()
        {
            pnlSelection.Visible = true;
            pnlSelection.Enabled = false;
            dataGridView1.Enabled = true;
            propertyGrid1.Enabled = true;

            btnEditSave.Visible = true;
            btnEditSave.Text = SaveButtonText;
            btnEditSave.Enabled = true;

            btnNewStyle.Visible = true;
            btnNewStyle.Enabled = false;

            btnCopyStyle.Visible = true;
            btnCopyStyle.Enabled = false;

            btnDeleteStyle.Visible = true;
            btnDeleteStyle.Enabled = false;

            btnCancelClose.Visible = true;
            btnCancelClose.Text = CancelButtonText;
            btnCancelClose.Enabled = true;

            btnDiscardAndClose.Visible = true;
            btnDiscardAndClose.Enabled = false;
        }

        private void SetViewingViewState()
        {
            pnlSelection.Visible = true;
            pnlSelection.Enabled = true;
            dataGridView1.Enabled = false;
            propertyGrid1.Enabled = false;

            btnEditSave.Visible = true;
            btnEditSave.Text = EditButtonText;
            btnEditSave.Enabled = true;

            btnNewStyle.Visible = true;
            btnNewStyle.Enabled = true;

            btnCopyStyle.Visible = true;
            btnCopyStyle.Enabled = true;

            btnDeleteStyle.Visible = true;
            btnDeleteStyle.Enabled = true;

            btnCancelClose.Visible = true;
            btnCancelClose.Text = CloseButtonText;
            btnDeleteStyle.Enabled = true;

            btnDiscardAndClose.Visible = true;
            btnDiscardAndClose.Enabled = true;
        }

        #endregion

        #region private [ property grid ]

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            UpdateUserSettings();

            GridStyleHelper.ApplyGridStyleSettings(dataGridView1, GridStyleSettings);

            dataGridView1.Refresh();
        }

        public void UpdateUserSettings()
        {
            GridStyleSettings = new GridStyleSettings()
            {
                Name = _editableGridStyleSettings.Name,
                GridStyle = new GridStyle()
                {
                    BackColor = _editableGridStyleSettings.GridStyle.BackColor.ToArgb(),
                    LineColor = _editableGridStyleSettings.GridStyle.LineColor.ToArgb(),
                },
                HeaderStyle = new GridRowStyle()
                {
                    ForeColor = _editableGridStyleSettings.HeaderStyle.ForeColor.ToArgb(),
                    BackColor = _editableGridStyleSettings.HeaderStyle.BackColor.ToArgb(),
                    Font = new GridRowStyle.GridFont(
                         _editableGridStyleSettings.HeaderStyle.Font.Name,
                         (int)_editableGridStyleSettings.HeaderStyle.Font.Size,
                         (int)_editableGridStyleSettings.HeaderStyle.Font.Style)
                },
                RowStyle = new GridRowStyle()
                {
                    ForeColor = _editableGridStyleSettings.RowStyle.ForeColor.ToArgb(),
                    BackColor = _editableGridStyleSettings.RowStyle.BackColor.ToArgb(),
                    Font = new GridRowStyle.GridFont(
                         _editableGridStyleSettings.RowStyle.Font.Name,
                         (int)_editableGridStyleSettings.RowStyle.Font.Size,
                         (int)_editableGridStyleSettings.RowStyle.Font.Style)
                },
                AlternatingRowStyle = new GridRowStyle()
                {
                    ForeColor = _editableGridStyleSettings.AlternatingRowStyle.ForeColor.ToArgb(),
                    BackColor = _editableGridStyleSettings.AlternatingRowStyle.BackColor.ToArgb(),
                    Font = new GridRowStyle.GridFont(
                         _editableGridStyleSettings.AlternatingRowStyle.Font.Name,
                         (int)_editableGridStyleSettings.AlternatingRowStyle.Font.Size,
                         (int)_editableGridStyleSettings.AlternatingRowStyle.Font.Style)
                },
                SelectedRowStyle = new GridRowStyle()
                {
                    ForeColor = _editableGridStyleSettings.SelectedRowStyle.ForeColor.ToArgb(),
                    BackColor = _editableGridStyleSettings.SelectedRowStyle.BackColor.ToArgb(),
                    Font = null
                }
            };
        }

        public void BuildEditableSettings()
        {
            if (GridStyleSettings == null)
                return;

            _editableGridStyleSettings = new EditableGridStyleSettings()
            {
                Name = GridStyleSettings.Name,
                GridStyle = new EditableGridStyle()
                {
                    BackColor = Color.FromArgb(GridStyleSettings.GridStyle.BackColor.Value),
                    LineColor = Color.FromArgb(GridStyleSettings.GridStyle.LineColor.Value),
                },
                HeaderStyle = new EditableGridHeaderStyle()
                {
                    ForeColor = Color.FromArgb(GridStyleSettings.HeaderStyle.ForeColor.Value),
                    BackColor = Color.FromArgb(GridStyleSettings.HeaderStyle.BackColor.Value),
                    Font = new Font(
                        GridStyleSettings.HeaderStyle.Font.FontName,
                        (float)GridStyleSettings.HeaderStyle.Font.FontSize,
                        (FontStyle)GridStyleSettings.HeaderStyle.Font.FontStyle)
                },
                RowStyle = new EditableGridRowStyle()
                {
                    ForeColor = Color.FromArgb(GridStyleSettings.RowStyle.ForeColor.Value),
                    BackColor = Color.FromArgb(GridStyleSettings.RowStyle.BackColor.Value),
                    Font = new Font(
                        GridStyleSettings.RowStyle.Font.FontName,
                        (float)GridStyleSettings.RowStyle.Font.FontSize,
                        (FontStyle)GridStyleSettings.RowStyle.Font.FontStyle)
                },
                AlternatingRowStyle = new EditableGridRowStyle()
                {
                    ForeColor = Color.FromArgb(GridStyleSettings.AlternatingRowStyle.ForeColor.Value),
                    BackColor = Color.FromArgb(GridStyleSettings.AlternatingRowStyle.BackColor.Value),
                    Font = new Font(
                        GridStyleSettings.AlternatingRowStyle.Font.FontName,
                        (float)GridStyleSettings.AlternatingRowStyle.Font.FontSize,
                        (FontStyle)GridStyleSettings.AlternatingRowStyle.Font.FontStyle)
                },
                SelectedRowStyle = new EditableSelectedGridRowStyle()
                {
                    ForeColor = Color.FromArgb(GridStyleSettings.SelectedRowStyle.ForeColor.Value),
                    BackColor = Color.FromArgb(GridStyleSettings.SelectedRowStyle.BackColor.Value)
                },
            };
        }

        #endregion

        #region private [ ui logic ]

        private IList<GridStyleSettings> LoadStyles()
        {
            List<GridStyleSettings> styles = new List<GridStyleSettings>();

            styles.AddRange(_styleService.GetStyles());

            return styles;
        }

        private void SaveStyles()
        {
            try
            {
                _styleService.SaveStyles(_styles);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void PopulateStylesList(IList<GridStyleSettings> styles, string selectedStyle = "")
        {
            cboStyles.DataSource = null;

            cboStyles.DisplayMember = "Name";
            cboStyles.ValueMember = "Name";

            cboStyles.DataSource = styles.OrderBy(s => s.Name).ToList();

            cboStyles.SelectedIndex = -1;

            if (!string.IsNullOrEmpty(selectedStyle))
            {
                cboStyles.SelectedValue = selectedStyle;
            }
        }

        private void cboStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_viewState == ViewStates.Loading)
                    return;

                if (cboStyles.SelectedItem != null)
                {
                    GridStyleSettings = cboStyles.SelectedItem as GridStyleSettings;

                    Console.WriteLine($"Selected {GridStyleSettings.Name}");

                    DisplaySelectedStyle();

                    SetViewState(ViewStates.Viewing);
                }
                else
                {
                    Console.WriteLine($"Selected -NONE-");

                    ClearStyle();

                    SetViewState(ViewStates.Ready);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void DisplaySelectedStyle()
        {
            BuildEditableSettings();

            propertyGrid1.SelectedObject = _editableGridStyleSettings;

            GridStyleHelper.ApplyGridStyleSettings(dataGridView1, GridStyleSettings);
        }

        private void ClearStyle()
        {
            this.GridStyleSettings = new GridStyleSettings();

            BuildEditableSettings();

            propertyGrid1.SelectedObject = _editableGridStyleSettings;

            GridStyleHelper.ApplyGridStyleSettings(dataGridView1, GridStyleSettings);

            this.GridStyleSettings = null;

            propertyGrid1.SelectedObject = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!StyleIsValidForSave())
                    return;

                if (_viewState == ViewStates.SingleEdit)
                {
                    SaveSingleEdit();
                }
                else if (btnEditSave.Text == EditButtonText)
                {
                    // Should already be displaying the selected style,
                    // just need to set the form to Edit mode
                    SetViewState(ViewStates.Editing);
                }
                else if (btnEditSave.Text == SaveButtonText)
                {
                    switch (_viewState)
                    {
                        case ViewStates.Editing:
                            SaveEditedStyle();
                            break;
                        case ViewStates.Adding:
                            SaveAddedStyle();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private bool StyleIsValidForSave()
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(GridStyleSettings.Name))
            {
                MessageBox.Show("Name cannot be blank", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValid = false;
            }

            if (_viewState == ViewStates.Adding)
            {
                if (_styles.Any(s => s.Name == GridStyleSettings.Name))
                {
                    MessageBox.Show($"Name must be unique. There is already a style named {GridStyleSettings.Name}", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isValid = false;
                }
            }
            else if (_viewState == ViewStates.Editing)
            {
                var selected = cboStyles.SelectedItem as GridStyleSettings;

                if (selected.Name != GridStyleSettings.Name)
                {
                    // Name has changed
                    if (_styles.Any(s => s.Name == GridStyleSettings.Name))
                    {
                        MessageBox.Show($"Name must be unique. There is already a style named {GridStyleSettings.Name}", "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private void SaveSingleEdit()
        {
            DialogResult = DialogResult.OK;

            this.Close();
        }

        private void SaveEditedStyle()
        {
            var selected = cboStyles.SelectedItem as GridStyleSettings;

            var listItem = _styles.FirstOrDefault(s => s.Name == selected.Name);

            _styles.Remove(listItem);

            _styles.Add(GridStyleSettings);

            PopulateStylesList(_styles, GridStyleSettings.Name);

            SetViewState(ViewStates.Viewing);
        }

        private void SaveAddedStyle()
        {
            _styles.Add(GridStyleSettings);

            PopulateStylesList(_styles, GridStyleSettings.Name);

            SetViewState(ViewStates.Viewing);
        }

        private void btnNewStyle_Click(object sender, EventArgs e)
        {
            try
            {
                OpenNewStyleForEdit();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void OpenNewStyleForEdit()
        {
            GridStyleSettings = new GridStyleSettings()
            {
                Name = $"Style {_styles.Count}"
            };

            DisplaySelectedStyle();

            SetViewState(ViewStates.Adding);
        }

        private void btnCopyStyle_Click(object sender, EventArgs e)
        {
            try
            {
                CopySelectedStyleForEdit();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void CopySelectedStyleForEdit()
        {
            var selectedStyle = cboStyles.SelectedItem as GridStyleSettings;

            var json = JsonConvert.SerializeObject(selectedStyle);

            GridStyleSettings = JsonConvert.DeserializeObject<GridStyleSettings>(json);

            GridStyleSettings.Name = $"Copy of {selectedStyle.Name}";

            DisplaySelectedStyle();

            SetViewState(ViewStates.Adding);
        }

        private void btnDeleteStyle_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteSelectedStyle();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void DeleteSelectedStyle()
        {
            var selected = cboStyles.SelectedItem as GridStyleSettings;

            var promptResponse = MessageBox.Show(this, $"Delete style {selected.Name}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (promptResponse == DialogResult.Yes)
            {
                var listItem = _styles.FirstOrDefault(s => s.Name == selected.Name);

                _styles.Remove(listItem);

                PopulateStylesList(_styles);

                SetViewState(ViewStates.Ready);
            }
        }

        private void btnCancelClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (_viewState == ViewStates.SingleEdit)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                else if (btnCancelClose.Text == CloseButtonText)
                {
                    SaveStyles();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (btnCancelClose.Text == CancelButtonText)
                {
                    switch (_viewState)
                    {
                        case ViewStates.Editing:
                            CancelPendingEdit();
                            break;
                        case ViewStates.Adding:
                            CancelPendingAdd();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void CancelPendingEdit()
        {
            ClearStyle();

            var selectedStyle = cboStyles.SelectedItem as GridStyleSettings;

            var selectedStyleName = selectedStyle.Name;

            PopulateStylesList(_styles, selectedStyleName);

            SetViewState(ViewStates.Viewing);
        }

        private void CancelPendingAdd()
        {
            ClearStyle();

            if (cboStyles.SelectedItem != null)
            {
                var selectedStyle = cboStyles.SelectedItem as GridStyleSettings;

                var selectedStyleName = selectedStyle.Name;

                PopulateStylesList(_styles, selectedStyleName);

                SetViewState(ViewStates.Viewing);
            }
            else
            {
                SetViewState(ViewStates.Ready);
            }
        }

        private void btnDiscardAndClose_Click(object sender, EventArgs e)
        {
            try
            {
                DiscardChangesAndClose();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void DiscardChangesAndClose()
        {
            var promptResponse = MessageBox.Show(this, "Close form without saving any changes?", "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (promptResponse == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        #endregion

        #region private [ exception handlers ]

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

        #region classes

        [CategoryAttribute("Grid Styles")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        internal class EditableGridStyleSettings : ExpandableObjectConverter
        {
            [CategoryAttribute("Style Name"), DisplayName("Style Name"), Description("The unique name for the style")]
            public string Name { get; set; }

            [CategoryAttribute("Grid Style"), DisplayName("Grid Style"), Description("Grid style properties")]
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public EditableGridStyle GridStyle { get; set; }

            [CategoryAttribute("Header Style"), DisplayName("Header Style"), Description("Style properties for the header row of the grid")]
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public EditableGridHeaderStyle HeaderStyle { get; set; }

            [CategoryAttribute("Row Styles"), DisplayName("Primary Row Style"), Description("Style properties for the data rows of the grid")]
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public EditableGridRowStyle RowStyle { get; set; }

            [CategoryAttribute("Row Styles"), DisplayName("Alternating Row Style"), Description("Style properties for the alternating data rows of the grid")]
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public EditableGridRowStyle AlternatingRowStyle { get; set; }

            [CategoryAttribute("Row Styles"), DisplayName("Selected Row Style"), Description("Style properties for the selected data rows of the grid")]
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public EditableSelectedGridRowStyle SelectedRowStyle { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        [CategoryAttribute("Grid Styles")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        internal class EditableGridStyle : ExpandableObjectConverter
        {
            [CategoryAttribute("Grid Styles"), Description("Color of the background behind the grid")]
            public Color BackColor { get; set; }

            [CategoryAttribute("Grid Styles"), DisplayName("Grid Lines Color"), Description("Color of the grid lines")]
            public Color LineColor { get; set; }

            public override string ToString()
            {
                return "Grid Styles";
            }
        }

        [CategoryAttribute("Header Style")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        internal class EditableGridHeaderStyle : ExpandableObjectConverter
        {
            [CategoryAttribute("Header Style"), DisplayName("Header Text Color"), Description("Text Color for the column headers of the grid")]
            public Color ForeColor { get; set; }

            [CategoryAttribute("Header Style"), DisplayName("Header Background Color"), Description("Background Color for the column headers of the grid")]
            public Color BackColor { get; set; }

            [CategoryAttribute("Header Style"), DisplayName("Header Font"), Description("Font for the column headers of the grid")]
            public Font Font { get; set; }

            public override string ToString()
            {
                return "Header Style";
            }
        }

        [CategoryAttribute("Row Styles")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        internal class EditableGridRowStyle : ExpandableObjectConverter
        {
            [CategoryAttribute("Row Styles"), DisplayName("Data Text Color"), Description("Text Color for the data rows of the grid")]
            public Color ForeColor { get; set; }

            [CategoryAttribute("Row Styles"), DisplayName("Data Background Color"), Description("Background Color for the data rows of the grid")]
            public Color BackColor { get; set; }

            [CategoryAttribute("Row Styles"), DisplayName("Data Font"), Description("Font for the data rows of the grid")]
            public Font Font { get; set; }

            public override string ToString()
            {
                return "Row Styles";
            }
        }

        [CategoryAttribute("Row Styles")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        internal class EditableSelectedGridRowStyle : ExpandableObjectConverter
        {
            [CategoryAttribute("Row Styles"), DisplayName("Selected Text Color"), Description("Text Color for the selected data rows of the grid")]
            public Color ForeColor { get; set; }

            [CategoryAttribute("Row Styles"), DisplayName("Selected Background Color"), Description("Background Color for the selected data rows of the grid")]
            public Color BackColor { get; set; }

            public override string ToString()
            {
                return "Row Styles";
            }
        }

        internal class SampleData
        {
            public int Position { get; set; }
            public string Name { get; set; }
            public float Value { get; set; }
        }

        #endregion
    }
}
