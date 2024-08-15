using UrlShortener.Interfaces;

namespace UrlShortener.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        public async Task<string> ShortTheUrl(string url)
        {
            return $"{url}-short";
        }
    }
}
