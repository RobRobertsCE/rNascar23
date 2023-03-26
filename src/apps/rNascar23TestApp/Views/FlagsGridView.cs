using rNascar23.Flags.Models;
using rNascar23.LiveFeeds.Models;
using rNascar23TestApp.CustomViews;
using rNascar23TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23TestApp.Views
{
    public partial class FlagsGridView : UserControl, IGridView<FlagState>
    {
        #region consts

        private const int CautionFlag = 2;

        #endregion

        #region properties

        private IList<FlagState> _data = new List<FlagState>();
        public IList<FlagState> Data
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

        public FlagsGridView()
        {
            InitializeComponent();

            BuildGridView(Grid);

            Settings = new GridSettings()
            {
                ApiSource = ApiSources.Flags,
                SortOrderField = "LapNumber",
                SortOrder = 1
            };

            this.Width = 335;
        }

        #endregion

        #region private

        private void SetDataSource<T>(IList<T> values)
        {            
            var models = BuildViewModels((IList<FlagState>)values);

            var dataTable = GridViewTableBuilder.ToDataTable<CautionFlagViewModel>(models.ToList());

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

        private IList<CautionFlagViewModel> BuildViewModels(IList<FlagState> flagStates)
        {
            IList<CautionFlagViewModel> cautions = new List<CautionFlagViewModel>();

            //foreach (var item in flagStates.Where(f => f.State == CautionFlag).OrderBy(f => f.TimeOfDayOs))
            foreach (var item in flagStates.OrderBy(f => f.ElapsedTime))
            {
                var caution = new CautionFlagViewModel()
                {
                    Flag = item.State == 1 ? "Green" : item.State == 2 ? "Yellow" : item.State == 3 ? "Red" : "Track Closed",
                    LapNumber = item.LapNumber,
                    Comment = item.Comment,
                    Beneficiary = item.Beneficiary,
                };

                cautions.Add(caution);
            }

            return cautions;
        }

        private DataGridView BuildGridView(DataGridView dataGridView)
        {
            DataGridViewTextBoxColumn Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            dataGridView.RowHeadersVisible = false;

            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                Column1,
                Column2,
                Column3,
                Column4,
            });

            dataGridView.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular);

            GridViewColumnBuilder.ConfigureColumn(Column1, "Flag", 50, "Flag");

            GridViewColumnBuilder.ConfigureColumn(Column2, "LapNumber", 50, "Lap");

            GridViewColumnBuilder.ConfigureColumn(Column3, "Comment", 200, "Caution For");

            GridViewColumnBuilder.ConfigureColumn(Column4, "Beneficiary", 85, "Lucky Dog");

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