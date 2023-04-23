using System;
using System.Collections.Generic;

namespace rNascar23.LiveFeeds.Models
{
    public class LiveFeed
    {
        public int LapNumber { get; set; }
        public int ElapsedTime { get; set; }
        public int FlagState { get; set; }
        public int RaceId { get; set; }
        public int LapsInRace { get; set; }
        public int LapsToGo { get; set; }
        public IList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public int RunId { get; set; }
        public string RunName { get; set; }
        public int SeriesId { get; set; }
        public int TimeOfDay { get; set; }
        public string TimeOfDayOs { get; set; }
        public int TrackId { get; set; }
        public float TrackLength { get; set; }
        public string TrackName { get; set; }
        public int RunType { get; set; }
        public int NumberOfCautionSegments { get; set; }
        public int NumberOfCautionLaps { get; set; }
        public int NumberOfLeadChanges { get; set; }
        public int NumberOfLeaders { get; set; }
        public int AverageDifference1To3 { get; set; }
        public Stage Stage { get; set; }
        public int LastPit { get; set; }
    }
}
