using System;

namespace rNasar23.Common
{
    [Flags()]
    public enum PitStopChanges
    {
        Other = 1,
        LF = 2,
        LR = 4,
        RF = 8,
        RR = 16,
        LeftSide = LF + LR,
        RightSide = RF + RR,
        FourTires = LeftSide + RightSide
    }
}
