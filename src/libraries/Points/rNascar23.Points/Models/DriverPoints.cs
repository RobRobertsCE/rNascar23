namespace rNascar23.Points.Models
{
    public class DriverPoints
    {
        public int BonusPoints { get; set; }
        public string CarNumber { get; set; }
        public int DeltaLeader { get; set; }
        public int DeltaNext { get; set; }
        public string FirstName { get; set; }
        public int DriverId { get; set; }
        public bool IsInChase { get; set; }
        public bool IsPointsEligible { get; set; }
        public bool IsRookie { get; set; }
        public string LastName { get; set; }
        public int MembershipId { get; set; }
        public int Points { get; set; }
        public int PointsPosition { get; set; }
        public int PointsEarnedThisRace { get; set; }
        public int Stage1Points { get; set; }
        public bool Stage1Winner { get; set; }
        public int Stage2Points { get; set; }
        public bool Stage2Winner { get; set; }
        public int Stage3Points { get; set; }
        public bool Stage3Winner { get; set; }
        public int Wins { get; set; }
        public int Top5 { get; set; }
        public int Top10 { get; set; }
        public int Poles { get; set; }
        public int SeriesId { get; set; }
        public int RaceId { get; set; }
        public int RunId { get; set; }

        public string Driver
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }

}
