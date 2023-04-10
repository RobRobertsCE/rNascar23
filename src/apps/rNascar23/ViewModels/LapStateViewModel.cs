using System.Collections.Generic;

namespace rNascar23.ViewModels
{
    internal class LapStateViewModel
    {
        public enum FlagState
        {
            None = 0,
            Green = 1,
            Yellow = 2,
            Red = 3,
            White = 4,
            Checkered = 5,
            Hot = 8,
            Cold = 9
        }

        public int Stage1Laps { get; set; }
        public int Stage2Laps { get; set; }
        public int Stage3Laps { get; set; }
        public IList<LapSegment> LapSegments { get; set; } = new List<LapSegment>();

        public class LapSegment
        {
            public int StartLapNumber { get; set; }
            public int Laps { get; set; }
            public int Stage { get; set; }
            public FlagState FlagState { get; set; }

            public override string ToString()
            {
                return $"[{Stage}] {StartLapNumber} ({Laps})-{FlagState}";
            }
        }
    }
}
