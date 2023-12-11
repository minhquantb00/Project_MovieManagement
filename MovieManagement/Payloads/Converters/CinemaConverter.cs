using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataCinema;

namespace MovieManagement.Payloads.Converters
{
    public class CinemaConverter
    {
        private readonly AppDbContext _context;
        private readonly RoomConverter _roomConverter;
        public CinemaConverter()
        {
            _roomConverter = new RoomConverter();
            _context = new AppDbContext();
        }
        public DataResponseCinema EntityToDTO(Cinema cinema)
        {
            return new DataResponseCinema
            {
                Id = cinema.Id,
                Address = cinema.Address,
                Description = cinema.Description,
                NameOfCinema = cinema.NameOfCinema,
                Room = _context.rooms.Where(x => x.CinemaId == cinema.Id).Select(x => _roomConverter.EntityToDTO(x)).AsQueryable(),
            };
        }
    }
}
