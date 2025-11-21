using B2BOIDCManagement;

namespace OIDC.GrpcClients.B2BGrpcClient
{
    public interface IB2BGrpcClient
    {
        Task<ResponseGetApplicationDetails> GetClientInfo(string clientId, string clientSecret, CancellationToken cancellationToken);
    }
}
