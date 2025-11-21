using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OIDC.Services.AuthService.Models.Request
{
    public class RequestGetToken
    {
        [FromForm(Name = "grant_type")]
        [Required(ErrorMessage = "grant_type is required.")]
        public string Grant_Type { get; set; }

        [FromForm(Name = "code")]
        public string? Code { get; set; }

        [FromForm(Name = "refresh_token")]
        public string? Refresh_Token { get; set; }

        [FromForm(Name = "redirect_uri")]
        public string? Redirect_Uri { get; set; }

        [FromForm(Name = "client_id")]
        [Required(ErrorMessage = "client_id is required.")]
        public string Client_Id { get; set; }

        [FromForm(Name = "client_secret")]
        [Required(ErrorMessage = "client_secret is required.")]
        public string Client_Secret { get; set; }
    }
}

