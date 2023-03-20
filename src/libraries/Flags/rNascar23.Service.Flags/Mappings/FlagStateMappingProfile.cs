using AutoMapper;
using rNascar23.Flags.Models;
using rNascar23.Service.Flags.Data.Models;

namespace rNascar23.Service.Flags.Mappings
{
    internal class FlagStateMappingProfile : Profile
    {
        public FlagStateMappingProfile()
        {
            CreateMap<FlagStateModel, FlagState>()
           .ForMember(m => m.LapNumber, opts => opts.MapFrom(src => src.lap_number))
           .ForMember(m => m.State, opts => opts.MapFrom(src => src.flag_state))
           .ForMember(m => m.ElapsedTime, opts => opts.MapFrom(src => src.elapsed_time))
           .ForMember(m => m.Comment, opts => opts.MapFrom(src => src.comment))
           .ForMember(m => m.Beneficiary, opts => opts.MapFrom(src => src.beneficiary))
           .ForMember(m => m.TimeOfDay, opts => opts.MapFrom(src => src.time_of_day))
           .ForMember(m => m.TimeOfDayOs, opts => opts.MapFrom(src => src.time_of_day_os));
        }
    }
}
