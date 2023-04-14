using System;

namespace rNascar23.Schedules.Models
{
    public class SeriesEvent
    {
        public int RaceId { get; set; }
        public int SeriesId { get; set; }
        public string SeriesName { get; set; }
        public int RaceSeason { get; set; }
        public string RaceName { get; set; }
        public int RaceTypeId { get; set; }
        public bool RestrictorPlate { get; set; }
        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public DateTime DateScheduled { get; set; }
        public DateTime RaceDate { get; set; }
        public DateTime QualifyingDate { get; set; }
        public DateTime TuneInDate { get; set; }
        public float ScheduledDistance { get; set; }
        public float ActualDistance { get; set; }
        public int ScheduledLaps { get; set; }
        public int ActualLaps { get; set; }
        public int Stage1Laps { get; set; }
        public int Stage2Laps { get; set; }
        public int Stage3Laps { get; set; }
        public int NumberOfCarsInField { get; set; }
        public int PoleWinnerDriverId { get; set; }
        public float PoleWinnerSpeed { get; set; }
        public int NumberOfLeadChanges { get; set; }
        public int NumberOfLeaders { get; set; }
        public int NumberOfCautions { get; set; }
        public int NumberOfCautionLaps { get; set; }
        public float AverageSpeed { get; set; }
        public string TotalRaceTime { get; set; }
        public string MarginOfVictory { get; set; }
        public float RacePurse { get; set; }
        public string RaceComments { get; set; }
        public int Attendance { get; set; }
        public object[] Infractions { get; set; }
        public Schedule[] Schedule { get; set; }
        public string RadioBroadcaster { get; set; }
        public string TelevisionBroadcaster { get; set; }
        public string SatelliteRadioBroadcaster { get; set; }
        public int MasterRaceId { get; set; }
        public bool InspectionComplete { get; set; }
        public int PlayoffRound { get; set; }
        public bool IsQualifyingRace { get; set; }
        public int QualifyingRaceNumber { get; set; }
        public int QualifyingRaceId { get; set; }
        public bool HasQualifying { get; set; }
        public int? WinnerDriverId { get; set; }
        public object PoleWinnerLapTime { get; set; }
    }
}
