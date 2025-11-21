namespace OIDC.Models
{
    public class MfaViewModel
    {
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
        public string UserName { get; set; }
        public string? Nonce { get; set; }
        public string Totp { get; set; }
    }
}
