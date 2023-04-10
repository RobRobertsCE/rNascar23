namespace rNascar23.LoopData.Models
{
    public class Driver
    {
        public string DriverName { get; set; }
        public int DriverId { get; set; }
        public int StartPosition { get; set; }
        public int MidPosition { get; set; }
        public int Position { get; set; }
        public int ClosingPosition { get; set; }
        public int ClosingLapsDifference { get; set; }
        public int BestPosition { get; set; }
        public int WorstPosition { get; set; }
        public float AveragePosition { get; set; }
        public int PassesGreenFlag { get; set; }
        public int PassingDifference { get; set; }
        public int PassedGreenFlag { get; set; }
        public int QualityPasses { get; set; }
        public int FastestLaps { get; set; }
        public int Top15Laps { get; set; }
        public int LeadLaps { get; set; }
        public int Laps { get; set; }
        public float Rating { get; set; }
    }
}
