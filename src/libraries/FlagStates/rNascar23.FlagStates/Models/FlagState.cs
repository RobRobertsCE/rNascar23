using System;

namespace rNascar23.Flags.Models
{
    public class FlagState
    {
        public int LapNumber { get; set; }
        public int State { get; set; }
        public float ElapsedTime { get; set; }
        public string Comment { get; set; }
        public string Beneficiary { get; set; }
        public float TimeOfDay { get; set; }
        public DateTime TimeOfDayOs { get; set; }
    }
}
