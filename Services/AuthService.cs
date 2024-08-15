using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Enums;
using UrlShortener.Interfaces;
using UrlShortener.Models.Data;
using UrlShortener.Models.RequestModels;

namespace UrlShortener.Services
{
    public class AuthService : IAuthService
    {
        private readonly UrlShortenerContext _context;

        public AuthService(UrlShortenerContext context)
        {
            _context = context;
        }

        public async Task<User> SignInAsync(SignInRequest request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                throw new Exception($"NotFoundException. {nameof(User)} with email {request.Email} not found.");
            }

            if (!new IdentityPasswordHasher().VerifyPassword(user.PasswordHash, request.Password))
            {
                throw new Exception("BadRequestException. Invalid password.");
            }

            return user;
        }

        public async Task<User> SignUpAsync(SignUpRequest request)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user != null)
            {
                throw new Exception($"BadRequestException. {nameof(User)} with email {request.Email} already exists.");
            }

            user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = new IdentityPasswordHasher().HashPassword(request.Password),
                Role = Roles.RegularUser,
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
