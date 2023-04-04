namespace rNascar23.CustomViews
{
    public partial class GridRowStyle
    {
        public class GridFont
        {
            public string FontName { get; set; }
            public int? FontSize { get; set; }
            public int? FontStyle { get; set; }

            public GridFont()
            {

            }

            public GridFont(string name, int size = 10, int style = 0)
            {
                FontName = name;
                FontSize = size;
                FontStyle = style;
            }

            public static GridFont GetDefaultGridFont()
            {
                return new GridFont("Arial");
            }
        }
    }
}

