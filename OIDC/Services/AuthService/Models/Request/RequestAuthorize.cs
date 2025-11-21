namespace OIDC.Services.AuthService.Models.Request
{
    public class RequestAuthorize
    {
        public string Client_Id { get; set; }
        public string Client_Secret { get; set; }
        public string Redirect_Uri { get; set; }
        public string Scope { get; set; } = "openid";
        public string State { get; set; }
        public string? Nonce { get; set; }
        public string? Prompt { get; set; }
        public int? MaxAge { get; set; }
    }
}
