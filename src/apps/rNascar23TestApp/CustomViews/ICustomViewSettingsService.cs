using System.Collections.Generic;

namespace rNascar23TestApp.CustomViews
{
    public interface ICustomViewSettingsService
    {
        IList<GridSettings> GetCustomViewSettings();
        void SaveCustomViewSettings(IList<GridSettings> customGridSettings);
    }
}