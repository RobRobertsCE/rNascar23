using Microsoft.Extensions.DependencyInjection;
using rNascar23.LoopData.Ports;
using rNascar23.Service.LiveFeeds;
using rNascar23.Service.LoopData.Adapters;

namespace rNascar23.Service.LoopData
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoopData(this IServiceCollection services)
        {
            services
                .AddSchedules()
                .AddTransient<IDriverInfoRepository, DriverInfoRepository>()
                .AddTransient<ILoopDataRepository, LoopDataRepository>();

            return services;
        }
    }
}
