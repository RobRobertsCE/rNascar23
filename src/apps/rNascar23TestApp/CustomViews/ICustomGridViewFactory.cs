using rNascar23TestApp.Views;
using System.Collections.Generic;

namespace rNascar23TestApp.CustomViews
{
    public interface ICustomGridViewFactory
    {
        GridView GetCustomGridView(GridSettings customGridSettings);
        IList<GridView> GetCustomGridViews(IList<GridSettings> customGridSettings);
    }
}