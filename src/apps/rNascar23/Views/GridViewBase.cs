using rNascar23.Common;
using rNascar23.CustomViews;
using rNascar23.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class GridViewBase : UserControl
    {
        public const int ScrollBarWidth = 24;
        public const int N0Width = 40;
        public const int N2Width = 55;
        public const int N3Width = 70;
        public const int DriverNameWidth = 140;

        public enum GridSortOrder
        {
            None,
            Ascending,
            Descending
        }

        protected DataTable _dataTable = null;

        public GridViewTypes GridViewType { get; set; }

        public GridSettings Settings { get; set; }

        public GridStyleSettings Style { get; set; }

        public GridViewBase()
        {
            InitializeComponent();

            Settings = new GridSettings()
            {
                TitleBackColor = Color.White,
                TitleForeColor = Color.Black,
                Title = "Basic Grid",
                HideColumnHeaders = true,
                HideRowSelector = true,
                Columns = new List<GridColumnSettings>()
                {
                    new GridColumnSettings()
                    {
                        Index= 0,
                        DataProperty = "Position",
                        DisplayIndex= 0,
                        HeaderTitle ="",
                        Visible =false,
                        Width = N0Width
                    },
                    new GridColumnSettings()
                    {
                        Index= 1,
                        DataProperty = "Driver",
                        DisplayIndex= 1,
                        HeaderTitle ="",
                        Visible =true,
                        Width = DriverNameWidth
                    },
                    new GridColumnSettings()
                    {
                        Index= 2,
                        DataProperty = "Value",
                        DisplayIndex= 2,
                        HeaderTitle ="",
                        Visible =true,
                        Width = N0Width,
                        Format = "N0"
                    }
                },
                SortOrderField = "Value",
                SortOrder = (int)GridSortOrder.Descending
            };

            AutoSizeGrid();

            var userSettings = UserSettingsService.LoadUserSettings();

            if (userSettings.UseDarkTheme)
            {
                Style = GetDarkGridStyleSettings();
            }
            else
            {
                Style = GetLightGridStyleSettings();
            }
        }

        private GridStyleSettings GetDarkGridStyleSettings()
        {
            return new GridStyleSettings()
            {
                GridStyle = new GridStyle()
                {
                    BackColor = Color.Black.ToArgb(),
                    LineColor = Color.FromArgb(24, 24, 24).ToArgb(),
                },
                HeaderStyle = new GridRowStyle()
                {
                    BackColor = Color.Black.ToArgb(),
                    ForeColor = Color.Silver.ToArgb(),
                    Font = new GridRowStyle.GridFont()
                    {
                        FontName = "Segoe UI",
                        FontSize = 10,
                        FontStyle = 0
                    }
                },
                RowStyle = new GridRowStyle()
                {
                    BackColor = Color.Black.ToArgb(),
                    ForeColor = Color.Silver.ToArgb(),
                    Font = new GridRowStyle.GridFont()
                    {
                        FontName = "Segoe UI",
                        FontSize = 11,
                        FontStyle = 0
                    }
                },
                AlternatingRowStyle = new GridRowStyle()
                {
                    BackColor = Color.FromArgb(8, 8, 8).ToArgb(),
                    ForeColor = Color.WhiteSmoke.ToArgb(),
                    Font = new GridRowStyle.GridFont()
                    {
                        FontName = "Segoe UI",
                        FontSize = 11,
                        FontStyle = 0
                    }
                }
            };
        }

        private GridStyleSettings GetLightGridStyleSettings()
        {
            return new GridStyleSettings()
            {
                GridStyle = new GridStyle()
                {
                    BackColor = Color.WhiteSmoke.ToArgb(),
                    LineColor = Color.Gray.ToArgb(),
                },
                HeaderStyle = new GridRowStyle()
                {
                    BackColor = Color.WhiteSmoke.ToArgb(),
                    ForeColor = Color.Black.ToArgb(),
                    Font = new GridRowStyle.GridFont()
                    {
                        FontName = "Segoe UI",
                        FontSize = 10,
                        FontStyle = 0
                    }
                },
                RowStyle = new GridRowStyle()
                {
                    BackColor = Color.White.ToArgb(),
                    ForeColor = Color.Black.ToArgb(),
                    Font = new GridRowStyle.GridFont()
                    {
                        FontName = "Segoe UI",
                        FontSize = 11,
                        FontStyle = 0
                    }
                },
                AlternatingRowStyle = new GridRowStyle()
                {
                    BackColor = Color.FromArgb(233, 233, 233).ToArgb(),
                    ForeColor = Color.Black.ToArgb(),
                    Font = new GridRowStyle.GridFont()
                    {
                        FontName = "Segoe UI",
                        FontSize = 11,
                        FontStyle = 0
                    }
                }
            };
        }

        internal virtual void AutoSizeGrid()
        {
            Settings.GridWidth = Settings.Columns.Where(c => c.Visible == true).Sum(c => c.Width);
            Settings.ViewWidth = Settings.GridWidth + ScrollBarWidth;
        }

        private void BasicGrid_Load(object sender, EventArgs e)
        {
            AddColumns();

            if (Settings != null)
            {
                ApplyGridSettings(Settings);
            }

            if (Style != null)
            {
                GridStyleHelper.ApplyGridStyleSettings(Grid, Style);
            }
        }

        public virtual void SetDataSource<TModel>(IList<TModel> models)
        {
            if (Grid.DataSource == null)
            {
                if (Settings.MaxRows.HasValue)
                    models = models.Take(Settings.MaxRows.Value).ToList();

                _dataTable = GridViewTableBuilder.ToDataTable<TModel>(models.ToList());

                var dataView = new DataView(_dataTable);

                GridBindingSource.DataSource = dataView;

                Grid.DataSource = GridBindingSource;
            }
            else
            {
                // update the table
                if (_dataTable.Rows.Count > models.Count)
                {
                    IList<DataRow> rowsToRemove = new List<DataRow>();
                    foreach (DataRow row in _dataTable.Rows)
                    {
                        var position = row.Field<int>("Position");

                        if (!models.OfType<GenericGridViewModel>().Any(m => m.Position == position))
                        {
                            rowsToRemove.Add(row);
                        }
                    }

                    foreach (DataRow row in rowsToRemove)
                    {
                        _dataTable.Rows.Remove(row);
                    }
                }

                foreach (GenericGridViewModel model in models.OfType<GenericGridViewModel>())
                {
                    DataRow[] foundRows = _dataTable.Select($"Position = {model.Position}");

                    if (foundRows != null && foundRows.Length > 0)
                    {
                        foundRows[0]["Driver"] = model.Driver;
                        foundRows[0]["Value"] = model.Value;
                    }
                    else
                    {
                        _dataTable.Rows.Add(model.Position, model.Driver, model.Value);
                    }
                }
            }

            //try
            //{
            //    GridBindingSource.Sort = null;

            //    if (Grid.DataSource != null && String.IsNullOrEmpty(GridBindingSource.Sort) && !String.IsNullOrEmpty(Settings.SortOrderField))
            //    {
            //        var sortColumnIndex = _dataTable.Columns.IndexOf(Settings.SortOrderField);

            //        if (sortColumnIndex == -1)
            //        {
            //            // bad column
            //        }
            //        else
            //        {
            //            var sortDirection = Settings.SortOrder == 0 ? String.Empty :
            //                Settings.SortOrder == 1 ? "ASC" :
            //                "DESC";

            //            var sortString = $"{Settings.SortOrderField} {sortDirection}".Trim();

            //            GridBindingSource.Sort = sortString;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}
        }

        protected virtual void AddColumns()
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
            });

            GridViewColumnBuilder.ConfigureColumn(Column1, "Position", 25);

            GridViewColumnBuilder.ConfigureColumn(Column2, "Driver", 150);

            GridViewColumnBuilder.ConfigureColumn(Column3, "Value", 75);

            Grid.ColumnHeadersVisible = false;
            Grid.RowHeadersVisible = false;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.ReadOnly = true;
            Grid.AutoGenerateColumns = false;
            Grid.AllowUserToResizeRows = false;
            Grid.SelectionChanged += (s, e) => Grid.ClearSelection();
        }

        protected virtual void ApplyGridSettings(GridSettings gridSettings)
        {
            TitleLabel.Text = gridSettings.Title;
            TitleLabel.ForeColor = gridSettings.TitleForeColor;
            TitleLabel.BackColor = gridSettings.TitleBackColor;

            Grid.RowHeadersVisible = !gridSettings.HideRowSelector;
            Grid.ColumnHeadersVisible = !gridSettings.HideColumnHeaders;

            if (gridSettings.GridWidth.HasValue)
                Grid.Width = gridSettings.GridWidth.Value;
            if (gridSettings.GridHeight.HasValue)
                Grid.Height = gridSettings.GridHeight.Value;
            if (gridSettings.ViewWidth.HasValue)
                this.Width = gridSettings.ViewWidth.Value;
            if (gridSettings.ViewHeight.HasValue)
                this.Height = gridSettings.ViewHeight.Value;

            if (Settings.Columns != null && Settings.Columns.Count > 0)
            {
                foreach (GridColumnSettings columnSettings in Settings.Columns)
                {
                    var column = Grid.Columns[columnSettings.Index];
                    if (column == null) continue;

                    column.Visible = columnSettings.Visible;
                    column.DisplayIndex = columnSettings.DisplayIndex;
                    column.Width = columnSettings.Width;

                    if (!String.IsNullOrEmpty(columnSettings.Format))
                        column.DefaultCellStyle.Format = columnSettings.Format;

                    if (!String.IsNullOrEmpty(columnSettings.DataProperty))
                        column.DataPropertyName = columnSettings.DataProperty;

                    if (!String.IsNullOrEmpty(columnSettings.HeaderTitle))
                        column.HeaderText = columnSettings.HeaderTitle;
                }
            }

            Grid.AllowUserToAddRows = false;
            Grid.AllowUserToDeleteRows = false;
            Grid.AllowUserToResizeRows = false;

            Grid.AllowUserToOrderColumns = false;
            Grid.AllowUserToResizeColumns = false;

            Grid.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
        }
    }
}
