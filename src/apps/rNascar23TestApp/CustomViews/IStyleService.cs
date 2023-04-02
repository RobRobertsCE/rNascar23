using System.Collections.Generic;

namespace rNascar23TestApp.CustomViews
{
    public interface IStyleService
    {
        IList<GridStyleSettings> GetStyles();
        GridStyleSettings GetStyle(string styleName);
        void SaveStyles(IList<GridStyleSettings> customStyleSettings);
    }
}