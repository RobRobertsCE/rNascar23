using Microsoft.Extensions.DependencyInjection;
using rNascar23.DriverStatistics.Ports;
using rNascar23.Service.DriverStatistics.Adapters;
using rNascar23.Service.LiveFeeds;

namespace rNascar23.Service.DriverStatistics
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDriverStatistics(this IServiceCollection services)
        {
            services
                .AddSchedules()
                .AddTransient<IWeekendFeedRepository, WeekendFeedRepository>()
                .AddTransient<IDriverInfoRepository, DriverInfoRepository>()
                .AddTransient<IDriverStatisticsRepository, DriverStatisticsRepository>();

            return services;
        }
    }
}
