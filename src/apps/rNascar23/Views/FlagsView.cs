using rNascar23.Flags.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace rNascar23.Views
{
    public partial class FlagsView : rNascar23.Views.GridViewBase
    {
        public FlagsView()
        {
            InitializeComponent();

            Grid.RowsAdded += Grid_RowsAdded;
        }

        protected override void AddColumns()
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn colFlagColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                colFlagColor,
                Column2,
                Column3,
                Column4,
                Column5,
                Column6,
                Column7
            });

            GridViewColumnBuilder.ConfigureColumn(Column1, "State", 50, "Flag");

            colFlagColor.HeaderText = "";
            colFlagColor.Name = "FlagColor";
            colFlagColor.Width = 50;
            colFlagColor.Visible = true;
            colFlagColor.DefaultCellStyle.BackColor = Color.White;

            GridViewColumnBuilder.ConfigureColumn(Column2, "LapNumber", 35, "Lap");

            GridViewColumnBuilder.ConfigureColumn(Column3, "Comment", 275, "Caution For");

            GridViewColumnBuilder.ConfigureColumn(Column4, "Beneficiary", 45, "Lucky Dog");

            GridViewColumnBuilder.ConfigureColumn(Column5, "ElapsedTime", 35, "ElapsedTime");

            GridViewColumnBuilder.ConfigureColumn(Column6, "TimeOfDay", 45, "TimeOfDay");

            GridViewColumnBuilder.ConfigureColumn(Column7, "TimeOfDayOs", 45, "TimeOfDayOs");

            Grid.ColumnHeadersVisible = true;
            Grid.RowHeadersVisible = false;
            Grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Grid.ReadOnly = true;
            Grid.AutoGenerateColumns = false;
            Grid.AllowUserToResizeRows = false;
            Grid.SelectionChanged += (s, e) => Grid.ClearSelection();
        }

        public override void SetDataSource<TModel>(IList<TModel> models)
        {
            if (Grid.DataSource == null)
            {
                if (Settings.MaxRows.HasValue)
                    models = models.
                        Take(Settings.MaxRows.Value).
                        ToList();

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
                    DataRow[] foundRows = _dataTable.Select($"State = {model.State} AND LapNumber = {model.LapNumber}");

                    if (foundRows != null && foundRows.Length > 0)
                    {
                        foundRows[0]["Comment"] = model.Comment;
                        foundRows[0]["Beneficiary"] = model.Beneficiary;
                    }
                    else
                    {
                        var state = model.State == 1 ? "Green" :
                             model.State == 2 ? "Yellow" :
                             model.State == 3 ? "Red" :
                             model.State == 4 ? "White" :
                             model.State == 8 ? "Hot" :
                             model.State == 9 ? "Cold" :
                             "";

                        var comment = String.IsNullOrEmpty(model.Comment) ? " " : model.Comment;
                        var beneficiary = String.IsNullOrEmpty(model.Beneficiary) ? " " : model.Beneficiary;

                        var dr = _dataTable.NewRow();

                        dr["State"] = int.Parse(model.State.ToString());
                        dr["LapNumber"] = int.Parse(model.LapNumber.ToString());
                        dr["Comment"] = comment;
                        dr["Beneficiary"] = beneficiary;
                        dr["ElapsedTime"] = float.Parse(model.ElapsedTime.ToString());
                        dr["TimeOfDay"] = float.Parse(model.TimeOfDay.ToString());
                        dr["TimeOfDayOs"] = model.TimeOfDayOs.ToString("h:mm tt");

                        _dataTable.Rows.Add(dr);
                    }
                }
            }

            var cautionCountGroup = models.OfType<FlagState>().
               Where(m => m.State == 2).
               GroupBy(m => $"{m.State}-{m.LapNumber}");

            var cautionCount = cautionCountGroup.Count();

            int previousState = 0;
            int previousCautionStartLap = 0;
            int runningCautionLapCount = 0;
            foreach (FlagState model in models.OfType<FlagState>())
            {
                if (model.State == 1 && previousState == 0)
                {
                    // green flag to start event
                }
                else if (model.State == 1 && previousState == 2)
                {
                    // green flag after a yellow
                    runningCautionLapCount += (model.LapNumber - previousCautionStartLap);
                }
                else if (model.State == 2 && previousState == 1)
                {
                    // yellow flag after a green
                    previousCautionStartLap = model.LapNumber;
                }
                else if (model.State == 2 && previousState == 3)
                {
                    // yellow flag after a red
                }

                previousState = model.State;
            }

            var cautionCountLabel = cautionCount == 1 ? "Caution" : "Cautions";

            this.TitleLabel.Text = $"Flags     {cautionCount} {cautionCountLabel} for {runningCautionLapCount} Laps";

            if (Grid.RowCount > 0)
                Grid.FirstDisplayedScrollingRowIndex = Grid.RowCount - 1;
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var grid = sender as DataGridView;

            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                var row = grid.Rows[i];

                if (row.Cells[0].Value != null)
                {
                    if (int.Parse(row.Cells[0].Value.ToString()) == 1)
                    {
                        row.Cells[1].Style.BackColor = Color.Green;
                    }
                    else if (int.Parse(row.Cells[0].Value.ToString()) == 2)
                    {
                        row.Cells[1].Style.BackColor = Color.Gold;
                    }
                    else if (int.Parse(row.Cells[0].Value.ToString()) == 3)
                    {
                        row.Cells[1].Style.BackColor = Color.Red;
                    }
                    else if (int.Parse(row.Cells[0].Value.ToString()) == 4)
                    {
                        row.Cells[1].Style.BackColor = Color.White;
                    }
                    else if (int.Parse(row.Cells[0].Value.ToString()) == 8)
                    {
                        row.Cells[1].Style.BackColor = Color.Orange;
                    }
                    else if (int.Parse(row.Cells[0].Value.ToString()) == 9)
                    {
                        row.Cells[1].Style.BackColor = Color.CornflowerBlue;
                    }
                    else
                    {
                        row.Cells[1].Style.BackColor = row.Cells[0].Style.BackColor;
                    }
                }
                else
                {
                    row.Cells[1].Style.BackColor = row.Cells[0].Style.BackColor;
                }
            }
        }
    }
}
