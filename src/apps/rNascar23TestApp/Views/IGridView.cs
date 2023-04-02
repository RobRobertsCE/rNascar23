using rNascar23TestApp.CustomViews;
using System.Collections.Generic;
using System.Windows.Forms;

namespace rNascar23TestApp.Views
{
    public interface IGridView<T> : IGridView
    {
        IList<T> Data { get; set; }
    }

    public interface IGridView : IApiDataView
    {
        bool IsCustomGrid { get; set; }
        string CustomGridName { get; set; }
        GridSettings Settings { get; set; }
        DataGridView DataGridView { get; }
    }

    public interface IApiDataView<T> : IApiDataView
    {
        IList<T> Data { get; set; }
    }

    public interface IApiDataView
    {
        ApiSources ApiSource { get; }
        string Title { get; }
        string Description { get; set; }
    }
}

