using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OIDC.Models;
using OIDC.Services.AuthService;
using OIDC.Services.AuthService.Models.Request;

namespace OIDC.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IAuthService _authService;

        public AuthorizeController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Authorize([FromQuery] RequestAuthorize request, CancellationToken cancellationToken)
        {
            var result = await _authService.ValidateClientInfo(request, cancellationToken);

            // Always show consent first
            var model = new ConsentViewModel
            {
                ClientId = request.Client_Id,
                RedirectUri = request.Redirect_Uri,
                Scope = request.Scope,
                State = request.State,
                IsMFA = result.IsMFA,
                ApplicationName = result.ApplicationName,
            };
            return View("Consent", model);
        }

        [HttpPost]
        public async Task<IActionResult> Consent(ConsentViewModel model, CancellationToken cancellationToken)
        {
            if (!model.Accepted)
            {
                // if user denies
                return Redirect($"{model.RedirectUri}?error=access_denied&state={model.State}");
            }

            // if user gives consent → move to login
            var loginModel = new LoginViewModel
            {
                ClientId = model.ClientId,
                RedirectUri = model.RedirectUri,
                Scope = model.Scope,
                State = model.State,
                IsMFA = model.IsMFA
            };

            return View("Login", loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            var result = await _authService.ValidateUser(model.UserName, model.ClientId, cancellationToken);
            if (!result)
            {
                return View("Login", model);
            }

            var otpModel = new OtpViewModel
            {
                ClientId = model.ClientId,
                RedirectUri = model.RedirectUri,
                Scope = model.Scope,
                State = model.State,
                IsMFA = model.IsMFA,
            };

            return View("Otp", otpModel);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(OtpViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View("Otp", model);
            }

            var result = await _authService.VerifyOtp(model, cancellationToken);

            if(model.IsMFA)
            {
                var mfaModel = new MfaViewModel
                {
                    ClientId = model.ClientId,
                    RedirectUri = model.RedirectUri,
                    Scope = model.Scope,
                    State = model.State,
                    UserName = model.UserName
                };

                return View("Mfa", mfaModel);
            }

            if (!result.IsNullOrEmpty())
            {
                var url = $"{model.RedirectUri}?code={result}&state={model.State}";

                return Redirect(url);
            }

            return View("Otp", model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyMfa(MfaViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View("Mfa", model);

            var totpResult = await _authService.VerifyMfaTotp(model, cancellationToken);

            if (!string.IsNullOrEmpty(totpResult))
            {
                var redirectUrl = $"{model.RedirectUri}?code={totpResult}&state={model.State}";
                return Redirect(redirectUrl);
            }

            return View("Mfa", model);
        }
    }

}
