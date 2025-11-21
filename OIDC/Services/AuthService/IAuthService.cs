using OIDC.Models;
using OIDC.Services.AuthService.Models.Request;
using OIDC.Services.AuthService.Models.Response;

namespace OIDC.Services.AuthService
{
    public interface IAuthService
    {
        Task<bool> ValidateUser(string email, string clientId, CancellationToken cancellationToken);
        Task<string> VerifyOtp(OtpViewModel request, CancellationToken cancellationToken);
        Task<string> VerifyMfaTotp(MfaViewModel request, CancellationToken cancellationToken);
        Task<ResponseGetToken> GetToken(RequestGetToken request, CancellationToken cancellationToken);
        Task<ResponseUserInfo> GetUserInfo(string token, CancellationToken cancellationToken);
        Task<ResponseAuthorize> ValidateClientInfo(RequestAuthorize request, CancellationToken cancellationToken);
    }
}
