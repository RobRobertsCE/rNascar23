using rNascar23.Sdk.Flags.Models;
using rNascar23.Logic;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using rNascar23.Sdk.Common;

namespace rNascar23.Views
{
    public partial class KeyMomentsView : GridViewBase
    {
        #region consts

        private const int LapNumberColumnIndex = 1;
        private const int FlagStateColumnIndex = 2;

        #endregion

        #region ctor

        public KeyMomentsView()
        {
            InitializeComponent();

            Grid.RowsAdded += Grid_RowsAdded;
        }

        #endregion

        #region protected

        protected override void AddColumns()
        {
            DataGridViewTextBoxColumn colNoteId = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colLapNumber = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colFlagColor = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colNote = new DataGridViewTextBoxColumn();

            Grid.Columns.AddRange(new DataGridViewColumn[]
            {
                colNoteId,
                colLapNumber,
                colFlagColor,
                colNote,
            });

            GridViewColumnBuilder.ConfigureColumn(colNoteId, "NoteId", 50, "NoteId");
            GridViewColumnBuilder.ConfigureColumn(colLapNumber, "LapNumber", 35, "Lap");

            colFlagColor.HeaderText = "";
            colFlagColor.Name = "FlagColor";
            colFlagColor.Width = 50;
            colFlagColor.Visible = true;
            colFlagColor.DefaultCellStyle.BackColor = Color.White;

            GridViewColumnBuilder.ConfigureColumn(colNote, "Note", 200);

            Grid.ColumnHeadersVisible = true;
            Grid.RowHeadersVisible = false;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.ReadOnly = true;
            Grid.AutoGenerateColumns = false;
            Grid.AllowUserToResizeRows = false;
            Grid.SelectionChanged += (s, e) => Grid.ClearSelection();
            Grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        #endregion

        #region public

        public override void SetDataSource<TModel>(IList<TModel> models)
        {
            if (models == null || models.Count == 0)
                return;

            if (Settings.MaxRows.HasValue)
                models = models.
                    Take(Settings.MaxRows.Value).
                    ToList();

            _dataTable = GridViewTableBuilder.ToDataTable(models.ToList());

            var dataView = new DataView(_dataTable);

            GridBindingSource.DataSource = dataView;

            Grid.DataSource = GridBindingSource;

            if (Grid.RowCount > 0)
                Grid.FirstDisplayedScrollingRowIndex = Grid.RowCount - 1;
        }

        #endregion

        #region private

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var grid = sender as DataGridView;

            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                var row = grid.Rows[i];

                row.DividerHeight = 10;

                if (row.Cells[FlagStateColumnIndex].Value != null)
                {
                    if (int.Parse(row.Cells[FlagStateColumnIndex].Value.ToString()) == (int)FlagColors.Green)
                    {
                        row.Cells[LapNumberColumnIndex].Style.BackColor = FlagUiColors.Green;
                        row.Cells[LapNumberColumnIndex].Style.ForeColor = Color.White;
                    }
                    else if (int.Parse(row.Cells[FlagStateColumnIndex].Value.ToString()) == (int)FlagColors.Yellow)
                    {
                        row.Cells[LapNumberColumnIndex].Style.BackColor = FlagUiColors.Yellow;
                        row.Cells[LapNumberColumnIndex].Style.ForeColor = Color.Black;
                    }
                    else if (int.Parse(row.Cells[FlagStateColumnIndex].Value.ToString()) == (int)FlagColors.Red)
                    {
                        row.Cells[LapNumberColumnIndex].Style.BackColor = FlagUiColors.Red;
                        row.Cells[LapNumberColumnIndex].Style.ForeColor = Color.Black;
                    }
                    else if (int.Parse(row.Cells[FlagStateColumnIndex].Value.ToString()) == (int)FlagColors.White)
                    {
                        row.Cells[LapNumberColumnIndex].Style.BackColor = FlagUiColors.White;
                        row.Cells[LapNumberColumnIndex].Style.ForeColor = Color.Black;
                    }
                    else if (int.Parse(row.Cells[FlagStateColumnIndex].Value.ToString()) == (int)FlagColors.HotTrack)
                    {
                        row.Cells[LapNumberColumnIndex].Style.BackColor = FlagUiColors.HotTrack;
                        row.Cells[LapNumberColumnIndex].Style.ForeColor = Color.Black;
                    }
                    else if (int.Parse(row.Cells[FlagStateColumnIndex].Value.ToString()) == (int)FlagColors.ColdTrack)
                    {
                        row.Cells[LapNumberColumnIndex].Style.BackColor = FlagUiColors.ColdTrack;
                        row.Cells[LapNumberColumnIndex].Style.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.Cells[LapNumberColumnIndex].Style.BackColor = row.Cells[FlagStateColumnIndex].Style.BackColor;
                    }
                }
                else
                {
                    row.Cells[2].Style.BackColor = row.Cells[FlagStateColumnIndex].Style.BackColor;
                }
            }
        }

        #endregion
    }
}
