using Interview.Entity;
using Interview.Model;
using Interview.Service.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OAuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly OAuthConfig _oauthConfig;

        public OAuthController(ITokenService tokenService, IOptions<OAuthConfig> oauthConfig)
        {
            _tokenService = tokenService;
            _oauthConfig = oauthConfig.Value;
        }

        /// <summary>
        /// Generates an OAuth 2.0 access token.
        /// </summary>
        /// <param name="request">The token request containing client credentials and user details.</param>
        /// <returns>Returns a bearer access token if the request is valid.</returns>
        /// <response code="200">Token generated successfully.</response>
        /// <response code="400">Invalid request parameters.</response>
        /// <response code="401">Unauthorized request due to invalid client credentials or unsupported grant type.</response>
        [HttpPost("Token")]
        public IActionResult Token(TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.client_id) ||
                string.IsNullOrEmpty(request.Username) ||
                string.IsNullOrEmpty(request.Password) ||
                string.IsNullOrEmpty(request.Scope) ||
                string.IsNullOrEmpty(request.GrantType) ||
                string.IsNullOrEmpty(request.client_secret))
            {
                return BadRequest(new
                {
                    ClientId = string.IsNullOrEmpty(request.client_id) ? "The ClientId field is required." : null,
                    GrantType = string.IsNullOrEmpty(request.GrantType) ? "The GrantType field is required." : null,
                    Username = string.IsNullOrEmpty(request.Username) ? "The Username field is required." : null,
                    Password = string.IsNullOrEmpty(request.Password) ? "The Password field is required." : null,
                    Scope = string.IsNullOrEmpty(request.Scope) ? "The Scope field is required." : null,
                    ClientSecret = string.IsNullOrEmpty(request.client_secret) ? "The ClientSecret field is required." : null
                });
            }

            if (request.client_id != _oauthConfig.ClientId ||
                request.client_secret != _oauthConfig.ClientSecret)
            {
                return Unauthorized("Invalid client credentials.");
            }


            if (request.GrantType != "authorization_code" && request.GrantType != "password")
            {
                return BadRequest("Unsupported grant type.");
            }

            var user = new User { UserName = request.Username };
            var token = _tokenService.GenerateToken(user, request);
            return Ok(new { access_token = token, token_type = "bearer" });
        }
    }
}
