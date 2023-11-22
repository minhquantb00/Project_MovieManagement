using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataCinema;

namespace MovieManagement.Payloads.Converters
{
    public class RoomConverter
    {
        private readonly SeatConverter _seatConverter;
        private readonly AppDbContext _context;
        private readonly SchedulesConverter _scheduleConverter;
        public RoomConverter()
        {
            _context = new AppDbContext();
            _seatConverter = new SeatConverter();
            _scheduleConverter = new SchedulesConverter();
        }
        public DataResponseRoom EntityToDTO(Room room)
        {
            return new DataResponseRoom
            {
                Id = room.Id,
                Capacity = room.Capacity,
                Description = room.Description,
                Name = room.Name,
                Type = room.Type,
                DataResponseSeats = _context.seats.Select(x => _seatConverter.EntityToDTO(x))
            };
        }
    }
}
