using Microsoft.Extensions.DependencyInjection.Extensions;
using OIDC.GrpcClients.AuthGrpcClient;
using OIDC.GrpcClients.B2BGrpcClient;
using OIDC.Services.AuthService;

namespace OIDC.Services
{
    public static class DI
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.TryAddScoped<IAuthService, AuthService.AuthService>();


            Console.WriteLine($"[Info]----->{nameof(AddServices)} service added");




            return services;
        }
    }
}
