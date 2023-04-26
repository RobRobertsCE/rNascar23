using rNascar23.Sdk.Common;
using System.Collections.Generic;

namespace rNascar23.ViewModels
{
    internal class LapStateViewModel
    {
        public int Stage1Laps { get; set; }
        public int Stage2Laps { get; set; }
        public int Stage3Laps { get; set; }
        public IList<LapSegment> LapSegments { get; set; } = new List<LapSegment>();

        public class LapSegment
        {
            public int StartLapNumber { get; set; }
            public int Laps { get; set; }
            public int Stage { get; set; }
            public FlagColors FlagState { get; set; }

            public override string ToString()
            {
                return $"[{Stage}] {StartLapNumber} ({Laps})-{FlagState}";
            }
        }
    }
}
