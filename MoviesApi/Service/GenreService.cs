using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Service
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDBContext _context;

        public async Task<Genre> Add(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return genre;

        }

        public  async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();

        }

 
       public async Task<Genre> GetGenreById(byte id)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

        public Genre Update(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return genre ;

        }
    }
}
