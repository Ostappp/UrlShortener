using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Data;

namespace UrlShortener.Data
{
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext (DbContextOptions<UrlShortenerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Url> Urls { get; set; } 
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
