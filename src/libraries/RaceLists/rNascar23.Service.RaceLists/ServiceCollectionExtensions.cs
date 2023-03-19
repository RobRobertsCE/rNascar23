using Microsoft.Extensions.DependencyInjection;
using rNascar23.RaceLists.Ports;
using rNascar23.Service.RaceLists.Adapters;

namespace rNascar23.Service.LiveFeeds
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRaceLists(this IServiceCollection services)
        {
            services
                .AddTransient<IRaceListRepository, RaceListRepository>();

            return services;
        }
    }
}
