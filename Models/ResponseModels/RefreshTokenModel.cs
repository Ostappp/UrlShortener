namespace UrlShortener.Models.ResponseModels
{
    public class RefreshTokenModel
    {
        public int Id { get; set; }

        public UserModel User { get; set; }

        public string Value { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
