namespace rNascar23.LapTimes.Models
{
    public class LapAverages
    {
        public string Number { get; set; }
        public int NASCARDriverID { get; set; }
        public string Driver { get; set; }
        public string FullName { get; set; }
        public string Manufacturer { get; set; }
        public string Sponsor { get; set; }
        public float? OverAllAvg { get; set; }
        public int? OverAllAvgRank { get; set; }
        public float? BestLapTime { get; set; }
        public int? BestLapRank { get; set; }
        public float? Con5Lap { get; set; }
        public int? Con5LapRank { get; set; }
        public float? Con10Lap { get; set; }
        public int? Con10LapRank { get; set; }
        public float? Con15Lap { get; set; }
        public int? Con15LapRank { get; set; }
        public float? Con20Lap { get; set; }
        public int? Con20LapRank { get; set; }
        public float? Con25Lap { get; set; }
        public int? Con25LapRank { get; set; }
        public float? Con30Lap { get; set; }
        public int? Con30LapRank { get; set; }
    }
}
