using AutoMapper;
using rNascar23.DriverStatistics.Models;
using rNascar23.Service.DriverStatistics.Data.Models;

namespace rNascar23.Service.DriverStatistics.Mappings
{
    internal class WeekendFeedMappingProfile : Profile
    {
        public WeekendFeedMappingProfile()
        {
            CreateMap<WeekendFeedModel, WeekendFeed>();
            CreateMap<Weekend_RaceModel, Weekend_Race>();
            CreateMap<Weekend_RunsModel, Weekend_Runs>();
            CreateMap<ResultModel, Result>();
            CreateMap<Caution_SegmentsModel, Caution_Segments>();
            CreateMap<Race_LeadersModel, Race_Leaders>();
            CreateMap<ScheduleModel, Schedule>();
            CreateMap<Stage_ResultsModel, Stage_Results>();
            CreateMap<Result1Model, Result1>();
            CreateMap<Weekend_RunsModel, Weekend_Runs>(); ;
            CreateMap<Result2Model, Result2>();
        }
    }
}
