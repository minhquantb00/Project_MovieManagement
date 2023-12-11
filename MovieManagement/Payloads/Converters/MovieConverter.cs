using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataMovie;

namespace MovieManagement.Payloads.Converters
{
    public class MovieConverter
    {
        private readonly AppDbContext _context;
        public MovieConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseMovie EntityToDTO(Movie movie)
        {
            return new DataResponseMovie
            {
                Description = movie.Description,
                Director = movie.Director,
                EndTime = movie.EndTime,
                Id = movie.Id,
                Image = movie.Image,
                Language = movie.Language,
                MovieDuration = movie.MovieDuration,
                MovieTypeName = _context.movieTypes.SingleOrDefault(x => x.Id == movie.MovieTypeId).MovieTypeName,
                Name = movie.Name,
                PremiereDate = movie.PremiereDate,
                Trailer = movie.Trailer
            };
        }
    }
}
