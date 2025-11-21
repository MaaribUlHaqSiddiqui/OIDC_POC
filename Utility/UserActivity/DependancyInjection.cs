using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActivity
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddUserActivityLogging(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddSingleton<ConnectionMultiplexer>(x =>
            {
                var multiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions()
                {
                    EndPoints = { "redis_container:6379" }
                });

                return multiplexer;
            });
            services.AddSingleton<IUserActivity, UserActivity>();

            return services;
        }
    }
}
