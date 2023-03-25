using Microsoft.Extensions.DependencyInjection;
using rNascar23.Points.Ports;
using rNascar23.Service.Points.Adapters;

namespace rNascar23.Service.Points
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPoints(this IServiceCollection services)
        {
            services
                .AddTransient<IPointsRepository, PointsRepository>();

            return services;
        }
    }
}
