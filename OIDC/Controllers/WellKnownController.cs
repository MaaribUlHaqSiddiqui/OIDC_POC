using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OIDC.Jwt;

namespace OIDC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellKnownController : ControllerBase
    {
        private readonly RsaKeyService _rsaKeyService;
        private readonly IConfiguration _configuration;

        public WellKnownController(RsaKeyService rsaKeyService, IConfiguration configuration)
        {
            _rsaKeyService = rsaKeyService;
            _configuration = configuration;
        }

        [HttpGet("openid-configuration")]
        public IActionResult GetOpenIdConfiguration()
        {
            var issuer = _configuration["Oidc:Issuer"] ?? $"{Request.Scheme}://{Request.Host}";
            var config = new
            {
                issuer,
                authorization_endpoint = $"{issuer}/OIDC/authorize",
                token_endpoint = $"{issuer}/OIDC/token",
                userinfo_endpoint = $"{issuer}/OIDC/userinfo",
                jwks_uri = $"{issuer}/.well-known/jwks.json",
                response_types_supported = new[] { "code", "token", "id_token" },
                subject_types_supported = new[] { "public" },
                id_token_signing_alg_values_supported = new[] { SecurityAlgorithms.RsaSha256 },
                scopes_supported = new[] { "openid", "email" },
                grant_types_supported = new[] { "authorization_code", "refresh_token" }
            };

            return Ok(config);
        }

        [HttpGet("jwks.json")]
        public IActionResult GetJwks()
        {
            var key = _rsaKeyService.GetKey() as RsaSecurityKey;

            var parameters = key.Rsa?.ExportParameters(false) ?? key.Parameters;

            var jwk = new
            {
                kty = "RSA",
                use = "sig",
                kid = key.KeyId,
                e = Base64UrlEncoder.Encode(parameters.Exponent),
                n = Base64UrlEncoder.Encode(parameters.Modulus),
                alg = SecurityAlgorithms.RsaSha256
            };

            var jwks = new { keys = new[] { jwk } };
            return Ok(jwks);
        }
    }
}
