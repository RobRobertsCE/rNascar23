using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using rNascar23.Media.Ports;

namespace rNascar23.Services.Media
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMedia(this IServiceCollection services)
        {
            services
                .AddSingleton<IMediaRepository, MediaRepository>();

            return services;
        }
    }
}
