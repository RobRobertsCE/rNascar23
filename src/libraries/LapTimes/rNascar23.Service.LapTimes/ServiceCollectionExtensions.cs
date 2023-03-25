using Microsoft.Extensions.DependencyInjection;
using rNascar23.LapTimes.Ports;
using rNascar23.Service.LapTimes.Adapters;

namespace rNascar23.Service.LapTimes
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLapTimes(this IServiceCollection services)
        {
            services
                .AddTransient<ILapTimesRepository, LapTimesRepository>()
                .AddTransient<ILapAveragesRepository, LapAveragesRepository>();

            return services;
        }
    }
}
