using System.Drawing;
using System.Windows.Forms;

namespace rNascar23TestApp.CustomViews
{
    internal static class GridStyleHelper
    {
        public static void ApplyGridStyleSettings(DataGridView grid, GridStyleSettings styleSettings)
        {
            if (styleSettings == null)
                return;

            if (grid == null)
                return;

            grid.EnableHeadersVisualStyles = false;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            if (styleSettings.GridStyle != null)
            {
                if (styleSettings.GridStyle.BackColor.HasValue)
                    grid.BackgroundColor = Color.FromArgb(styleSettings.GridStyle.BackColor.Value);

                if (styleSettings.GridStyle.LineColor.HasValue)
                    grid.GridColor = Color.FromArgb(styleSettings.GridStyle.LineColor.Value);
            }

            if (styleSettings.RowStyle != null)
            {
                if (styleSettings.RowStyle.BackColor.HasValue)
                    grid.RowsDefaultCellStyle.BackColor = Color.FromArgb(styleSettings.RowStyle.BackColor.Value);

                if (styleSettings.RowStyle.ForeColor.HasValue)
                    grid.RowsDefaultCellStyle.ForeColor = Color.FromArgb(styleSettings.RowStyle.ForeColor.Value);

                if (styleSettings.RowStyle.Font != null)
                    grid.RowsDefaultCellStyle.Font = new Font(
                        styleSettings.RowStyle.Font.FontName,
                        (float)styleSettings.RowStyle.Font.FontSize.Value,
                        (FontStyle)styleSettings.RowStyle.Font.FontStyle.Value);
            }

            if (styleSettings.HeaderStyle != null)
            {
                if (styleSettings.HeaderStyle.BackColor.HasValue)
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(styleSettings.HeaderStyle.BackColor.Value);

                if (styleSettings.HeaderStyle.ForeColor.HasValue)
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(styleSettings.HeaderStyle.ForeColor.Value);

                if (styleSettings.HeaderStyle.Font != null)
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font(
                        styleSettings.HeaderStyle.Font.FontName,
                        (float)styleSettings.HeaderStyle.Font.FontSize.Value,
                        (FontStyle)styleSettings.HeaderStyle.Font.FontStyle.Value);
            }

            if (styleSettings.AlternatingRowStyle != null)
            {
                if (styleSettings.AlternatingRowStyle.BackColor.HasValue)
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(styleSettings.AlternatingRowStyle.BackColor.Value);

                if (styleSettings.AlternatingRowStyle.ForeColor.HasValue)
                    grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(styleSettings.AlternatingRowStyle.ForeColor.Value);

                if (styleSettings.AlternatingRowStyle.Font != null)
                    grid.AlternatingRowsDefaultCellStyle.Font = new Font(
                        styleSettings.AlternatingRowStyle.Font.FontName,
                        (float)styleSettings.AlternatingRowStyle.Font.FontSize.Value,
                        (FontStyle)styleSettings.AlternatingRowStyle.Font.FontStyle.Value);
            }

            if (styleSettings.SelectedRowStyle != null)
            {
                if (styleSettings.SelectedRowStyle.BackColor.HasValue)
                    grid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(styleSettings.SelectedRowStyle.BackColor.Value);

                if (styleSettings.SelectedRowStyle.ForeColor.HasValue)
                    grid.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(styleSettings.SelectedRowStyle.ForeColor.Value);

                if (styleSettings.SelectedRowStyle.BackColor.HasValue)
                    grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(styleSettings.SelectedRowStyle.BackColor.Value);

                if (styleSettings.SelectedRowStyle.ForeColor.HasValue)
                    grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(styleSettings.SelectedRowStyle.ForeColor.Value);
            }
        }
    }
}
