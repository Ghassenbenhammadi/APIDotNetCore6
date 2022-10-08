namespace MoviesApi.Dtos
{
    public class MoviesDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
            public string Storieline { get; set; }
        public IFormFile Poster { get; set; }
        public byte GenreId { get; set; }
        public string GenreName { get; set; }

    }
}
