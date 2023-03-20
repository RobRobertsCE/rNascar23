using AutoMapper;
using rNascar23.LiveFeeds.Models;
using rNascar23.Service.LiveFeeds.Data.Models;
using System.Collections.Generic;

namespace rNascar23.LiveFeeds.Mappings
{
    internal class LiveFeedMappingProfile : Profile
    {
        public LiveFeedMappingProfile()
        {
            CreateMap<PitStopModel, PitStop>();

            CreateMap<DriverModel, Driver>();

            CreateMap<LapsLedModel, LapsLed>();

            CreateMap<VehicleModel, Vehicle>();
                //.ForMember(m => m.laps_led, opts => opts.MapFrom(src => new LapsLed() {  start_lap = src.laps_led }));

            CreateMap<StageModel, Stage>()
              .ForMember(m => m.Number, opts => opts.MapFrom(src => (long)src.stage_num))
              .ForMember(m => m.FinishAtLap, opts => opts.MapFrom(src => src.finish_at_lap))
              .ForMember(m => m.LapsInStage, opts => opts.MapFrom(src => src.laps_in_stage))
              .ReverseMap();

            CreateMap<LiveFeedModel, LiveFeed>()
               .ForMember(m => m.AverageDifference1To3, opts => opts.MapFrom(src => src.avg_diff_1to3))
               .ForMember(m => m.ElapsedTime, opts => opts.MapFrom(src => src.elapsed_time))
               .ForMember(m => m.FlagState, opts => opts.MapFrom(src => src.flag_state))
               .ForMember(m => m.LapNumber, opts => opts.MapFrom(src => src.lap_number))
               .ForMember(m => m.LapsInRace, opts => opts.MapFrom(src => src.laps_in_race))
               .ForMember(m => m.LapsToGo, opts => opts.MapFrom(src => src.laps_to_go))
               .ForMember(m => m.NumberOfCautionLaps, opts => opts.MapFrom(src => src.number_of_caution_laps))
               .ForMember(m => m.NumberOfCautionSegments, opts => opts.MapFrom(src => src.number_of_caution_segments))
               .ForMember(m => m.NumberOfLeadChanges, opts => opts.MapFrom(src => src.number_of_lead_changes))
               .ForMember(m => m.NumberOfLeaders, opts => opts.MapFrom(src => src.number_of_leaders))
               .ForMember(m => m.RaceId, opts => opts.MapFrom(src => src.race_id))
               .ForMember(m => m.RunId, opts => opts.MapFrom(src => src.run_id))
               .ForMember(m => m.RunName, opts => opts.MapFrom(src => src.run_name))
               .ForMember(m => m.RunType, opts => opts.MapFrom(src => src.run_type))
               .ForMember(m => m.SeriesId, opts => opts.MapFrom(src => src.series_id))
               .ForMember(m => m.Stage, opts => opts.MapFrom(src => src.stage))
               .ForMember(m => m.TimeOfDay, opts => opts.MapFrom(src => src.time_of_day))
               .ForMember(m => m.TimeOfDayOs, opts => opts.MapFrom(src => src.time_of_day_os))
               .ForMember(m => m.TrackId, opts => opts.MapFrom(src => src.track_id))
               .ForMember(m => m.TrackLength, opts => opts.MapFrom(src => src.track_length))
               .ForMember(m => m.TrackName, opts => opts.MapFrom(src => src.track_name))
               .ForMember(m => m.Vehicles, opts => opts.MapFrom(src => src.vehicles))
               .ReverseMap();
        }
    }
}
