using AutoMapper;
using rNascar23.Service.DriverStatistics.Data.Models;
using rNascar23.DriverStatistics.Models;

namespace rNascar23.Service.DriverStatistics.Mappings
{
    internal class DriverMappingProfile : Profile
    {
        public DriverMappingProfile()
        {
            CreateMap<EventStatsModel, EventStats>();

            CreateMap<DriverModel, Driver>();
        }
    }
}
