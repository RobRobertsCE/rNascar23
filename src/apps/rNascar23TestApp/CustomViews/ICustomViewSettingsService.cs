using System.Collections.Generic;

namespace rNascar23.CustomViews
{
    public interface ICustomViewSettingsService
    {
        IList<GridSettings> GetCustomViewSettings();
        void SaveCustomViewSettings(IList<GridSettings> customGridSettings);
    }
}