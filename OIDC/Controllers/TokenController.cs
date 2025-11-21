using Microsoft.AspNetCore.Mvc;
using OIDC.Models;
using OIDC.Services.AuthService;
using OIDC.Services.AuthService.Models.Request;
using OIDC.Services.AuthService.Models.Response;

namespace OIDC.Controllers
{
    [ApiController] //will give model validation in advance
    [Route("connect")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;
        public TokenController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost($"{nameof(Token)}")]
        public async Task<ActionResult<ResponseGetToken>> Token([FromForm] RequestGetToken request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new
                {
                    error = "invalid_request",
                    error_description = "Model validation failed.",
                    errors = validationErrors
                });
            }

            try
            {
                // Service call
                var result = await _authService.GetToken(request, cancellationToken);
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
                return StatusCode(500, new
                {
                    error = "server_error",
                    error_description = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }
    }
}
