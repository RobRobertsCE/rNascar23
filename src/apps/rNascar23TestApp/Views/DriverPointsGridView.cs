﻿using rNascar23.Points.Models;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23TestApp.Views
{
    public partial class DriverPointsGridView : UserControl, IGridView<DriverPoints>
    {
        #region consts

        private const int CautionFlag = 2;

        #endregion

        #region properties

        private IList<DriverPoints> _data = new List<DriverPoints>();
        public IList<DriverPoints> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;

                SetDataSource(_data);
            }
        }
        public string CustomGridName { get; set; }
        public string Description { get; set; }
        public GridSettings Settings { get; set; }
        public bool IsCustomGrid { get; set; }

        #endregion

        #region ctor

        public DriverPointsGridView()
        {
            InitializeComponent();

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.DriverPoints,
                SortOrderField = "Position",
                SortOrder = 1
            };

            this.Height = 310;
        }

        #endregion

        #region private

        private void SetDataSource<T>(IList<T> values)
        {
            var models = BuildViewModels((IList<DriverPoints>)values);

            var dataTable = GridViewTableBuilder.ToDataTable<DriverPointsViewModel>(models.ToList());

            var dataView = new DataView(dataTable);

            GridBindingSource.DataSource = dataView;

            Grid.DataSource = GridBindingSource;

            foreach (DataGridViewTextBoxColumn column in Grid.Columns)
            {
                column.Name = column.DataPropertyName;
            }

            if (!String.IsNullOrEmpty(Settings.SortOrderField))
            {
                var sortDirection = Settings.SortOrder == 0 ? String.Empty :
                    Settings.SortOrder == 1 ? "ASC" :
                    "DESC";

                var sortString = $"{Settings.SortOrderField} {sortDirection}".Trim();

                GridBindingSource.Sort = sortString;
            }
        }

        private IList<DriverPointsViewModel> BuildViewModels(IList<DriverPoints> data)
        {
            return data.
                 Select(p => new DriverPointsViewModel()
                 {
                     Position = p.points_position,
                     Driver = p.Driver,
                     Points = p.points,
                     Bonus = p.bonus_points
                 }).OrderBy(p => p.Position).ToList();
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
                Column4,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            GridViewColumnBuilder.ConfigureColumn(Column1, "Position", 25, "");

            GridViewColumnBuilder.ConfigureColumn(Column2, "Driver", 150, "Driver");

            GridViewColumnBuilder.ConfigureColumn(Column3, "Points", 75, "Points");

            GridViewColumnBuilder.ConfigureColumn(Column4, "Bonus", 75, "Bonus");

            dataGridView.ColumnHeadersVisible = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            dataGridView.AutoGenerateColumns = false;

            return dataGridView;
        }

        private void Grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < Grid.Rows.Count; i++)
            {
                var row = Grid.Rows[i];

                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        #endregion
    }
}