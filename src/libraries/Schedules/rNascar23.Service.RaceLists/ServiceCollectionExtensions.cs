using Microsoft.Extensions.DependencyInjection;
using rNascar23.Schedules.Ports;
using rNascar23.Service.RaceLists.Adapters;

namespace rNascar23.Service.LiveFeeds
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSchedules(this IServiceCollection services)
        {
            services
                .AddTransient<ISchedulesRepository, SchedulesRepository>();

            return services;
        }
    }
}
