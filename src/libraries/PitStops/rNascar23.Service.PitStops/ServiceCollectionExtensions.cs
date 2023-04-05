using Microsoft.Extensions.DependencyInjection;
using rNascar23.PitStops.Ports;
using rNascar23.Service.PitStops.Adapters;

namespace rNascar23.Service.PitStops
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPitStops(this IServiceCollection services)
        {
            services
                .AddTransient<IPitStopsRepository, PitStopsRepository>();

            return services;
        }
    }
}