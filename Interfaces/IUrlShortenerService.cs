namespace UrlShortener.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<string> ShortTheUrl(string url);
    }
}
