using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataSeat;

namespace MovieManagement.Payloads.Converters
{
    public class SeatConverter
    {
        private readonly AppDbContext _context;
        public SeatConverter(AppDbContext context)
        {
            _context = context;
        }
        public DataResponseSeat EntityToDTO(Seat seat)
        {
            var seatInfo = _context.seats
                .Include(s => s.Room)
                .Include(s => s.SeatStatus)
                .Include(s => s.SeatType)
                .AsNoTracking()
                .SingleOrDefault(s => s.Id == seat.Id);

            if (seatInfo == null)
            {
                // Xử lý trường hợp không tìm thấy thông tin ghế.
                return null;
            }

            return new DataResponseSeat
            {
                Id = seatInfo.Id,
                Line = seatInfo.Line,
                Number = seatInfo.Number,
                RoomName = seatInfo.Room?.Name, // Sử dụng Room từ seatInfo
                SeatStatusName = seatInfo.SeatStatus?.NameStatus, // Sử dụng SeatStatus từ seatInfo
                SeatTypeName = seatInfo.SeatType?.NameType // Sử dụng SeatType từ seatInfo
            };
        }

    }
}
