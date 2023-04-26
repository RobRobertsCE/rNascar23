using rNascar23.Sdk.Common;

namespace rNascar23.ViewModels
{
    public class DriverPitStopViewModel
    {
        public int CarNumber { get; set; }
        public string DriverName { get; set; }
        public int RunningPosition { get; set; }
        public int PitOnLap { get; set; }
        public float PitStopTime { get; set; }
        public int PositionIn { get; set; }
        public int PositionOut { get; set; }
        public int PositionDelta { get; set; }
        public PitStopChanges Changes { get; set; }
    }
}
