using rNascar23TestApp.CustomViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Markup;

namespace rNascar23TestApp.Views
{
    public partial class GridView : UserControl, IGridView
    {
        public bool IsCustomGrid { get; set; } = true;
        public string CustomGridName { get; set; }
        public string Description { get; set; }
        public GridSettings Settings { get; set; }

        public GridView()
        {
            InitializeComponent();
        }

        public void SetDataSource<T>(GridViewDataSource<T> source)
        {
            var dataTable = source.ToDataTable();

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

        public void RefreshBindings()
        {
            RefreshBindings(false);
        }
        public void RefreshBindings(bool metadataChanged)
        {
            GridBindingSource.ResetBindings(metadataChanged);
        }
    }
}
