using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly ApplicationDBContext _context;
        private new List<string> _allowedExtentions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        public MoviesController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies.Include(m => m.Genre).ToListAsync();
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.GenreId).SingleOrDefaultAsync(g => g.Id == id);
            if (movie == null)
                return NotFound();


            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
            {
                return BadRequest("Poster non valid");
            }
            //Control of type of Poster
            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg are allowed ");
            //Control for the size of Poster
            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster 1MB");
            //Control if Id is find
            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID ");
            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = dataStream.ToArray(),
                Rate = dto.Rate,
                Storieline = dto.Storieline,
                Year = dto.Year

            };
            await _context.AddAsync(movie);
            _context.SaveChanges();

            return Ok(movie);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody] MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No Movies with this ID{id}");
            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genre ID ");
            // verified if Poster here 
            if (dto.Poster != null)
            {
                //Control of type of Poster
                if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg are allowed ");
                //Control for the size of Poster
                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster 1MB");
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster= dataStream.ToArray();
            }
            movie.Title = dto.Title;
            movie.Storieline = dto.Storieline;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.Rate= dto.Rate;
            _context.SaveChanges();
            return Ok(movie);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            
                return NotFound($"No movies with this ID{id}");
                _context.Remove(movie);
                _context.SaveChanges();
                return Ok(movie);
        }
    }
}

