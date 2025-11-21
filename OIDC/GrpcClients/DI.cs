using Microsoft.Extensions.DependencyInjection.Extensions;
using OIDC.GrpcClients.AuthGrpcClient;
using OIDC.GrpcClients.B2BGrpcClient;

namespace OIDC.GrpcClients
{
    public static class DI
    {
        public static IServiceCollection AddGrpcClients(this IServiceCollection services)
        {

            services.TryAddScoped<IAuthGrpcClient, AuthGrpcClient.AuthGrpcClient>();
            services.TryAddScoped<IB2BGrpcClient, B2BGrpcClient.B2BGrpcClient>();


            Console.WriteLine($"[Info]----->{nameof(AddGrpcClients)} service added");




            return services;
        }
    }
}
