namespace MoviesApi.Service
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> Add(Genre genre);
        Task<Genre> GetGenreById(byte id);
       
        Genre Update(Genre genre);
        Genre Delete(Genre genre);


    }
}
