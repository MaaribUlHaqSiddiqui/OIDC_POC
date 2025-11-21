using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace OIDC.Jwt
{
    public class RsaKeyService
    {
        private readonly RsaSecurityKey _key;

        public RsaKeyService(RsaSecurityKey key)
        {
            _key = key;
        }

        public SecurityKey GetKey() => _key;

        public SigningCredentials GetSigningCredentials() =>
            new SigningCredentials(_key, SecurityAlgorithms.RsaSha256);

        public string GetKeyId() => _key.KeyId;
    }
}
