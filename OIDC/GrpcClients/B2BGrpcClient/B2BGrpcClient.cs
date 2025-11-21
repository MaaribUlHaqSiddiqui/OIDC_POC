using Grpc.Core;
using Helpers.Singletons;
using B2BOIDCManagement;

namespace OIDC.GrpcClients.B2BGrpcClient
{
    public class B2BGrpcClient : IB2BGrpcClient
    {
        public async Task<ResponseGetApplicationDetails> GetClientInfo(string clientId, string clientSecret, CancellationToken cancellationToken)
        {
            var key = "UserManagement";//name of proto
            //var grpcCreds = ReadGRPCEndpoints.Instance.Get(key);

            var metadata = new Metadata
            {
                { "password", "grpcCreds.Password" },
            };

            string source = $"http://192.168.80.123:7089";

            var grpcClient = ReadGRPCEndpoints.Instance.CreateGrpcClient<B2B_OIDCGRPCServiceContract.B2B_OIDCGRPCServiceContractClient>(
                key, source, metadata);

            var request = new RequestGetApplicationDetails
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
            };
            var grpcResponse = await grpcClient.GetApplicationDetailsGRPCAsync(request);
            if (grpcResponse.IsRequestSuccess)
            {
                var data = grpcResponse.ApplicationDetails;

                return data;
            }

            return null;
        }
    }
}
