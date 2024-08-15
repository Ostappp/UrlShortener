using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Data;
using UrlShortener.Interfaces;
using UrlShortener.Models.Data;
using UrlShortener.Models.ResponseModels;

namespace UrlShortener.Services
{
    public class TokenService : ITokenService
    {
        private readonly UrlShortenerContext _context;
        private readonly IConfiguration _configuration;

        public TokenService(UrlShortenerContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenResponse> CreateTokenAsync(User user)
        {
            RefreshToken refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (refreshToken == null)
            {
                refreshToken = await CreateRefreshTokenAsync(user.Id);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("refreshTokenId", refreshToken.Id.ToString(CultureInfo.InvariantCulture)),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                    SecurityAlgorithms.HmacSha256));

            string jwtToken = await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));

            return new TokenResponse
            {
                Token = jwtToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenResponse> RefreshTokenAsync(string expiredToken)
        {
            int userId;

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(expiredToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = validatedToken as JwtSecurityToken;
                int currentRefreshTokenId = int.Parse(
                    jwtToken.Claims.FirstOrDefault(c => c.Type == "refreshTokenId")?.Value,
                    CultureInfo.InvariantCulture);

                RefreshToken currentRefreshToken = await _context.RefreshTokens.FindAsync(currentRefreshTokenId);
                userId = int.Parse(
                    jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value,
                    CultureInfo.InvariantCulture);

                _context.RefreshTokens.Remove(currentRefreshToken);
                await _context.SaveChangesAsync();
                await CreateRefreshTokenAsync(userId);
            }
            catch
            {
                throw new Exception("BadRequestException. Invalid token.");
            }

            User user = await _context.Users.FindAsync(userId);

            return await CreateTokenAsync(user);
        }

        private async Task<RefreshToken> CreateRefreshTokenAsync(int userId)
        {
            RefreshToken token = new RefreshToken
            {
                UserId = userId,
                Value = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.Now.AddDays(30)
            };
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();

            return token;
        }
    }
}
