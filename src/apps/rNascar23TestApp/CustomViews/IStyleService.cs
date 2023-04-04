using System.Collections.Generic;

namespace rNascar23.CustomViews
{
    public interface IStyleService
    {
        IList<GridStyleSettings> GetStyles();
        GridStyleSettings GetStyle(string styleName);
        void SaveStyles(IList<GridStyleSettings> customStyleSettings);
    }
}