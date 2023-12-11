using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataSeat;

namespace MovieManagement.Payloads.Converters
{
    public class SeatConverter
    {
        private readonly AppDbContext _context;
        public SeatConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseSeat EntityToDTO(Seat seat)
        {

            return new DataResponseSeat
            {
                Id = seat.Id,
                Line = seat.Line,
                Number = seat.Number,
                RoomName = _context.rooms.SingleOrDefault(x => x.Seats.Any(y => y.Id == seat.Id)).Name,
                SeatStatusName = _context.seatsStatus.SingleOrDefault(x => x.Seats.Any(y => y.Id == seat.Id)).NameStatus,
                SeatTypeName = _context.seatTypes.SingleOrDefault(x => x.Seats.Any(y => y.Id == seat.Id)).NameType
            };
        }


    }
}
