using AutoMapper;
using rNascar23.PitStops.Models;
using rNascar23.Service.PitStops.Data.Models;

namespace rNascar23.Service.PitStops.Mappings
{
    internal class PitStopMappingProfile : Profile
    {
        public PitStopMappingProfile()
        {
            CreateMap<PitStopModel, PitStop>();
        }
    }
}
