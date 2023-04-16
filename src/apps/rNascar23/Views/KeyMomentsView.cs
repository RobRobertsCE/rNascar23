using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class KeyMomentsView : rNascar23.Views.GridViewBase
    {
        public KeyMomentsView()
        {
            InitializeComponent();

            Grid.RowsAdded += Grid_RowsAdded;
        }

        protected override void AddColumns()
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colFlagColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
                colFlagColor,
                Column4,
            });

            GridViewColumnBuilder.ConfigureColumn(Column1, "NoteId", 50, "NoteId");
            GridViewColumnBuilder.ConfigureColumn(Column2, "LapNumber", 35, "Lap");
            GridViewColumnBuilder.ConfigureColumn(Column3, "FlagState", 50, "State");

            colFlagColor.HeaderText = "";
            colFlagColor.Name = "FlagColor";
            colFlagColor.Width = 50;
            colFlagColor.Visible = true;
            colFlagColor.DefaultCellStyle.BackColor = Color.White;

            GridViewColumnBuilder.ConfigureColumn(Column4, "Note", 275);

            Grid.ColumnHeadersVisible = true;
            Grid.RowHeadersVisible = false;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.ReadOnly = true;
            Grid.AutoGenerateColumns = false;
            Grid.AllowUserToResizeRows = false;
            Grid.SelectionChanged += (s, e) => Grid.ClearSelection();
            Grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        }

        public override void SetDataSource<TModel>(IList<TModel> models)
        {
            if (models == null || models.Count == 0)
                return;

            if (Settings.MaxRows.HasValue)
                models = models.
                    Take(Settings.MaxRows.Value).
                    ToList();

            _dataTable = GridViewTableBuilder.ToDataTable<TModel>(models.ToList());

            var dataView = new DataView(_dataTable);

            GridBindingSource.DataSource = dataView;

            Grid.DataSource = GridBindingSource;

            if (Grid.RowCount > 0)
                Grid.FirstDisplayedScrollingRowIndex = Grid.RowCount - 1;
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var grid = sender as DataGridView;

            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                var row = grid.Rows[i];

                if (row.Cells[2].Value != null)
                {
                    if (int.Parse(row.Cells[2].Value.ToString()) == 1)
                    {
                        row.Cells[3].Style.BackColor = Color.Green;
                    }
                    else if (int.Parse(row.Cells[2].Value.ToString()) == 2)
                    {
                        row.Cells[3].Style.BackColor = Color.Gold;
                    }
                    else if (int.Parse(row.Cells[2].Value.ToString()) == 3)
                    {
                        row.Cells[3].Style.BackColor = Color.Red;
                    }
                    else if (int.Parse(row.Cells[2].Value.ToString()) == 4)
                    {
                        row.Cells[3].Style.BackColor = Color.White;
                    }
                    else if (int.Parse(row.Cells[2].Value.ToString()) == 8)
                    {
                        row.Cells[3].Style.BackColor = Color.Orange;
                    }
                    else if (int.Parse(row.Cells[2].Value.ToString()) == 9)
                    {
                        row.Cells[3].Style.BackColor = Color.CornflowerBlue;
                    }
                    else
                    {
                        row.Cells[3].Style.BackColor = row.Cells[0].Style.BackColor;
                    }
                }
                else
                {
                    row.Cells[3].Style.BackColor = row.Cells[0].Style.BackColor;
                }
            }
        }
    }
}
