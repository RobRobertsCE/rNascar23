using System.Drawing;

namespace rNascar23.CustomViews
{
    public partial class GridRowStyle
    {
        public int? ForeColor { get; set; }
        public int? BackColor { get; set; }
        public GridFont Font { get; set; } = GridFont.GetDefaultGridFont();

        public GridRowStyle()
        {

        }

        public GridRowStyle(int backColor, int foreColor)
        {
            BackColor = backColor;
            ForeColor = foreColor;
        }

        public static GridRowStyle GetDefaultGridRowStyle()
        {
            return new GridRowStyle(Color.White.ToArgb(), Color.Black.ToArgb());
        }
        public static GridRowStyle GetDefaultAlternateGridRowStyle()
        {
            return new GridRowStyle(Color.WhiteSmoke.ToArgb(), Color.Black.ToArgb());
        }
        public static GridRowStyle GetDefaultSelectedGridRowStyle()
        {
            return new GridRowStyle(Color.SkyBlue.ToArgb(), Color.Black.ToArgb());
        }
        public static GridRowStyle GetDefaultHeaderGridRowStyle()
        {
            return new GridRowStyle(Color.WhiteSmoke.ToArgb(), Color.Black.ToArgb());
        }
    }
}
