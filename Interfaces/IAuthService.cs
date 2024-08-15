using UrlShortener.Models.Data;
using UrlShortener.Models.RequestModels;

namespace UrlShortener.Interfaces
{
    public interface IAuthService
    {
        Task<User> SignInAsync(SignInRequest request);
        Task<User> SignUpAsync(SignUpRequest request);
    }
}
