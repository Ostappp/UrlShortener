using Microsoft.AspNetCore.Identity;
using UrlShortener.Models.Data;

namespace UrlShortener.Services
{
    public class IdentityPasswordHasher
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public IdentityPasswordHasher()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }
    }
}
