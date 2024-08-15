namespace UrlShortener.Models.ResponseModels
{
    public class TokenResponseModel
    {
        public string Token { get; set; }

        public RefreshTokenModel RefreshToken { get; set; }
    }
}
