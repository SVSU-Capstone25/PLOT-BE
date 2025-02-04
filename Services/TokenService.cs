using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;


using System.Text;
using Microsoft.Extensions.Options;
using Plot.Data.Models.Token;

namespace Plot.Services

{
    public class TokenService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _lifeTime;

        private readonly string _secretKey="REPLACE_THIS_SECRET_KEY_AND_STORE_IT_SOMEWHERE_SECURE_I_DONT_KNOW_THE_BEST_WAY_TO_DO_THAT";//******TEMP DEV*********


        public TokenService(IOptions<TokenSettings> tokenSettings) 
            {
                _issuer=tokenSettings.Value.Issuer
                    ?? throw new ArgumentNullException(tokenSettings.Value.Issuer);

                _audience=tokenSettings.Value.Audience
                    ?? throw new ArgumentNullException(tokenSettings.Value.Audience);

                _lifeTime = tokenSettings.Value.Lifetime 
                    ?? throw new ArgumentNullException(nameof(tokenSettings.Value.Lifetime));
            }

        public string GenerateToken(string userEmail)

        {
            var claims = new Claim[]
            {
                new(ClaimTypes.Email, userEmail)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenStartTime = DateTime.UtcNow;

            var tokenExpirationTime = tokenStartTime.AddMinutes(_lifeTime);

            
            var token = new JwtSecurityToken(
                _issuer, 
                _audience, 
                claims,
                tokenStartTime,
                tokenExpirationTime, 
                signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // No extra time allowance
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var emailClaim = principal.FindFirst(ClaimTypes.Email);
                
                return emailClaim?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}