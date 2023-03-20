using Microsoft.Extensions.DependencyInjection;
using rNascar23.Data.Flags.Ports;
using rNascar23.Service.Flags.Adapters;

namespace rNascar23.Service.Flags
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFlagState(this IServiceCollection services)
        {
            services
                .AddTransient<IFlagStateRepository, FlagStateRepository>();

            return services;
        }
    }
}
