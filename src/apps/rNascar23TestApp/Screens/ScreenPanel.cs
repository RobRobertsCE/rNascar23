using System.Collections.Generic;
using System.Windows.Forms;
using static rNascar23TestApp.Dialogs.ScreenEditor;

namespace rNascar23TestApp.Screens
{
    public class ScreenPanel
    {
        public string Name { get; set; }
        public int DisplayIndex { get; set; }
        public DockStyle Dock { get; set; }
        public float Size { get; set; }
        public IList<ScreenPanelGridDefinition> GridViews { get; set; } = new List<ScreenPanelGridDefinition>();
    }
}
