using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23TestApp.Views
{
    internal static class GridViewColumnBuilder
    {
        public static void ConfigureColumn(
            DataGridViewTextBoxColumn column,
            string propertyName,
            int? width = 125,
            string headerText = "",
            string cellFormat = "")
        {
            column.HeaderText = headerText;
            column.Name = propertyName;
            column.DataPropertyName = propertyName;

            if (width.HasValue)
            {
                column.Width = width.Value;
                column.Visible = true;
            }
            else
            {
                column.Width = 0;
                column.Visible = false;
            }

            if (!String.IsNullOrEmpty(cellFormat))
            {
                column.DefaultCellStyle.Format = cellFormat;
            }
        }
    }
}
