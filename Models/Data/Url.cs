namespace UrlShortener.Models.Data
{
    public class Url
    {
        public int Id { get; set; }

        public string UrlOrigin { get; set; }

        public string UrlShort { get; set; }

        public DateTime DateOfCreation { get; set; }

        public int AuthorId { get; set; }

        public virtual User Author { get; set; }

    }
}
