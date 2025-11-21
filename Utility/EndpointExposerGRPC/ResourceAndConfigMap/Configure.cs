

using EndpointGRPC.Server;
using Microsoft.AspNetCore.Builder;

namespace MapConfig
{
    public static class ConfigureEndpointsExposed
    {
        public  static void MapEndpointsExposed(this WebApplication app)
        {
            app.MapGrpcService<ActionExposedImpl>();
            //TODO: Add migration await app.EnsureDatabaseCreated();

        }
        
    }
}