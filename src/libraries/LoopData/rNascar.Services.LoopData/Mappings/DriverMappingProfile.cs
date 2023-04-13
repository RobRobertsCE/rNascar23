using AutoMapper;
using rNascar23.Service.LoopData.Data.Models;
using rNascar23.LoopData.Models;

namespace rNascar23.Service.LoopData.Mappings
{
    internal class DriverMappingProfile : Profile
    {
        public DriverMappingProfile()
        {
            CreateMap<EventStatsModel, EventStats>();

            CreateMap<DriverModel, Driver>()
                .ForMember(m => m.AveragePosition, opts => opts.MapFrom(src => src.avg_ps))
                .ForMember(m => m.BestPosition, opts => opts.MapFrom(src => src.best_ps))
                .ForMember(m => m.ClosingLapsDifference, opts => opts.MapFrom(src => src.closing_laps_diff))
                .ForMember(m => m.ClosingPosition, opts => opts.MapFrom(src => src.closing_ps))
                .ForMember(m => m.DriverId, opts => opts.MapFrom(src => src.driver_id))
                .ForMember(m => m.FastestLaps, opts => opts.MapFrom(src => src.fast_laps))
                .ForMember(m => m.Laps, opts => opts.MapFrom(src => src.laps))
                .ForMember(m => m.LeadLaps, opts => opts.MapFrom(src => src.lead_laps))
                .ForMember(m => m.MidPosition, opts => opts.MapFrom(src => src.mid_ps))
                .ForMember(m => m.PassedGreenFlag, opts => opts.MapFrom(src => src.passed_gf))
                .ForMember(m => m.PassesGreenFlag, opts => opts.MapFrom(src => src.passes_gf))
                .ForMember(m => m.PassingDifference, opts => opts.MapFrom(src => src.passing_diff))
                .ForMember(m => m.Position, opts => opts.MapFrom(src => src.ps))
                .ForMember(m => m.QualityPasses, opts => opts.MapFrom(src => src.quality_passes))
                .ForMember(m => m.Rating, opts => opts.MapFrom(src => src.rating))
                .ForMember(m => m.StartPosition, opts => opts.MapFrom(src => src.start_ps))
                .ForMember(m => m.Top15Laps, opts => opts.MapFrom(src => src.top15_laps))
                .ForMember(m => m.WorstPosition, opts => opts.MapFrom(src => src.worst_ps))
                .ReverseMap();
        }
    }
}
