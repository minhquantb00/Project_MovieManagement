using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataSeat;

namespace MovieManagement.Payloads.Converters
{
    public class SeatConverter
    {
        private readonly RoomConverter roomConverter;
        private readonly AppDbContext _context;
        public SeatConverter()
        {
            _context = new AppDbContext();
            roomConverter = new RoomConverter();
        }
        public DataResponseSeat EntityToDTO(Seat seat)
        {
            return new DataResponseSeat
            {
                Id = seat.Id,
                Line = seat.Line,
                Number = seat.Number,
                RoomName = _context.rooms.SingleOrDefault(x => x.Id == seat.RoomId).Name,
                SeatStatusName = _context.seatsStatus.SingleOrDefault(x => x.Id == seat.SeatStatusId).NameStatus,
                SeatTypeName = _context.seatTypes.SingleOrDefault(x => x.Id == seat.SeatTypeId).NameType
            };
        }
    }
}
