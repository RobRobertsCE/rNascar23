using AutoMapper;
using rNascar23.Schedules.Models;
using rNascar23.Service.RaceLists.Data.Models;

namespace rNascar23.Service.RaceLists.Mappings
{
    internal class SchedulesMappingProfile : Profile
    {
        public SchedulesMappingProfile()
        {
            CreateMap<SeriesEventModel, SeriesSchedules>()
                .ForMember(m => m.CupSeries, opts => opts.MapFrom(src => src.series_1))
                .ForMember(m => m.XfinitySeries, opts => opts.MapFrom(src => src.series_2))
                .ForMember(m => m.TruckSeries, opts => opts.MapFrom(src => src.series_3))
                .ReverseMap();

            CreateMap<SeriesModel, SeriesEvent>()
                .ForMember(m => m.ActualDistance, opts => opts.MapFrom(src => src.actual_distance))
                .ForMember(m => m.ActualLaps, opts => opts.MapFrom(src => src.actual_laps))
                .ForMember(m => m.Attendance, opts => opts.MapFrom(src => src.attendance))
                .ForMember(m => m.AverageSpeed, opts => opts.MapFrom(src => src.average_speed))
                .ForMember(m => m.DateScheduled, opts => opts.MapFrom(src => src.date_scheduled))
                .ForMember(m => m.HasQualifying, opts => opts.MapFrom(src => src.has_qualifying))
                .ForMember(m => m.Infractions, opts => opts.MapFrom(src => src.infractions))
                .ForMember(m => m.InspectionComplete, opts => opts.MapFrom(src => src.inspection_complete))
                .ForMember(m => m.IsQualifyingRace, opts => opts.MapFrom(src => src.is_qualifying_race))
                .ForMember(m => m.MarginOfVictory, opts => opts.MapFrom(src => src.margin_of_victory))
                .ForMember(m => m.MasterRaceId, opts => opts.MapFrom(src => src.master_race_id))
                .ForMember(m => m.NumberOfCarsInField, opts => opts.MapFrom(src => src.number_of_cars_in_field))
                .ForMember(m => m.NumberOfCautionLaps, opts => opts.MapFrom(src => src.number_of_caution_laps))
                .ForMember(m => m.NumberOfCautions, opts => opts.MapFrom(src => src.number_of_cautions))
                .ForMember(m => m.NumberOfLeadChanges, opts => opts.MapFrom(src => src.number_of_lead_changes))
                .ForMember(m => m.NumberOfLeaders, opts => opts.MapFrom(src => src.number_of_leaders))
                .ForMember(m => m.PlayoffRound, opts => opts.MapFrom(src => src.playoff_round))
                .ForMember(m => m.PoleWinnerDriverId, opts => opts.MapFrom(src => src.pole_winner_driver_id))
                .ForMember(m => m.PoleWinnerLapTime, opts => opts.MapFrom(src => src.pole_winner_laptime))
                .ForMember(m => m.PoleWinnerSpeed, opts => opts.MapFrom(src => src.pole_winner_speed))
                .ForMember(m => m.QualifyingDate, opts => opts.MapFrom(src => src.qualifying_date))
                .ForMember(m => m.QualifyingRaceId, opts => opts.MapFrom(src => src.qualifying_race_id))
                .ForMember(m => m.QualifyingRaceNumber, opts => opts.MapFrom(src => src.qualifying_race_no))
                .ForMember(m => m.RaceComments, opts => opts.MapFrom(src => src.race_comments))
                .ForMember(m => m.RaceDate, opts => opts.MapFrom(src => src.race_date))
                .ForMember(m => m.RaceId, opts => opts.MapFrom(src => src.race_id))
                .ForMember(m => m.RaceName, opts => opts.MapFrom(src => src.race_name))
                .ForMember(m => m.RacePurse, opts => opts.MapFrom(src => src.race_purse))
                .ForMember(m => m.RaceSeason, opts => opts.MapFrom(src => src.race_season))
                .ForMember(m => m.RaceTypeId, opts => opts.MapFrom(src => src.race_type_id))
                .ForMember(m => m.RadioBroadcaster, opts => opts.MapFrom(src => src.radio_broadcaster))
                .ForMember(m => m.RestrictorPlate, opts => opts.MapFrom(src => src.restrictor_plate))
                .ForMember(m => m.SatelliteRadioBroadcaster, opts => opts.MapFrom(src => src.satellite_radio_broadcaster))
                .ForMember(m => m.Schedule, opts => opts.MapFrom(src => src.schedule))
                .ForMember(m => m.ScheduledDistance, opts => opts.MapFrom(src => src.scheduled_distance))
                .ForMember(m => m.ScheduledLaps, opts => opts.MapFrom(src => src.scheduled_laps))
                .ForMember(m => m.SeriesId, opts => opts.MapFrom(src => src.series_id))
                .ForMember(m => m.SeriesName, opts => opts.MapFrom(src => src.series_id == 1 ?
                                                                   "Cup Series" : src.series_id == 2 ?
                                                                   "Xfinity Series" : src.series_id == 3 ?
                                                                   "Craftsman Truck Series" : "Unknown"))
                .ForMember(m => m.Stage1Laps, opts => opts.MapFrom(src => src.stage_1_laps))
                .ForMember(m => m.Stage2Laps, opts => opts.MapFrom(src => src.stage_2_laps))
                .ForMember(m => m.Stage3Laps, opts => opts.MapFrom(src => src.stage_3_laps))
                .ForMember(m => m.TelevisionBroadcaster, opts => opts.MapFrom(src => src.television_broadcaster))
                .ForMember(m => m.TotalRaceTime, opts => opts.MapFrom(src => src.total_race_time))
                .ForMember(m => m.TrackId, opts => opts.MapFrom(src => src.track_id))
                .ForMember(m => m.TrackName, opts => opts.MapFrom(src => src.track_name))
                .ForMember(m => m.TuneInDate, opts => opts.MapFrom(src => src.tunein_date))
                .ForMember(m => m.WinnerDriverId, opts => opts.MapFrom(src => src.winner_driver_id));

            CreateMap<ScheduleModel, Schedule>()
                .ForMember(m => m.EventName, opts => opts.MapFrom(src => src.event_name))
                .ForMember(m => m.RunType, opts => opts.MapFrom(src => src.run_type))
                .ForMember(m => m.StartTimeUtc, opts => opts.MapFrom(src => src.start_time_utc))
                .ForMember(m => m.Notes, opts => opts.MapFrom(src => src.notes))
                .ReverseMap();
        }
    }
}