using Microsoft.Extensions.Logging;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.Screens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace rNascar23TestApp.Dialogs
{
    public partial class ScreenEditor : Form
    {
        #region [ enums ]

        private enum ViewStates
        {
            Loading,
            Ready,
            Viewing,
            Editing,
            Adding,
            Deleting,
            Saving
        }

        #endregion

        #region [ fields ]

        private Panel _selectedGridViewPanelControl = null;

        private Panel _selectedScreenPanelControl = null;

        private ScreenDefinition _selectedScreenDefinition = null;

        private ScreenPanelGridDefinition _selectedScreenPanelGridDefinition = null;

        private bool _dragging = false;
        private ViewStates _viewState = ViewStates.Loading;

        private IList<ScreenDefinition> _screenDefinitions = new List<ScreenDefinition>();
        private IDictionary<string, ScreenPanelGridDefinition> _screenPanelGridDefinitions = new Dictionary<string, ScreenPanelGridDefinition>();

        private readonly ICustomViewSettingsService _settingsService = null;
        private readonly IScreenService _screenService = null;
        private readonly IStyleService _styleService = null;
        private IList<GridStyleSettings> _styles = new List<GridStyleSettings>();

        private readonly ILogger<ScreenEditor> _logger = null;

        private IList<SampleData> _sampleData = new List<SampleData>()
        {
            new SampleData() { Position = 1, Name = "John Smith", Value = 195.23F},
            new SampleData() { Position = 2, Name = "Fred Jones", Value = 194.85F},
            new SampleData() { Position = 3, Name = "Bill Anderson", Value = 193.19F}
        };
        #endregion

        #region [ properties ]

        private bool IsEditing
        {
            get
            {
                return _viewState == ViewStates.Editing || _viewState == ViewStates.Adding;
            }
        }

        private bool IsScreenDefinitionSelected
        {
            get
            {
                return _selectedScreenDefinition != null;
            }
        }

        private bool IsPanelControlSelected
        {
            get
            {
                return _selectedScreenPanelControl != null;
            }
        }

        private bool IsGridViewControlSelected
        {
            get
            {
                return _selectedGridViewPanelControl != null;
            }
        }

        #endregion

        #region [ ctor/load ]

        public ScreenEditor(
            ILogger<ScreenEditor> logger,
            ICustomViewSettingsService settingsService,
            IScreenService screenService,
            IStyleService styleService)
        {
            InitializeComponent();

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _screenService = screenService ?? throw new ArgumentNullException(nameof(screenService));
            _styleService = styleService ?? throw new ArgumentNullException(nameof(styleService));
        }

        private void GridStyleEditor_Load(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewStates.Loading);

                var grids = BuildGridViewsList();

                PopulateGridViewsSelectionControl(grids);

                _styles = LoadStylesList();

                PopulateStylesSelectionControls(_styles);

                _screenDefinitions = LoadScreenDefinitionsFromFile();

                if (_screenDefinitions == null)
                    _screenDefinitions = BuildDefaultScreenDefinitionsList();

                PopulateScreenDefinitionsSelectionControl(_screenDefinitions);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                SetViewState(ViewStates.Ready);
            }
        }

        #endregion

        #region [ view states ]

        private void SetViewState(ViewStates newViewState)
        {
            switch (newViewState)
            {
                case ViewStates.Loading:
                    // loading data, populating selection controls
                    SetUiStateLoading();
                    break;
                case ViewStates.Ready:
                    // selection controls populated, nothing selected
                    SetUiStateReady();
                    break;
                case ViewStates.Viewing:
                    // screen definition selected
                    SetUiStateViewing();
                    break;
                case ViewStates.Editing:
                    // editing an existing screen definition
                    SetUiStateEditing();
                    break;
                case ViewStates.Adding:
                    // editing a new screen definition
                    SetUiStateEditing();
                    break;
                case ViewStates.Deleting:
                    // removing an existing screen definition
                    break;
                default:
                    break;
            }

            _viewState = newViewState;

            SetToolbarButtonStates();
        }

        private void SetUiStateLoading()
        {
            ClearSelectedScreenDefinition();

            lstScreens.Enabled = false;
            lstGridViews.Enabled = false;
            pnlDialogButtons.Enabled = false;
            pnlScreenMock.Enabled = false;
        }
        private void SetUiStateReady()
        {
            // No screen selected
            lstScreens.Enabled = true;
            pnlDialogButtons.Enabled = true;
            pnlScreenMock.Enabled = false;
        }
        private void SetUiStateViewing()
        {
            // A screen has been selected
            lstScreens.Enabled = true;
            lstGridViews.Enabled = false;
            pnlDialogButtons.Enabled = true;
            pnlScreenMock.Enabled = false;
        }
        private void SetUiStateEditing()
        {
            lstScreens.Enabled = false;
            lstGridViews.Enabled = true;
            pnlDialogButtons.Enabled = false;
            pnlScreenMock.Enabled = true;
        }

        private void SetToolbarButtonStates()
        {
            SetScreenDefinitionToolbarButtonStates();
            SetPanelToolbarButtonStates();
            SetGridToolbarButtonStates(_selectedScreenPanelGridDefinition);

        }
        private void SetScreenDefinitionToolbarButtonStates()
        {
            tsbSaveScreen.Enabled = IsEditing;
            tsbEditScreen.Enabled = !IsEditing && IsScreenDefinitionSelected;
            tsbAddScreen.Enabled = !IsEditing;
            tsbCopyScreen.Enabled = !IsEditing && IsScreenDefinitionSelected;
            tsbRemoveScreen.Enabled = !IsEditing && IsScreenDefinitionSelected;
            tsbCancelScreenEdit.Enabled = IsEditing;
            tsbEditScreenName.Enabled = IsEditing;

            tsbShowHeader.Enabled = IsEditing;
            tsbShowFlagStatus.Enabled = IsEditing;
            tsbShowLapFlagBar.Enabled = IsEditing;

            txtScreenName.Enabled = IsEditing;

            lstScreens.Enabled = !IsEditing;
            lstGridViews.Enabled = IsEditing;

            btnScreenUp.Enabled = IsScreenDefinitionSelected && _selectedScreenDefinition.DisplayIndex > 0;
            btnScreenDown.Enabled = IsScreenDefinitionSelected && _selectedScreenDefinition.DisplayIndex < lstScreens.Items.Count - 1;
        }
        private void SetPanelToolbarButtonStates()
        {
            tsbBringToFront.Enabled = IsEditing && IsPanelControlSelected;
            tsbSendToBack.Enabled = IsEditing && IsPanelControlSelected;
            tysbDropDownDock.Enabled = IsEditing && IsPanelControlSelected;

            tsbAddPanel.Enabled = IsEditing;
            tsbRemovePanel.Enabled = IsEditing && IsPanelControlSelected;
        }
        private void SetGridToolbarButtonStates(ScreenPanelGridDefinition screenPanelGrid)
        {
            tsbMoveUp.Enabled = false;
            tsbMoveUp.Enabled = false;

            tsbMoveLeft.Enabled = false;
            tsbMoveRight.Enabled = false;

            tsbDeleteGrid.Enabled = false;

            cboGridStyles.Enabled = IsEditing;

            if (screenPanelGrid == null)
                return;

            var panelName = $"{screenPanelGrid.ScreenName}_{screenPanelGrid.PanelName}";

            var screenPanel = pnlScreenMock.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == panelName);

            var panelControlCount = screenPanel.Controls.OfType<Panel>().Count();

            var gridPanel = screenPanel.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == screenPanelGrid.Name);

            if (gridPanel.Dock == DockStyle.Top)
            {
                tsbMoveDown.Enabled = screenPanelGrid.DisplayIndex < (panelControlCount - 1);
                tsbMoveUp.Enabled = screenPanelGrid.DisplayIndex > 0;
            }
            if (gridPanel.Dock == DockStyle.Bottom)
            {
                tsbMoveUp.Enabled = screenPanelGrid.DisplayIndex < (panelControlCount - 1);
                tsbMoveDown.Enabled = screenPanelGrid.DisplayIndex > 0;
            }
            if (gridPanel.Dock == DockStyle.Left)
            {
                tsbMoveLeft.Enabled = screenPanelGrid.DisplayIndex > 0;
                tsbMoveRight.Enabled = screenPanelGrid.DisplayIndex < (panelControlCount - 1);
            }
            if (gridPanel.Dock == DockStyle.Right)
            {
                tsbMoveRight.Enabled = screenPanelGrid.DisplayIndex > 0;
                tsbMoveLeft.Enabled = screenPanelGrid.DisplayIndex < (panelControlCount - 1);
            }

            tsbDeleteGrid.Enabled = (_selectedScreenPanelGridDefinition != null);
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

        #region [ drag/drop ]

        private void lstGridViews_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (lstGridViews.SelectedItems.Count > 0)
                    {
                        var lst = new List<string>();
                        foreach (GridViewDefinition item in lstGridViews.SelectedItems)
                        {
                            lst.Add(item.Name);
                            _dragging = true;
                        }
                        pnlMain.DoDragDrop(lst, DragDropEffects.Copy | DragDropEffects.Move);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void lstGridViews_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                if (!_dragging) return;

                if (e.Data.GetDataPresent(typeof(List<string>)))
                {
                    e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void screenPanel_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(List<string>)))
                {
                    e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void screenPanel_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(List<string>)))
                {
                    e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
                    List<string> lst = e.Data.GetData(typeof(List<string>)) as List<string>;
                    foreach (var item in lst)
                    {
                        Panel targetPanel = (Panel)sender;
                        Console.WriteLine($"Dropped {item} on panel {targetPanel.Name}");
                        DropGridOnPanel(targetPanel, item);
                    }
                    _dragging = false;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void DropGridOnPanel(Panel targetPanel, string gridName)
        {
            var addedGridPanel = AddGridToScreenPanel(targetPanel, gridName);

            var index = targetPanel.Controls.OfType<Panel>().Count() - 1;

            ScreenPanelGridDefinition screenPanelGrid = new ScreenPanelGridDefinition()
            {
                Name = $"{targetPanel.Name}_{gridName}",
                DisplayIndex = index,
                Dock = addedGridPanel.Grid.Dock,
                HasSplitter = addedGridPanel.HasSplitter,
                Width = addedGridPanel.Grid.Width,
                Height = addedGridPanel.Grid.Height,
            };

            _screenPanelGridDefinitions.Add(screenPanelGrid.Name, screenPanelGrid);
        }

        private GridAdded AddGridToScreenPanel(Panel targetPanel, string gridName, string styleName = "")
        {
            var gridPanel = new Panel()
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Name = $"{targetPanel.Name}_{gridName}"
            };

            gridPanel.DoubleClick += GridPanel_DoubleClick;

            var gridPanelLabel = new System.Windows.Forms.Label()
            {
                Text = gridName,
                AutoSize = false,
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                Dock = DockStyle.Top
            };

            gridPanelLabel.DoubleClick += GridPanelLabel_DoubleClick;

            gridPanel.Controls.Add(gridPanelLabel);

            var label = new Label()
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                Text = gridPanel.Name
            };

            gridPanel.Controls.Add(label);

            bool addSplitter = false;

            switch (targetPanel.Dock)
            {
                case DockStyle.Top:
                case DockStyle.Bottom:
                    gridPanel.Dock = DockStyle.Left;
                    gridPanel.Width = (targetPanel.Width / 5) - 1;
                    gridPanel.Height = targetPanel.Height - 1;
                    addSplitter = true;
                    break;
                case DockStyle.Left:
                case DockStyle.Right:
                    gridPanel.Dock = DockStyle.Top;
                    gridPanel.Height = (targetPanel.Height / 3) - 1;
                    gridPanel.Width = targetPanel.Width - 1;
                    break;
                case DockStyle.None:
                case DockStyle.Fill:
                    gridPanel.Dock = DockStyle.Fill;
                    break;
                default:
                    break;
            }

            DataGridView mockDataGridView = new DataGridView()
            {
                Dock = DockStyle.Top
            };

            gridPanel.Controls.Add(mockDataGridView);
            mockDataGridView.BringToFront();
            mockDataGridView.DataSource = _sampleData;

            if (!String.IsNullOrEmpty(styleName))
            {
                var style = _styleService.GetStyle(styleName);

                GridStyleHelper.ApplyGridStyleSettings(mockDataGridView, style);
            }

            targetPanel.Controls.Add(gridPanel);

            gridPanel.BringToFront();

            if (addSplitter)
            {
                var splitter = new Splitter()
                {
                    Dock = gridPanel.Dock
                };
                targetPanel.Controls.Add(splitter);

                splitter.BringToFront();
            }

            return new GridAdded()
            {
                Grid = gridPanel,
                HasSplitter = addSplitter
            };
        }

        #endregion

        #region [ screen definitions ]

        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewStates.Saving);

                var screen = BuildScreenDefinitionFromUi();

                bool validationPassed = ValidateScreenDefinition(screen);

                if (!validationPassed)
                    return;

                var existingScreen = _screenDefinitions.FirstOrDefault(s => s.Name == txtScreenName.Text);

                if (existingScreen != null)
                {
                    _screenDefinitions.Remove(existingScreen);
                }

                _screenDefinitions.Add(screen);

                PopulateScreenDefinitionsSelectionControl(_screenDefinitions, screen.Name);

                SetScreenDefinitionToolbarButtonStates();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                SetViewState(ViewStates.Viewing);
            }
        }
        private void tsbAddScreen_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewScreenDefinition();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbCopyScreen_Click(object sender, EventArgs e)
        {
            try
            {
                CopySelectedScreenDefinition();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbDeleteScreen_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveSelectedScreenDefinition();

                SetViewState(ViewStates.Ready);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbEditScreen_Click(object sender, EventArgs e)
        {
            try
            {
                SetViewState(ViewStates.Editing);

                SetScreenDefinitionToolbarButtonStates();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbCancelScreenEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ClearSelectedScreenDefinitionUi();

                switch (_viewState)
                {
                    case ViewStates.Editing:
                        // Reload the existing screen definition
                        _selectedScreenDefinition = lstScreens.SelectedItem as ScreenDefinition;
                        DisplayScreenDefinition(_selectedScreenDefinition);
                        SetViewState(ViewStates.Viewing);
                        break;
                    case ViewStates.Adding:
                        // Delete the added screen definition
                        RemoveSelectedScreenDefinition();
                        SetViewState(ViewStates.Ready);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbShowHeader_CheckStateChanged(object sender, EventArgs e)
        {
            pnlEventTitleMock.Visible = tsbShowHeader.Checked;
        }
        private void tsbShowFlagStatus_CheckStateChanged(object sender, EventArgs e)
        {
            picFlagStateMock.Visible = tsbShowFlagStatus.Checked;
        }
        private void tsbShowLapFlagBar_CheckStateChanged(object sender, EventArgs e)
        {
            pnlGreenYellowMock.Visible = tsbShowLapFlagBar.Checked;
        }

        private void btnScreenUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstScreens.SelectedItem == null)
                    return;

                SortScreenDefinitions(lstScreens.SelectedIndex, -1);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void btnScreenDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstScreens.SelectedItem == null)
                    return;

                SortScreenDefinitions(lstScreens.SelectedIndex, 1);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void lstScreens_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_viewState == ViewStates.Loading)
                return;

            if (_viewState == ViewStates.Saving)
                return;

            ClearSelectedScreenDefinition();

            if (lstScreens.SelectedItem == null)
                return;

            _selectedScreenDefinition = lstScreens.SelectedItem as ScreenDefinition;

            DisplayScreenDefinition(_selectedScreenDefinition);

            SetViewState(ViewStates.Viewing);
        }

        private void PopulateScreenDefinitionsSelectionControl(IList<ScreenDefinition> screens, string selected = "")
        {
            lstScreens.DataSource = null;

            lstScreens.DisplayMember = "Name";

            lstScreens.ValueMember = "Name";

            var displayIndexGroups = screens.GroupBy(s => s.DisplayIndex);
            if (displayIndexGroups.Any(g => g.Count() > 1))
            {
                for (int i = 0; i < screens.Count; i++)
                {
                    screens[i].DisplayIndex = i;
                }
            }

            lstScreens.DataSource = screens.OrderBy(s => s.DisplayIndex).ToList();

            lstScreens.SelectedIndex = -1;

            if (!String.IsNullOrEmpty(selected))
                lstScreens.SelectedValue = selected;
        }

        private void SaveScreenDefinitionsToFile()
        {
            _screenService.SaveScreenDefinitions(_screenDefinitions);
        }

        private bool ValidateScreenDefinition(ScreenDefinition screenDefinition)
        {
            bool validationPassed = true;

            StringBuilder sb = new StringBuilder();

            if (String.IsNullOrEmpty(screenDefinition.Name))
            {
                validationPassed = false;
                sb.AppendLine("Screen name cannot be blank.");
            }

            if (_viewState == ViewStates.Adding)
            {
                if (_screenDefinitions.Any(s => s.Name == screenDefinition.Name))
                {
                    validationPassed = false;
                    sb.AppendLine($"Name must be unique. A screen named '{screenDefinition.Name}' already exists.");
                }
            }

            if (!validationPassed)
                DisplayMessage(sb.ToString(), "New screen validation failed");

            return validationPassed;
        }

        private IList<ScreenDefinition> LoadScreenDefinitionsFromFile()
        {
            return _screenService.GetScreenDefinitions();
        }

        private IList<ScreenDefinition> BuildDefaultScreenDefinitionsList()
        {
            var screens = new List<ScreenDefinition>();

            screens.Add(new ScreenDefinition()
            {
                Name = "Practice",
                Enabled = true,
                Panels = new List<ScreenPanel>()
                {
                    new ScreenPanel()
                    {
                        Name = "pnlRight",
                        DisplayIndex = 0,
                        Dock = DockStyle.Right
                    },
                     new ScreenPanel()
                    {
                        Name = "pnlBottom",
                        DisplayIndex = 1,
                        Dock = DockStyle.Bottom
                    },
                      new ScreenPanel()
                    {
                        Name = "pnlMain",
                        DisplayIndex = 2,
                        Dock = DockStyle.Fill
                    }
                }
            });

            return screens;
        }

        private ScreenDefinition BuildScreenDefinitionFromUi()
        {
            var screen = new ScreenDefinition()
            {
                Name = txtScreenName.Text,
                Enabled = true,
                DisplayEventTitle = tsbShowHeader.Checked,
                DisplayFlagStatus = tsbShowFlagStatus.Checked,
                DisplayGreenYellowLapIndicator = tsbShowLapFlagBar.Checked,
                DisplayIndex = _selectedScreenDefinition.DisplayIndex
            };

            int panelDisplayIndex = 0;

            var mockPanels = pnlScreenMock.Controls.OfType<Panel>().ToList();

            for (int p = mockPanels.Count() - 1; p >= 0; p--)
            {
                var screenMockControl = mockPanels[p];

                var screenPanel = new ScreenPanel()
                {
                    Name = screenMockControl.Name,
                    DisplayIndex = panelDisplayIndex,
                    Dock = screenMockControl.Dock
                };

                if (screenPanel.Dock == DockStyle.Top || screenPanel.Dock == DockStyle.Bottom)
                {
                    screenPanel.Size = (float)screenMockControl.Height / pnlScreenMock.Height;
                }
                else if (screenPanel.Dock == DockStyle.Left || screenPanel.Dock == DockStyle.Right)
                {
                    screenPanel.Size = (float)screenMockControl.Width / pnlScreenMock.Width;
                }

                int gridDisplayIndex = 0;

                var mockGrids = screenMockControl.Controls.OfType<Panel>().ToList();

                for (int g = mockGrids.Count() - 1; g >= 0; g--)
                {
                    Panel gridPanel = mockGrids[g];

                    var gridView = new ScreenPanelGridDefinition()
                    {
                        Name = gridPanel.Name,
                        DisplayIndex = gridDisplayIndex,
                        Dock = gridPanel.Dock,
                        HasSplitter = gridPanel.Dock != DockStyle.None && gridPanel.Dock != DockStyle.Fill,
                        Height = gridPanel.Height,
                        Width = gridPanel.Width
                    };

                    var gridDefinition = _screenPanelGridDefinitions[gridPanel.Name];

                    gridView.Style = gridDefinition.Style;

                    screenPanel.GridViews.Add(gridView);

                    gridDisplayIndex++;
                }

                screen.Panels.Add(screenPanel);

                panelDisplayIndex++;
            }

            return screen;
        }

        private void ClearSelectedScreenDefinitionUi()
        {
            txtScreenName.Clear();

            pnlScreenMock.Controls.Clear();

            cboGridStyles.SelectedIndex = -1;

            _screenPanelGridDefinitions = new Dictionary<string, ScreenPanelGridDefinition>();
        }

        private void ClearSelectedScreenDefinition()
        {
            ClearSelectedScreenDefinitionUi();

            _selectedScreenPanelGridDefinition = null;

            _selectedGridViewPanelControl = null;

            _selectedScreenPanelControl = null;

            _selectedScreenDefinition = null;
        }

        private void DisplayScreenDefinition(ScreenDefinition screen)
        {
            txtScreenName.Text = screen.Name;

            tsbShowHeader.Checked = screen.DisplayEventTitle;
            pnlEventTitleMock.Visible = screen.DisplayEventTitle;

            tsbShowFlagStatus.Checked = screen.DisplayFlagStatus;
            picFlagStateMock.Visible = screen.DisplayFlagStatus;

            tsbShowLapFlagBar.Checked = screen.DisplayGreenYellowLapIndicator;
            pnlGreenYellowMock.Visible = screen.DisplayGreenYellowLapIndicator;

            int i = 0;
            foreach (ScreenPanel screenPanel in screen.Panels.OrderBy(p => p.DisplayIndex))
            {
                Panel p = new Panel()
                {
                    Name = screenPanel.Name.Contains("_") ? screenPanel.Name : $"{_selectedScreenDefinition.Name}_{screenPanel.Name}",
                    BorderStyle = BorderStyle.FixedSingle,
                    Dock = screenPanel.Dock,
                    Size = new Size(100, 100),
                    Location = new Point(100 * i, 100 * i),
                    BackColor = Color.WhiteSmoke
                };

                var label = new Label()
                {
                    AutoSize = false,
                    Dock = DockStyle.Bottom,
                    Text = $"{p.Name} [{screenPanel.DisplayIndex}]"
                };

                p.Controls.Add(label);

                p.DoubleClick += ScreenPanel_DoubleClick;
                p.AllowDrop = true;
                p.DragEnter += screenPanel_DragEnter;
                p.DragDrop += screenPanel_DragDrop;

                pnlScreenMock.Controls.Add(p);

                p.BringToFront();

                if (screenPanel.Size != 0 && (screenPanel.Dock == DockStyle.Top || screenPanel.Dock == DockStyle.Bottom))
                {
                    p.Height = (int)(screenPanel.Size * pnlScreenMock.Height);
                }
                else if (screenPanel.Size != 0 && (screenPanel.Dock == DockStyle.Left || screenPanel.Dock == DockStyle.Right))
                {
                    p.Width = (int)(screenPanel.Size * pnlScreenMock.Width);
                }

                if (p.Dock != DockStyle.None && p.Dock != DockStyle.Fill)
                {
                    Splitter s = new Splitter()
                    {
                        Dock = p.Dock
                    };

                    pnlScreenMock.Controls.Add(s);

                    s.BringToFront();
                }

                for (int x = 0; x < screenPanel.GridViews.Count; x++)
                {
                    if (string.IsNullOrEmpty(screenPanel.GridViews[x].ScreenName))
                        screenPanel.GridViews[x].Name = $"{screen.Name}_{screenPanel.GridViews[x].PanelName}_{screenPanel.GridViews[x].GridName}";
                }

                foreach (ScreenPanelGridDefinition gridView in screenPanel.GridViews.OrderBy(g => g.DisplayIndex))
                {
                    AddGridToScreenPanel(p, gridView.GridName, gridView.Style);

                    _screenPanelGridDefinitions.Add(gridView.Name, gridView);
                }

                i++;
            }
        }

        private void RemoveSelectedScreenDefinition()
        {
            if (lstScreens.SelectedItem == null)
                return;

            var selectedScreen = lstScreens.SelectedItem as ScreenDefinition;

            var promptResponse = MessageBox.Show(this, $"Delete screen '{selectedScreen.Name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (promptResponse == DialogResult.Yes)
            {
                _screenDefinitions.Remove(selectedScreen);

                PopulateScreenDefinitionsSelectionControl(_screenDefinitions);
            }
        }

        private void AddNewScreenDefinition()
        {
            var dialog = new NewScreenNameDialog();

            var response = dialog.ShowDialog(this);

            if (response == DialogResult.OK)
            {
                _selectedScreenDefinition = new ScreenDefinition()
                {
                    Name = dialog.ScreenName
                };

                SetViewState(ViewStates.Adding);

                DisplayScreenDefinition(_selectedScreenDefinition);
            }
        }

        private void tsbEditScreenName_Click(object sender, EventArgs e)
        {
            try
            {
                EditScreenName();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void EditScreenName()
        {
            if (_selectedScreenDefinition == null)
                return;

            var dialog = new NewScreenNameDialog(_selectedScreenDefinition.Name);

            var response = dialog.ShowDialog(this);

            var nameChanged = (response == DialogResult.OK) && (dialog.ScreenName.Trim() != _selectedScreenDefinition.Name.Trim());

            if (nameChanged)
            {
                UpdateScreenName(dialog.ScreenName);
            }
        }

        private void UpdateScreenName(string newScreenName)
        {
            _selectedScreenDefinition.Name = newScreenName;

            foreach (ScreenPanel screenPanel in _selectedScreenDefinition.Panels)
            {
                var panelNameSections = screenPanel.Name.Split('_');

                screenPanel.Name = $"{newScreenName}_{panelNameSections[1]}";

                foreach (ScreenPanelGridDefinition screenPanelGrid in screenPanel.GridViews)
                {
                    var gridNameSections = screenPanelGrid.Name.Split('_');

                    screenPanelGrid.Name = $"{newScreenName}_{gridNameSections[1]}_{gridNameSections[2]}";
                }
            }

            ClearSelectedScreenDefinitionUi();

            DisplayScreenDefinition(_selectedScreenDefinition);
        }

        private void CopySelectedScreenDefinition()
        {
            if (lstScreens.SelectedItem == null)
                return;

            ClearSelectedScreenDefinition();

            var selectedScreen = lstScreens.SelectedItem as ScreenDefinition;

            _selectedScreenDefinition = new ScreenDefinition()
            {
                Name = $"Copy of {selectedScreen.Name}",
                Panels = selectedScreen.Panels.ToList()
            };

            SetViewState(ViewStates.Adding);

            DisplayScreenDefinition(_selectedScreenDefinition);
        }

        private void SortScreenDefinitions(int oldDisplayIndex, int change)
        {
            int newDisplayIndex = oldDisplayIndex + change;

            var targetScreen1 = _screenDefinitions.SingleOrDefault(s => s.DisplayIndex == oldDisplayIndex);
            var targetScreen2 = _screenDefinitions.SingleOrDefault(s => s.DisplayIndex == newDisplayIndex);

            targetScreen2.DisplayIndex = oldDisplayIndex;
            targetScreen1.DisplayIndex = newDisplayIndex;

            PopulateScreenDefinitionsSelectionControl(_screenDefinitions, _selectedScreenDefinition.Name);

            SetToolbarButtonStates();
        }

        #endregion

        #region [ screen panels ]

        private void dockLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedScreenPanelControl == null)
                    return;

                _selectedScreenPanelControl.Dock = DockStyle.Left;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void dockRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedScreenPanelControl == null)
                    return;

                _selectedScreenPanelControl.Dock = DockStyle.Right;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void dockTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedScreenPanelControl == null)
                    return;

                _selectedScreenPanelControl.Dock = DockStyle.Top;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void dockBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedScreenPanelControl == null)
                    return;

                _selectedScreenPanelControl.Dock = DockStyle.Bottom;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void dockFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedScreenPanelControl == null)
                    return;

                _selectedScreenPanelControl.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbBringToFront_Click(object sender, EventArgs e)
        {
            _selectedScreenPanelControl?.BringToFront();
        }
        private void tsbSendToBack_Click(object sender, EventArgs e)
        {
            _selectedScreenPanelControl?.SendToBack();
        }
        private void tsbAddPanel_Click(object sender, EventArgs e)
        {
            try
            {
                AddPanelToScreen();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbRemovePanel_Click(object sender, EventArgs e)
        {
            try
            {
                RemovePanelFromScreen();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void ScreenPanel_DoubleClick(object sender, EventArgs e)
        {
            // Clear any currently selected grids
            ClearSelectedGridPanel();

            Panel panel = (Panel)sender;

            if (panel.BackColor == Color.Yellow)
            {
                panel.BackColor = Color.WhiteSmoke;
                _selectedScreenPanelControl = null;
            }
            else
            {
                foreach (Panel mockPanel in pnlScreenMock.Controls.OfType<Panel>())
                {
                    mockPanel.BackColor = Color.WhiteSmoke;
                }
                panel.BackColor = Color.Yellow;
                _selectedScreenPanelControl = panel;
            }

            SetPanelToolbarButtonStates();
        }

        private void ClearSelectedPanel()
        {
            foreach (Panel mockPanel in pnlScreenMock.Controls.OfType<Panel>())
            {
                mockPanel.BackColor = Color.WhiteSmoke;
            }

            _selectedScreenPanelControl = null;
        }

        private void AddPanelToScreen()
        {
            var newScreenPanel = new ScreenPanel()
            {
                Name = $"Panel{_selectedScreenDefinition.Panels.Count + 1}",
                DisplayIndex = _selectedScreenDefinition.Panels.Count,
                Dock = DockStyle.Left
            };
            _selectedScreenDefinition.Panels.Add(newScreenPanel);

            ClearSelectedScreenDefinitionUi();

            DisplayScreenDefinition(_selectedScreenDefinition);
        }

        private void RemovePanelFromScreen()
        {
            if (_selectedScreenPanelControl == null)
                return;

            var panelToRemove = _selectedScreenDefinition.Panels.FirstOrDefault(p => p.Name == _selectedScreenPanelControl.Name);

            if (panelToRemove != null)
            {
                var promptResponse = MessageBox.Show(this, "Delete panel?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (promptResponse == DialogResult.Yes)
                {
                    _selectedScreenDefinition.Panels.Remove(panelToRemove);

                    ClearSelectedScreenDefinitionUi();

                    DisplayScreenDefinition(_selectedScreenDefinition);

                    SetPanelToolbarButtonStates();
                }
            }
        }

        #endregion

        #region [ grid views ]

        private void tsbMoveLeft_Click(object sender, EventArgs e)
        {
            try
            {
                SortScreenPanelGrids(-1);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbMoveRight_Click(object sender, EventArgs e)
        {
            try
            {
                SortScreenPanelGrids(1);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                SortScreenPanelGrids(-1);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                SortScreenPanelGrids(1);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void tsbDeleteGrid_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveSelectedGrid();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void GridPanel_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ClearSelectedGridPanel();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void GridPanelLabel_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Label selectedLabel = (System.Windows.Forms.Label)sender;

                if (selectedLabel.BackColor == Color.Yellow)
                {
                    ClearSelectedGridPanel();
                }
                else
                {
                    var gridPanel = selectedLabel.Parent as Panel;
                    var screenPanel = gridPanel.Parent as Panel;

                    SelectGridPanel(screenPanel.Name, selectedLabel.Text, selectedLabel);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private IList<GridViewDefinition> BuildDynamicGridViewsList()
        {
            var grids = new List<GridViewDefinition>();

            // find custom grid views

            var customGridDefinitions = _settingsService.GetCustomViewSettings();

            foreach (var customView in customGridDefinitions)
            {
                grids.Add(new GridViewDefinition()
                {
                    Name = $"{customView.Title} *",
                    IsCustomView = true
                });
            }

            return grids;
        }

        private IList<GridViewDefinition> BuildGridViewsList()
        {
            var grids = new List<GridViewDefinition>();

            grids.Add(new GridViewDefinition()
            {
                Name = "Leaderboard",
                Width = 800,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Driver Points",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Fastest Laps",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Flags",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Lap Leaders",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Movers",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Fallers",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Stage Points",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Best 5 Lap Average",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Best 10 Lap Average",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Best 15 Lap Average",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Last 5 Lap Average",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Last 10 Lap Average",
                Width = 250,
                Height = 250
            });

            grids.Add(new GridViewDefinition()
            {
                Name = "Last 15 Lap Average",
                Width = 250,
                Height = 250
            });

            var customGrids = BuildDynamicGridViewsList();

            grids.AddRange(customGrids);

            return grids;
        }

        private void PopulateGridViewsSelectionControl(IList<GridViewDefinition> grids)
        {
            lstGridViews.DataSource = null;

            lstGridViews.DisplayMember = "Name";

            lstGridViews.ValueMember = "Name";

            lstGridViews.DataSource = grids.OrderBy(g => g.Name).ToList();

            lstGridViews.SelectedIndex = -1;
        }

        private void ClearSelectedGridPanel()
        {
            if (_selectedGridViewPanelControl != null)
            {
                foreach (Label childLabel in _selectedGridViewPanelControl.Controls.OfType<Label>())
                {
                    if (childLabel.BackColor == Color.Yellow)
                    {
                        childLabel.BackColor = childLabel.Text.Contains("_") ? Color.White : Color.DimGray;
                        childLabel.ForeColor = childLabel.Text.Contains("_") ? Color.Black : Color.White;
                    }
                }

                _selectedGridViewPanelControl.BackColor = Color.White;
                _selectedGridViewPanelControl = null;
            }

            _selectedScreenPanelGridDefinition = null;

            SetGridToolbarButtonStates(_selectedScreenPanelGridDefinition);
        }

        private void SelectGridPanel(string panelName, string gridName, Label label)
        {
            // Clear any currently selected panels
            ClearSelectedPanel();

            if (_selectedGridViewPanelControl != null)
            {
                foreach (Label childLabel in _selectedGridViewPanelControl.Controls.OfType<Label>())
                {
                    if (childLabel.BackColor == Color.Yellow)
                    {
                        childLabel.BackColor = childLabel.Text.Contains("_") ? Color.White : Color.DimGray;
                        childLabel.ForeColor = childLabel.Text.Contains("_") ? Color.Black : Color.White;
                    }
                }

                _selectedGridViewPanelControl.BackColor = Color.White;
                _selectedGridViewPanelControl = null;
            }

            label.ForeColor = Color.Black;
            label.BackColor = Color.Yellow;

            var gridPanelKey = $"{panelName}_{gridName}";

            var screenPanel = pnlScreenMock.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == panelName);

            var gridPanel = screenPanel.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == gridPanelKey);

            _selectedGridViewPanelControl = gridPanel;

            gridPanel.BackColor = Color.Yellow;

            _selectedScreenPanelGridDefinition = _screenPanelGridDefinitions[gridPanelKey];

            SetGridToolbarButtonStates(_selectedScreenPanelGridDefinition);

            cboGridStyles.SelectedItem = _selectedScreenPanelGridDefinition.Style;
        }

        private void RemoveSelectedGrid()
        {
            if (_selectedScreenPanelGridDefinition == null)
                return;

            var screenPanel = GetSelectedScreenPanel();

            var promptResponse = MessageBox.Show(this, $"Remove grid '{_selectedScreenPanelGridDefinition.GridName}' from this screen?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (promptResponse == DialogResult.Yes)
            {
                _screenPanelGridDefinitions.Remove(_selectedScreenPanelGridDefinition.Name);

                var screenGrids = GetGridsByScreen(_selectedScreenPanelGridDefinition.PanelName);

                int i = 0;

                foreach (var item in screenGrids)
                {
                    item.DisplayIndex = i;
                    i++;
                }

                DisplayScreenPanelGrids(screenPanel, screenGrids);

                _selectedGridViewPanelControl = null;
                _selectedScreenPanelGridDefinition = null;

                SetGridToolbarButtonStates(_selectedScreenPanelGridDefinition);
            }
        }

        private Panel GetSelectedScreenPanel()
        {
            if (_selectedGridViewPanelControl == null)
                return null;

            return _selectedGridViewPanelControl.Parent as Panel;
        }

        private void SortScreenPanelGrids(int change)
        {
            var screenPanel = GetSelectedScreenPanel();

            if (screenPanel == null)
                return;

            SortScreenPanelGrids(
                screenPanel,
                _selectedScreenPanelGridDefinition.DisplayIndex,
                change);
        }

        private void SortScreenPanelGrids(Panel screenPanel, int oldDisplayIndex, int change)
        {
            if (_selectedScreenPanelGridDefinition == null)
                return;

            int newDisplayIndex = oldDisplayIndex + change;

            var screenGrids = GetGridsByScreen(_selectedScreenPanelGridDefinition.PanelName);

            var targetScreenGrid1 = screenGrids.SingleOrDefault(s => s.DisplayIndex == oldDisplayIndex);
            var targetScreenGrid2 = screenGrids.SingleOrDefault(s => s.DisplayIndex == newDisplayIndex);

            targetScreenGrid2.DisplayIndex = oldDisplayIndex;
            targetScreenGrid1.DisplayIndex = newDisplayIndex;

            DisplayScreenPanelGrids(screenPanel, screenGrids);

            SetGridToolbarButtonStates(_selectedScreenPanelGridDefinition);
        }

        private void DisplayScreenPanelGrids(Panel screenPanel, IList<ScreenPanelGridDefinition> screenGrids)
        {
            var gridPanels = screenPanel.Controls.OfType<Panel>().ToList();
            var gridPanelSplitters = screenPanel.Controls.OfType<Splitter>().ToList();

            screenPanel.Controls.Clear();

            foreach (Splitter splitter in gridPanelSplitters)
            {
                splitter.Dispose();
            }

            foreach (ScreenPanelGridDefinition screenGrid in screenGrids.OrderBy(g => g.DisplayIndex))
            {
                Panel gridPanel = gridPanels.FirstOrDefault(g => g.Name == screenGrid.Name);

                screenPanel.Controls.Add(gridPanel);
                gridPanel.BringToFront();

                if (screenGrid.HasSplitter)
                {
                    Splitter splitter = new Splitter()
                    {
                        Dock = gridPanel.Dock
                    };
                    screenPanel.Controls.Add(splitter);
                    splitter.BringToFront();
                }
            }
        }

        private IList<ScreenPanelGridDefinition> GetGridsByScreen(string panelName)
        {
            IList<ScreenPanelGridDefinition> screenPanelGrids = new List<ScreenPanelGridDefinition>();

            foreach (KeyValuePair<string, ScreenPanelGridDefinition> item in _screenPanelGridDefinitions)
            {
                if (item.Key.Contains(panelName))
                    screenPanelGrids.Add(item.Value);
            }

            return screenPanelGrids.OrderBy(s => s.DisplayIndex).ToList();
        }

        #endregion

        #region [ styles ]

        private IList<GridStyleSettings> LoadStylesList()
        {
            return _styleService.GetStyles();
        }

        private void PopulateStylesSelectionControls(IList<GridStyleSettings> styles, string selectedStyle = "")
        {
            foreach (GridStyleSettings style in styles.OrderBy(s => s.Name))
            {
                cboGridStyles.Items.Add(style.Name);
            }

            cboGridStyles.SelectedIndex = -1;
        }

        private void btnRemoveStyle_Click(object sender, EventArgs e)
        {
            cboGridStyles.SelectedIndex = -1;
        }

        private void cboGridStyles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsEditing)
                return;

            if (cboGridStyles.SelectedItem != null)
            {
                var styleName = cboGridStyles.SelectedItem.ToString();

                if (_selectedScreenPanelGridDefinition != null)
                {
                    _selectedScreenPanelGridDefinition.Style = styleName;
                    _screenPanelGridDefinitions[_selectedScreenPanelGridDefinition.Name].Style = styleName;
                }

                // apply the slected style
                if (_selectedGridViewPanelControl != null)
                {
                    foreach (DataGridView childDataGridView in _selectedGridViewPanelControl.Controls.OfType<DataGridView>())
                    {
                        var style = _styleService.GetStyle(styleName);

                        GridStyleHelper.ApplyGridStyleSettings(childDataGridView, style);
                    }
                }
            }
            else
            {
                // clear the style
                if (_selectedGridViewPanelControl != null)
                {
                    foreach (DataGridView childDataGridView in _selectedGridViewPanelControl.Controls.OfType<DataGridView>())
                    {
                        GridStyleHelper.ApplyGridStyleSettings(childDataGridView, new GridStyleSettings());
                    }
                }
            }
        }

        #endregion

        #region [ dialog buttons ]

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                SaveScreenDefinitionsToFile();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

        }

        #endregion

        #region classes

        internal class GridViewDefinition
        {
            public string Name { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public bool Visible { get; set; }
            public bool IsCustomView { get; set; }
        }

        internal class GridAdded
        {
            public Panel Grid { get; set; }
            public bool HasSplitter { get; set; }
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