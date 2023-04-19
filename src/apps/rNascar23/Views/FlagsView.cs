using rNascar23.Flags.Models;
using rNascar23.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class FlagsView : GridViewBase
    {
        #region consts

        private const int FlagStateColumnIndex = 0;
        private const int LapNumberColumnIndex = 1;
        private const int CommentColumnIndex = 2;

        #endregion

        #region ctor

        public FlagsView()
        {
            InitializeComponent();

            Grid.RowsAdded += Grid_RowsAdded;
        }

        #endregion

        #region protected

        protected override void AddColumns()
        {
            DataGridViewTextBoxColumn colFlagState = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colLapNumber = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colComment = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colLuckyDog = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colTimeOfDayOS = new DataGridViewTextBoxColumn();

            Grid.Columns.AddRange(new DataGridViewColumn[]
            {
                colFlagState,
                colLapNumber,
                colComment,
                colLuckyDog,
                colTimeOfDayOS
            });

            GridViewColumnBuilder.ConfigureColumn(colFlagState, "State", 50, "Flag");

            GridViewColumnBuilder.ConfigureColumn(colLapNumber, "LapNumber", 35, "Lap");

            GridViewColumnBuilder.ConfigureColumn(colComment, "Comment", 275, "Caution For");

            GridViewColumnBuilder.ConfigureColumn(colLuckyDog, "Beneficiary", 45, "Lucky Dog");

            GridViewColumnBuilder.ConfigureColumn(colTimeOfDayOS, "TimeOfDayOs", 45, "TimeOfDayOs");

            Grid.ColumnHeadersVisible = true;
            Grid.RowHeadersVisible = false;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.ReadOnly = true;
            Grid.AutoGenerateColumns = false;
            Grid.AllowUserToResizeRows = false;
            Grid.SelectionChanged += (s, e) => Grid.ClearSelection();
        }

        #endregion

        #region public

        public override void SetDataSource<TModel>(IList<TModel> models)
        {
            if (Grid.DataSource == null)
            {
                if (Settings.MaxRows.HasValue)
                    models = models.
                        Take(Settings.MaxRows.Value).
                        ToList();

                _dataTable = GridViewTableBuilder.ToDataTable(models.ToList());

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
                        var position = row.Field<int>("LapNumber");

                        if (!models.OfType<FlagState>().Any(m => m.LapNumber == position))
                        {
                            rowsToRemove.Add(row);
                        }
                    }

                    foreach (DataRow row in rowsToRemove)
                    {
                        _dataTable.Rows.Remove(row);
                    }
                }

                foreach (FlagState model in models.OfType<FlagState>())
                {
                    DataRow[] foundRows = _dataTable.Select($"State = {(int)model.State} AND LapNumber = {model.LapNumber}");

                    if (foundRows != null && foundRows.Length > 0)
                    {
                        foundRows[0]["Comment"] = model.Comment;
                        foundRows[0]["Beneficiary"] = model.Beneficiary;
                    }
                    else
                    {
                        var comment = String.IsNullOrEmpty(model.Comment) ? " " : model.Comment;
                        var beneficiary = String.IsNullOrEmpty(model.Beneficiary) ? " " : model.Beneficiary;

                        var dr = _dataTable.NewRow();

                        dr["State"] = (int)model.State;
                        dr["LapNumber"] = model.LapNumber;
                        dr["Comment"] = comment;
                        dr["Beneficiary"] = beneficiary;
                        dr["TimeOfDayOs"] = model.TimeOfDayOs.ToString("h:mm tt");

                        _dataTable.Rows.Add(dr);
                    }
                }
            }

            var cautionCountGroup = models.OfType<FlagState>().
               Where(m => m.State == FlagColors.Yellow).
               GroupBy(m => $"{m.State}-{m.LapNumber}");

            var cautionCount = cautionCountGroup.Count();

            FlagColors previousFlag = FlagColors.None;
            int previousCautionStartLap = 0;
            int runningCautionLapCount = 0;
            foreach (FlagState model in models.OfType<FlagState>())
            {
                if (model.State == FlagColors.Green && previousFlag == FlagColors.None)
                {
                    // green flag to start event
                }
                else if (model.State == FlagColors.Green && previousFlag == FlagColors.Yellow)
                {
                    // green flag after a yellow
                    runningCautionLapCount += (model.LapNumber - previousCautionStartLap);
                }
                else if (model.State == FlagColors.Yellow && previousFlag == FlagColors.Green)
                {
                    // yellow flag after a green
                    previousCautionStartLap = model.LapNumber;
                }
                else if (model.State == FlagColors.Yellow && previousFlag == FlagColors.Red)
                {
                    // yellow flag after a red
                }

                previousFlag = model.State;
            }

            var cautionCountLabel = cautionCount == 1 ? "Caution" : "Cautions";

            this.TitleLabel.Text = $"Flags     {cautionCount} {cautionCountLabel} for {runningCautionLapCount} Laps";

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

                row.DividerHeight = 4;

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
                    else if (int.Parse(row.Cells[FlagStateColumnIndex].Value.ToString()) == (int)FlagColors.Checkered)
                    {
                        row.Cells[LapNumberColumnIndex].Style.BackColor = FlagUiColors.Checkered;
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
                    row.Cells[LapNumberColumnIndex].Style.BackColor = row.Cells[FlagStateColumnIndex].Style.BackColor;
                }
            }
        }

        #endregion
    }
}
