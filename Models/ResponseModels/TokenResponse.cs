using UrlShortener.Models.Data;

namespace UrlShortener.Models.ResponseModels
{
    public class TokenResponse
    {
        public string Token { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}
