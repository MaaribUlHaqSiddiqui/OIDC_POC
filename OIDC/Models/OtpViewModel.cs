namespace OIDC.Models
{
    public class OtpViewModel
    {
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
        public string UserName { get; set; }
        public string Otp {  get; set; }
        public string? Nonce { get; set; }
        public bool IsMFA { get; set; }
    }
}
