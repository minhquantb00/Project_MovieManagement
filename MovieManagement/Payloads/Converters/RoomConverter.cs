using Microsoft.EntityFrameworkCore;
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
        public RoomConverter(SeatConverter seatConverter, AppDbContext context, SchedulesConverter scheduleConverter)
        {
            _context = context;
            _seatConverter = seatConverter;
            _scheduleConverter = scheduleConverter;
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
                DataResponseSeats = _context.seats.Include(x => x.SeatType).Include(x => x.SeatStatus).Include(x => x.Tickets).Include(x => x.Room).Where(x => x.RoomId == room.Id).Select(x => _seatConverter.EntityToDTO(x)),
            };
        }
    }
}
