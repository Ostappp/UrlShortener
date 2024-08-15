using UrlShortener.Enums;

namespace UrlShortener.Models.Data
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public Roles Role { get; set; }

        public string PasswordHash { get; set; }

        public virtual ICollection<Url> URLs { get; set; } = new List<Url>();

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
