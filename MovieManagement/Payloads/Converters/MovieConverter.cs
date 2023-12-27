using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataMovie;

namespace MovieManagement.Payloads.Converters
{
    public class MovieConverter
    {
        private readonly AppDbContext _context;
        private readonly SchedulesConverter _converter;
        public MovieConverter()
        {
            _context = new AppDbContext();
            _converter = new SchedulesConverter();
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
                HeroImage = movie.HeroImage,
                Language = movie.Language,
                MovieDuration = movie.MovieDuration,
                MovieTypeName = _context.movieTypes.SingleOrDefault(x => x.Id == movie.MovieTypeId).MovieTypeName,
                Name = movie.Name,
                PremiereDate = movie.PremiereDate,
                Trailer = movie.Trailer,
                Schedules = _context.schedules.Where(x => x.MovieId == movie.Id).Select(x => _converter.EntityToDTO(x))
            };
        }
    }
}
