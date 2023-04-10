using rNascar23.Views;
using System.Collections.Generic;

namespace rNascar23.CustomViews
{
    public interface ICustomGridViewFactory
    {
        GridView GetCustomGridView(GridSettings customGridSettings);
        IList<GridView> GetCustomGridViews(IList<GridSettings> customGridSettings);
    }
}