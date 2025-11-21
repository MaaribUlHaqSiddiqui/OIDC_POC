using EndpointGRPC.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Helpers
{
    public static class DependencyInjection
    {
        
        public static IServiceCollection AddEndpointGRPC(this IServiceCollection services, IConfiguration configuration, string ApiBaseName, Assembly assembly, Type iFeature)
        {
            services.AddGrpc();
            services.AddTransient<ActionExposedImpl>(serviceProvider =>
            {
                return new ActionExposedImpl(ApiBaseName, assembly, iFeature);
            });

            return services;
        }

        
    }
}
