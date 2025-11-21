namespace OIDC.EnumsAndConstants
{
    public class OIDCCodeSession
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public string? Nonce { get; set; }
    }
}
