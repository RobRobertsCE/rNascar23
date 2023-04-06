using rNascar23.ViewModels;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace rNascar23.CustomViews
{
    public class GridSettings : INamedItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; } = true;
        public ApiSources ApiSource { get; set; }
        public string Title { get; set; }
        public GridLocations Location { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public int TitleForeColorInt { get; set; } = -16777216;
        public int TitleBackColorInt { get; set; } = -1;
        public int? GridWidth { get; set; } = 200;
        public int? ViewWidth { get; set; } = 228;
        public int? GridHeight { get; set; } = 250;
        public int? ViewHeight { get; set; } = 228;
        public bool HideRowSelector { get; set; } = false;
        public bool HideColumnHeaders { get; set; } = false;
        public int? MaxRows { get; set; } = 8;
        public int SortOrder { get; set; } = 0;
        public string SortOrderField { get; set; }
        public string Style { get; set; }

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
