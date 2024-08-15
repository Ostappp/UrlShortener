using UrlShortener.Models.Data;
using UrlShortener.Models.ResponseModels;

namespace UrlShortener.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> CreateTokenAsync(User user);
        Task<TokenResponse> RefreshTokenAsync(string expiredToken);
    }
}
