namespace UrlShortener.Models.Data
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Value { get; set; } = null!;

        public DateTime ExpirationDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
