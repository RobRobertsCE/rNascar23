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

            CreateMap<DriverPointsModel, DriverPoints>()
               .ForMember(m => m.BonusPoints, opts => opts.MapFrom(src => src.bonus_points))
               .ForMember(m => m.CarNumber, opts => opts.MapFrom(src => src.car_number))
               .ForMember(m => m.DeltaLeader, opts => opts.MapFrom(src => src.delta_leader))
               .ForMember(m => m.DeltaNext, opts => opts.MapFrom(src => src.delta_next))
               .ForMember(m => m.DriverId, opts => opts.MapFrom(src => src.driver_id))
               .ForMember(m => m.FirstName, opts => opts.MapFrom(src => src.first_name))
               .ForMember(m => m.IsInChase, opts => opts.MapFrom(src => src.is_in_chase))
               .ForMember(m => m.IsPointsEligible, opts => opts.MapFrom(src => src.is_points_eligible))
               .ForMember(m => m.IsRookie, opts => opts.MapFrom(src => src.is_rookie))
               .ForMember(m => m.LastName, opts => opts.MapFrom(src => src.last_name))
               .ForMember(m => m.MembershipId, opts => opts.MapFrom(src => src.membership_id))
               .ForMember(m => m.Points, opts => opts.MapFrom(src => src.points))
               .ForMember(m => m.PointsEarnedThisRace, opts => opts.MapFrom(src => src.points_earned_this_race))
               .ForMember(m => m.PointsPosition, opts => opts.MapFrom(src => src.points_position))
               .ForMember(m => m.Poles, opts => opts.MapFrom(src => src.poles))
               .ForMember(m => m.RaceId, opts => opts.MapFrom(src => src.race_id))
               .ForMember(m => m.RunId, opts => opts.MapFrom(src => src.run_id))
               .ForMember(m => m.SeriesId, opts => opts.MapFrom(src => src.series_id))
               .ForMember(m => m.Stage1Points, opts => opts.MapFrom(src => src.stage_1_points))
               .ForMember(m => m.Stage1Winner, opts => opts.MapFrom(src => src.stage_1_winner))
               .ForMember(m => m.Stage2Points, opts => opts.MapFrom(src => src.stage_2_points))
               .ForMember(m => m.Stage2Winner, opts => opts.MapFrom(src => src.stage_2_winner))
               .ForMember(m => m.Stage3Points, opts => opts.MapFrom(src => src.stage_3_points))
               .ForMember(m => m.Stage3Winner, opts => opts.MapFrom(src => src.stage_3_winner))
               .ForMember(m => m.Top10, opts => opts.MapFrom(src => src.top_10))
               .ForMember(m => m.Top5, opts => opts.MapFrom(src => src.top_5))
               .ForMember(m => m.Wins, opts => opts.MapFrom(src => src.wins));
        }
    }
}
