using System.Data.Common;
using Helpers.CustomExceptionThrower;
using OIDC.EnumsAndConstants;
using OIDC.GrpcClients.AuthGrpcClient;
using OIDC.GrpcClients.B2BGrpcClient;
using OIDC.Jwt;
using OIDC.Models;
using OIDC.Services.AuthService.Models.Request;
using OIDC.Services.AuthService.Models.Response;
using SessionManager;

namespace OIDC.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAuthGrpcClient _authGrpcClient;
        private readonly RedisSessionManager _sessionManager;
        private readonly IdTokenGenerator _idTokenGenerator;
        private readonly IB2BGrpcClient _b2BGrpcClient;
        public AuthService(IAuthGrpcClient authGrpcClient, RedisSessionManager sessionManager, IdTokenGenerator idTokenGenerator, IB2BGrpcClient b2BGrpcClient)
        {
            _authGrpcClient = authGrpcClient;
            _sessionManager = sessionManager;
            _idTokenGenerator = idTokenGenerator;
            _b2BGrpcClient = b2BGrpcClient;
        }

        public async Task<bool> ValidateUser(string email, string clientId, CancellationToken cancellationToken)
        {
            var result = await _authGrpcClient.GenerateOtp(email, clientId, cancellationToken);

            return result;
            //return true;
        }

        public async Task<string> VerifyOtp(OtpViewModel request, CancellationToken cancellationToken)
        {
            var code = "";
            var result = await _authGrpcClient.VerifyOtp(request.Otp, request.UserName, request.ClientId, cancellationToken);

            if (result && !request.IsMFA)
            {
                code = Guid.NewGuid().ToString("N");
                await _sessionManager.StoreAsync<OIDCCodeSession>($"{code}:oidc_code", new OIDCCodeSession()
                {
                    ClientId = request.ClientId,
                    Email = request.UserName,
                    Code = code,
                    RedirectUri = request.RedirectUri,
                    Scope = request.Scope,
                    Nonce = request.Nonce,
                }, TimeSpan.FromMinutes(5));
            }

            return code;
        }

        public async Task<string> VerifyMfaTotp(MfaViewModel request, CancellationToken cancellationToken)
        {
            var code = "";

            if (request.Totp == "123456") ;
            {
                code = Guid.NewGuid().ToString("N");
                await _sessionManager.StoreAsync<OIDCCodeSession>($"{code}:oidc_code", new OIDCCodeSession()
                {
                    ClientId = request.ClientId,
                    Email = request.UserName,
                    Code = code,
                    RedirectUri = request.RedirectUri,
                    Scope = request.Scope,
                    Nonce = request.Nonce,
                }, TimeSpan.FromMinutes(5));
            }
            //var result = await _authGrpcClient.VerifyOtp(request.Otp, request.UserName, request.ClientId, cancellationToken);

            //if (result && !request.IsMFA)
            //{
            //    code = Guid.NewGuid().ToString("N");
            //    await _sessionManager.StoreAsync<OIDCCodeSession>($"{code}:oidc_code", new OIDCCodeSession()
            //    {
            //        ClientId = request.ClientId,
            //        Email = request.UserName,
            //        Code = code,
            //        RedirectUri = request.RedirectUri,
            //        Scope = request.Scope,
            //        Nonce = request.Nonce,
            //    }, TimeSpan.FromMinutes(5));
            //}

            return code;
        }

        public async Task<ResponseGetToken> GetToken(RequestGetToken request, CancellationToken cancellationToken)
        {
            var response = new ResponseGetToken();

            if (request.Grant_Type == EnumAuthorizationType.authorization_code.ToString())
            {
                var result = await _sessionManager.GetAsync<OIDCCodeSession>($"{request.Code}:oidc_code");
                if (result == null)
                {
                    throw new ArgumentFalseException("Invalid Code");
                }

                if (result.ClientId != request.Client_Id)
                {
                    throw new ArgumentFalseException("Invalid Client Id");
                }

                var grpcResult = await _authGrpcClient.GetTokenFromAuthCode(result.Email, result.Scope, cancellationToken);
                if (grpcResult == null)
                {
                    throw new Exception("Token could not created");
                }

                await _sessionManager.StoreAsync<OIDCRefreshTokenSession>($"{grpcResult.RefreshToken}:oidc_refreshtoken", new OIDCRefreshTokenSession
                {
                    Email = result.Email,
                    Scope = result.Scope,
                    ClientId = result.ClientId
                });

                response.AccessToken = grpcResult.Token;
                response.RefreshToken = grpcResult.RefreshToken;
                response.Expires_In = 3600;
                response.Id_Token = result.Scope.Contains("openid") ? _idTokenGenerator.GenerateIdToken(result.Email, result.ClientId, result.Nonce) : null;

            }

            if (request.Grant_Type == EnumAuthorizationType.refresh_token.ToString())
            {
                var result = await _sessionManager.GetAsync<OIDCRefreshTokenSession>($"{request.Refresh_Token}:oidc_refreshtoken");
                if (result == null)
                {
                    throw new ArgumentFalseException("Invalid Refresh Token");
                }

                var grpcResult = await _authGrpcClient.GetTokenFromRefreshToken(result.Email, result.Scope, request.Refresh_Token, cancellationToken);
                if (grpcResult == null)
                {
                    throw new ArgumentFalseException("Token could not created");
                }

                await _sessionManager.RemoveTokenAsync($"{request.Refresh_Token}:oidc_refreshtoken");
                await _sessionManager.StoreAsync<OIDCRefreshTokenSession>($"{grpcResult.RefreshToken}:oidc_refreshtoken", new OIDCRefreshTokenSession
                {
                    Email = result.Email,
                    Scope = result.Scope,
                    ClientId = result.ClientId
                });

                response.AccessToken = grpcResult.Token;
                response.RefreshToken = grpcResult.RefreshToken;
                response.Expires_In = 3600;
                response.Id_Token = result.Scope.Contains("openid") ? _idTokenGenerator.GenerateIdToken(result.Email, request.Client_Id) : null;

            }

            return response;
        }

        public async Task<ResponseUserInfo> GetUserInfo(string token, CancellationToken cancellationToken)
        {
            var result = await _authGrpcClient.UserInfoFromToken(token);
            if (result is null)
            {
                throw new UnauthorizedAccessException("Invalid access token");
            }

            var allowedScopes = result.Scope?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

            var response = new ResponseUserInfo
            {
                Sub = result.UserId
            };

            if (allowedScopes.Contains("email"))
            {
                response.Email = result.Email;
            }

            // Optional: Add more fields based on custom scopes
            if (allowedScopes.Contains("profile"))
            {
                response.UserType = result.UserType;
                response.SessionStartDate = result.SessionStartDate;
                response.SessionEndDate = result.SessionEndDate;
            }

            if (allowedScopes.Contains("resources"))
            {
                response.Resources = result.Resources;
            }

            return response;
        }

        public async Task<ResponseAuthorize> ValidateClientInfo(RequestAuthorize request, CancellationToken cancellationToken)
        {
            var result = await _b2BGrpcClient.GetClientInfo(request.Client_Id, request.Client_Secret, cancellationToken);
            ArgumentThrowCustom.ThrowIfNull<ArgumentFalseException>(result, "Client not found");

            return new ResponseAuthorize()
            {
                IsAuthorized = true,
                IsMFA = result.MFA,
                ApplicationName = result.ApplicationName,
            };
        }
    }
}
