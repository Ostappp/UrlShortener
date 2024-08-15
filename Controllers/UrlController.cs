using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Interfaces;
using UrlShortener.Models.Data;
namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlShortenerContext _context;
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlController(UrlShortenerContext context, IUrlShortenerService urlShortenerService)
        {
            _context = context;
            _urlShortenerService = urlShortenerService;
        }

        // GET: api/Url
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Url>>> GetUrls()
        {
            return await _context.Urls.ToListAsync();
        }

        // GET: api/Url/5
        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<Url>> GetUrl(int id)
        {
            var urlShortener = await _context.Urls.FindAsync(id);

            if (urlShortener == null)
            {
                return NotFound();
            }

            return urlShortener;
        }


        // POST: api/Url
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<IActionResult> AddUrl(int userId, string url)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            Url newUrl = new Url
            {
                AuthorId = userId,
                UrlOrigin = url,
                UrlShort = await _urlShortenerService.ShortTheUrl(url),
                DateOfCreation = DateTime.Now,
            };
            _context.Urls.Add(newUrl);
            await _context.SaveChangesAsync();

            return CreatedAtRoute(new { id = newUrl.Id }, newUrl);
        }

        // DELETE: api/Url/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteUrl(int id)
        {
            var urlShortener = await _context.Urls.FindAsync(id);
            if (urlShortener == null)
            {
                return NotFound();
            }

            _context.Urls.Remove(urlShortener);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
