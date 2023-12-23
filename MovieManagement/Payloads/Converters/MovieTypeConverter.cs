using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataMovie;

namespace MovieManagement.Payloads.Converters
{
    public class MovieTypeConverter
    {
        private readonly AppDbContext _context;
        private readonly MovieConverter _converter;
        public MovieTypeConverter()
        {
            _context = new AppDbContext();
            _converter = new MovieConverter();
        }
        public DataResponseMovieType EntityToDTO(MovieType movieType)
        {
            return new DataResponseMovieType
            {
                Id = movieType.Id,
                MovieTypeName = movieType.MovieTypeName,
                Movies = _context.movies.Where(x => x.MovieTypeId == movieType.Id).Select(x => _converter.EntityToDTO(x)),
            };
        }
    }
}
