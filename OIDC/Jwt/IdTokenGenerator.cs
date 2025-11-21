using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace OIDC.Jwt
{
    public class IdTokenGenerator
    {
        private readonly RsaKeyService _rsaKeyService;
        private readonly string _issuer;
        private readonly int _tokenExpiryInMinutes;

        public IdTokenGenerator(RsaKeyService rsaKeyService, IConfiguration configuration)
        {
            _rsaKeyService = rsaKeyService;
            _issuer = configuration["Oidc:Issuer"] ?? "localhost";
            _tokenExpiryInMinutes = int.TryParse(configuration["Oidc:TokenExpirationInMinutes"], out var minutes) ? minutes : 60;
        }

        public string GenerateIdToken(string email, string clientId, string? nonce = null)
        {
            var signingCredentials = _rsaKeyService.GetSigningCredentials();
            var claims = CreateClaims(email, clientId, nonce);

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_tokenExpiryInMinutes); // Could be configured if needed

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: clientId,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IEnumerable<Claim> CreateClaims(string email, string clientId, string? nonce)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Aud, clientId),
                new Claim(JwtRegisteredClaimNames.Iss, _issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim("client_id", clientId)
            };

            if (!string.IsNullOrEmpty(nonce))
            {
                claims.Add(new Claim("nonce", nonce));
            }

            return claims;
        }
    }
}
