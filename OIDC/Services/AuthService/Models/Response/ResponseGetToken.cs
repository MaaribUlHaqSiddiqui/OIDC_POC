namespace OIDC.Services.AuthService.Models.Response
{
    public class ResponseGetToken
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int Expires_In { get; set; }
        public string RefreshToken { get; set; }
        public string? Id_Token { get; set; }
    }
}
