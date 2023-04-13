using rNascar23.ViewModels;
using System.Collections.Generic;
using static rNascar23.Dialogs.ScreenEditor;

namespace rNascar23.Screens
{
    public class ScreenDefinition : INamedItem
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public int DisplayIndex { get; set; }
        public bool DisplayEventTitle { get; set; } = true;
        public bool DisplayFlagStatus { get; set; } = true;
        public bool DisplayGreenYellowLapIndicator { get; set; } = true;
        public string Style { get; set; }
        public IList<ScreenPanel> Panels { get; set; } = new List<ScreenPanel>();
    }
}
