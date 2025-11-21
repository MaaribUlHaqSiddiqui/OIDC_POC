namespace OIDC.Services.AuthService.Models.Response
{
    public class ResponseUserInfo
    {
        public string Sub { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public string SessionStartDate { get; set; }
        public string SessionEndDate { get; set; }
        public string Resources { get; set; }
    }
}
