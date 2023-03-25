using AutoMapper;
using rNascar23.Points.Models;
using rNascar23.Service.Points.Data.Models;

namespace rNascar23.Service.Points.Mappings
{
    internal class PointsMappingProfile : Profile
    {
        public PointsMappingProfile()
        {
            CreateMap<StageModel, Stage>();

            CreateMap<StagePointsModel, StagePoints>();

            CreateMap<DriverPointsModel, DriverPoints>();
        }
    }
}
