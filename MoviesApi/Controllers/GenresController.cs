using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Service;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genresService;

        public GenresController(IGenreService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genresService.GetAll();

            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> CreateGenre(CreateGenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _genresService.Add(genre);
            return Ok(genre);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(byte id, [FromBody] CreateGenreDto dto)
        {
            var genre = await _genresService.GetGenreById(id);
            if (genre == null)
                return NotFound($"Genre not found with Id {id}");
            genre.Name = dto.Name;
           _genresService.Update(genre);
            return Ok (genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(byte id)
        {
            var genre = await _genresService.GetGenreById(id);
            if (genre==null)
                return NotFound($"Genre not fount with ID: {id}");
           _genresService.Delete(genre);
            return Ok(genre);
        }

    }
    


}
