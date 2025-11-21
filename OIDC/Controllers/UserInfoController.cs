using Microsoft.AspNetCore.Mvc;
using OIDC.Services.AuthService;
using OIDC.Services.AuthService.Models.Response;

namespace OIDC.Controllers
{
    [ApiController]
    [Route("connect")]
    public class UserInfoController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserInfoController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet($"{nameof(UserInfo)}")]
        public async Task<ActionResult<ResponseUserInfo>> UserInfo(CancellationToken cancellationToken)
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new
                {
                    error = "unauthorized",
                    error_description = "access token not provided or invalid"
                });
            }

            var accessToken = authHeader.Substring("Bearer ".Length).Trim();
            try
            {
                // Service call
                var result = await _authService.GetUserInfo(accessToken, cancellationToken);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = "invalid_argument",
                    error_description = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    error = "unauthorized",
                    error_description = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Generic fallback
                return StatusCode(500, new
                {
                    error = "server_error",
                    error_description = "An unexpected error occurred.",
                    details = ex.Message // Optional: remove in production
                });
            }
        }
    }
}
