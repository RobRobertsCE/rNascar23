namespace rNascar23TestApp
{
    public class RaceVehicleView
    {
        public int RunningPosition { get; set; }
        public string Driver { get; set; }
        public string CarNumber { get; set; }
        public string CarManufacturer { get; set; }
        public int LapsCompleted { get; set; }
        public float DeltaLeader { get; set; }
        public float DeltaNext { get; set; }
        public bool IsOnTrack { get; set; }
        public float LastLap { get; set; }
        public float BestLap { get; set; }
        public int BestLapNumber { get; set; }
        public int LastPit { get; set; }
    }
}

