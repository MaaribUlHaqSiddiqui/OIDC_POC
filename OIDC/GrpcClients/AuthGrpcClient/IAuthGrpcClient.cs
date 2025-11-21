using AuthManagementGRPC;

namespace OIDC.GrpcClients.AuthGrpcClient
{
    public interface IAuthGrpcClient
    {
        Task<bool> GenerateOtp(string email, string clientId, CancellationToken cancellationToken);
        Task<bool> VerifyOtp(string otp, string email, string clientId, CancellationToken cancellationToken);
        Task<ResponseToken> GetTokenFromAuthCode(string email, string scope, CancellationToken cancellationToken);
        Task<ResponseToken> GetTokenFromRefreshToken(string email, string scope, string RefreshToken, CancellationToken cancellationToken);
        Task<ResponseGetUserInfoFromToken> UserInfoFromToken(string token);
    }
}
