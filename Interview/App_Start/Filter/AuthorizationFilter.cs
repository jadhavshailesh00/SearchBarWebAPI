using Interview.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Interview.App_Start.Filter
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly OAuthConfig _oauthConfig;
        public AuthorizationFilter(IOptions<OAuthConfig> oauthConfig)
        {
            _oauthConfig = oauthConfig.Value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var tokentype = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").First();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(tokentype))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!tokentype.Contains("Bearer") && !tokentype.Contains("bearer"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_oauthConfig.Key);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _oauthConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _oauthConfig.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims);

                var user = new ClaimsPrincipal(claimsIdentity);
                context.HttpContext.User = user;

                var hasRequiredScope = user.HasClaim(c => c.Type == "scope" && c.Value == "Product");
                if (!hasRequiredScope)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch (SecurityTokenException)
            {
                context.Result = new UnauthorizedResult();
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}