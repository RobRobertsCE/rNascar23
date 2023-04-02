using System.Drawing;
using static rNascar23TestApp.CustomViews.GridRowStyle;

namespace rNascar23TestApp.CustomViews
{
    public class GridStyle
    {
        public int? BackColor { get; set; }
        public int? LineColor { get; set; }

        public GridStyle()
        {

        }

        public GridStyle(int backColor, int lineColor)
        {
            BackColor = backColor;
            LineColor = lineColor;
        }

        public static GridStyle GetDefaultGridStyle()
        {
            return new GridStyle(Color.Black.ToArgb(), Color.DimGray.ToArgb());
        }
    }
}
