using System.Collections.Generic;

namespace rNascar23TestApp.Screens
{
    public interface IScreenService
    {
        IList<ScreenDefinition> GetScreenDefinitions();
        void SaveScreenDefinitions(IList<ScreenDefinition> screenDefinitions);
    }
}