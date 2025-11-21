using Google.Protobuf;
using Grpc.Core;
using Helpers.Singletons;
using AuthManagementGRPC;

namespace OIDC.GrpcClients.AuthGrpcClient
{
    public class AuthGrpcClient : IAuthGrpcClient
    {
        public AuthGrpcClient() { }

        public async Task<bool> GenerateOtp(string email, string clientId, CancellationToken cancellationToken)
        {
            var key = "AuthManagement";//name of proto
            //var grpcCreds = ReadGRPCEndpoints.Instance.Get(key);

            var metadata = new Metadata
            {
                { "password", "grpcCreds.Password" },
            };

            string source = $"http://localhost:7089";

            var grpcClient = ReadGRPCEndpoints.Instance.CreateGrpcClient<AuthManagementGRPCServiceContract.AuthManagementGRPCServiceContractClient>(
                key, source, metadata);

            var request = new RequestGenerateOtp
            {
                Email = email,
                ClientId = clientId,
            };
            var grpcResponse = await grpcClient.GenerateOtpAsync(request);
            if (grpcResponse.IsRequestSuccess && grpcResponse.ResponseGenerateOtp.IsGenerated)
            {
                return true;
            }

            return false;

        }

        public async Task<bool> VerifyOtp(string otp, string email, string clientId, CancellationToken cancellationToken)
        {
            var key = "AuthManagement";//name of proto
            //var grpcCreds = ReadGRPCEndpoints.Instance.Get(key);

            var metadata = new Metadata
            {
                { "password", "grpcCreds.Password" },
            };

            string source = $"http://localhost:7089";

            var grpcClient = ReadGRPCEndpoints.Instance.CreateGrpcClient<AuthManagementGRPCServiceContract.AuthManagementGRPCServiceContractClient>(
                key, source, metadata);

            var request = new RequestVerifyOtp
            {
                Otp = otp,
                Email = email,
                ClientId = clientId,
            };
            var grpcResponse = await grpcClient.VerifyOtpAsync(request);
            if (grpcResponse.IsRequestSuccess && grpcResponse.ResponseVerifyOtp.IsVerified)
            {
                return true;
            }

            return false;

        }

        public async Task<ResponseToken> GetTokenFromAuthCode(string email, string scope, CancellationToken cancellationToken)
        {
            var key = "UserManagement";//name of proto
            //var grpcCreds = ReadGRPCEndpoints.Instance.Get(key);

            var metadata = new Metadata
            {
                { "password", "grpcCreds.Password" },
            };

            string source = $"http://localhost:7089";

            var grpcClient = ReadGRPCEndpoints.Instance.CreateGrpcClient<AuthManagementGRPCServiceContract.AuthManagementGRPCServiceContractClient>(
                key, source, metadata);

            var request = new RequestToken
            {
                Email = email,
                Scope = scope,
            };
            var grpcResponse = await grpcClient.GetTokenAsync(request);
            if (grpcResponse.IsRequestSuccess && grpcResponse.ResponseToken.IsInitialized())
            {
                return grpcResponse.ResponseToken;
            }

            return null;
        }

        public async Task<ResponseToken> GetTokenFromRefreshToken(string email, string scope, string RefreshToken, CancellationToken cancellationToken)
        {
            var key = "UserManagement";//name of proto
            //var grpcCreds = ReadGRPCEndpoints.Instance.Get(key);

            var metadata = new Metadata
            {
                { "password", "grpcCreds.Password" },
            };

            string source = $"http://localhost:7089";

            var grpcClient = ReadGRPCEndpoints.Instance.CreateGrpcClient<AuthManagementGRPCServiceContract.AuthManagementGRPCServiceContractClient>(
                key, source, metadata);

            var request = new RequestTokenFromRefreshToken
            {
                Email = email,
                Scope = scope,
                RefreshToken = RefreshToken,
            };
            var grpcResponse = await grpcClient.GetTokenFromRefreshTokenAsync(request);
            if (grpcResponse.IsRequestSuccess && grpcResponse.ResponseToken.IsInitialized())
            {
                return grpcResponse.ResponseToken;
            }

            return null;
        }

        public async Task<ResponseGetUserInfoFromToken> UserInfoFromToken(string token)
        {
            var key = "UserManagement";//name of proto
            //var grpcCreds = ReadGRPCEndpoints.Instance.Get(key);

            var metadata = new Metadata
            {
                { "password", "grpcCreds.Password" },
            };

            string source = $"http://localhost:7089";

            var grpcClient = ReadGRPCEndpoints.Instance.CreateGrpcClient<AuthManagementGRPCServiceContract.AuthManagementGRPCServiceContractClient>(
                key, source, metadata);

            var request = new RequestGetUserInfoFromToken
            {
                AccessToken = token
            };
            var grpcResponse = await grpcClient.GetUserInfoFromTokenAsync(request);
            if (grpcResponse.IsRequestSuccess)
            {
                return grpcResponse.ResponseGetUserInfoFromToken;
            }

            return null;
        }
    }
}
