namespace OIDC.Services.AuthService.Models.Response
{
    public class ResponseAuthorize
    {
        public bool IsAuthorized { get; set; }
        public bool IsMFA { get; set; }
        public string ApplicationName { get; set; } = string.Empty;
    }
}
