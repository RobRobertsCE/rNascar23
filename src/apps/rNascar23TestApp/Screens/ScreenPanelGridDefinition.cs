using System.Windows.Forms;

namespace rNascar23.Screens
{
    public class ScreenPanelGridDefinition
    {
        public string Name { get; set; }
        public string ScreenName
        {
            get
            {
                if (Name.Contains("_"))
                {
                    var sections = Name.Split('_');
                    if (sections.Length == 3)
                        return sections[0];
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;
            }
        }
        public string PanelName
        {
            get
            {
                if (Name.Contains("_"))
                {
                    var sections = Name.Split('_');
                    if (sections.Length == 3)
                        return sections[1];
                    else
                        return sections[0];
                }
                else
                    return string.Empty;
            }
        }
        public string GridName
        {
            get
            {
                if (Name.Contains("_"))
                {
                    var sections = Name.Split('_');
                    if (sections.Length == 3)
                        return sections[2];
                    else
                        return sections[1];
                }
                else
                    return string.Empty;
            }
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public int DisplayIndex { get; set; }
        public DockStyle Dock { get; set; }
        public bool HasSplitter { get; set; }
        public string Style { get; set; }
    }
}
