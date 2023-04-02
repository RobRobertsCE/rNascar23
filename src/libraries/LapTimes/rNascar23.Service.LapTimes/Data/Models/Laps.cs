namespace rNascar23.Service.LapTimes.Data.Models
{
    internal class Rootobject
    {
        public Lap[] Laps { get; set; }
        public Flag[] Flags { get; set; }
    }

    public class Lap
    {
        public string Number { get; set; }
        public string FullName { get; set; }
        public string Manufacturer { get; set; }
        public int RunningPos { get; set; }
        public int NASCARDriverID { get; set; }
        public Lap1[] Laps { get; set; }
    }

    public class Lap1
    {
        public int Lap { get; set; }
        public float? LapTime { get; set; }
        public string LapSpeed { get; set; }
        public int RunningPos { get; set; }
    }

    public class Flag
    {
        public int LapsCompleted { get; set; }
        public int FlagState { get; set; }
    }
}
