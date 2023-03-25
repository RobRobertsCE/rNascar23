using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace rNascar23TestApp.CustomViews
{
    public class GridSettings
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ApiSources ApiSource { get; set; }
        public string Title { get; set; }
        public GridLocations Location { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public int TitleForeColorInt { get; set; } = -16777216;
        public int TitleBackColorInt { get; set; } = -1;
        public int GridWidth { get; set; } = 250;
        public bool HideRowSelector { get; set; } = false;
        public int SortOrder { get; set; } = 0;
        public string SortOrderField { get; set; }

        [JsonIgnore()]
        public Color TitleForeColor
        {
            get
            {
                return Color.FromArgb(TitleForeColorInt);
            }
            set
            {
                TitleForeColorInt = value.ToArgb();
            }
        }

        [JsonIgnore()]
        public Color TitleBackColor
        {
            get
            {
                return Color.FromArgb(TitleBackColorInt);
            }
            set
            {
                TitleBackColorInt = value.ToArgb();
            }
        }

        public IList<GridColumnSettings> Columns { get; set; } = new List<GridColumnSettings>();
    }
}
