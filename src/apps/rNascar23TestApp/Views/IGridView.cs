using rNascar23TestApp.CustomViews;
using System.Collections.Generic;

namespace rNascar23TestApp.Views
{
    public interface IGridView<T> : IGridView
    {
        IList<T> Data { get; set; }
    }

    public interface IGridView
    {
        bool IsCustomGrid { get; set; }
        string CustomGridName { get; set; }
        string Description { get; set; }
        GridSettings Settings { get; set; }
    }
}
