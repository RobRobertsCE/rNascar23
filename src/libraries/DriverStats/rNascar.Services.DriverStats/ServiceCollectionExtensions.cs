using Microsoft.Extensions.DependencyInjection;
using rNascar23.Service.DriverStatistics.Adapters;
using rNascar23.DriverStatistics.Ports;

namespace rNascar23.Service.DriverStatistics
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDriverStatistics(this IServiceCollection services)
        {
            services
                .AddTransient<IDriverStatisticsRepository, DriverStatisticsRepository>();

            return services;
        }
    }
}
