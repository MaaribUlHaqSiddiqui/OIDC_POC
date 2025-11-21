namespace OIDC.Models
{
    public class ConsentViewModel
    {
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
        public bool Accepted { get; set; }
        public bool IsMFA { get; set; }
        public string ApplicationName { get; set; }
    }
}
