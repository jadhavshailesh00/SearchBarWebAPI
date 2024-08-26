using Interview.Entity;
using Interview.Model;
using Interview.Repository.Token;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Interview.Service.Token
{
    public class TokenService : ITokenService
    {
        private readonly OAuthConfig _oauthConfig;
        private ITokenRepository _TokenRepository;

        public TokenService(IOptions<OAuthConfig> oauthConfig, ITokenRepository tokenRepository)
        {
            _oauthConfig = oauthConfig.Value;
            _TokenRepository = tokenRepository;
        }

        public string GenerateToken(User user, TokenRequest request)
        {
            var CurrentUser = _TokenRepository.FatchUses(user);

            if (CurrentUser == null)
            {
                return string.Empty;
            }

            if (CurrentUser.UserName != request.Username || request.Password != CurrentUser.Password)
            {
                return string.Empty;
            }

            var issuer = _oauthConfig.Issuer;
            var audience = _oauthConfig.Audience;
            var Scopes = _oauthConfig.Scopes;
            var key = Encoding.ASCII.GetBytes(_oauthConfig.Key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("scope", Scopes),
                new Claim(ClaimTypes.Email, CurrentUser.Email)
            };

            var roles = CurrentUser.Role.Split(",").ToArray();


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
