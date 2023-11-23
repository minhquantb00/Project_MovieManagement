using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataTicket;

namespace MovieManagement.Payloads.Converters
{
    public class TicketConverter
    {
        private readonly AppDbContext _context;
        public TicketConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseTicket EntityToDTO(Ticket ticket)
        {
            return new DataResponseTicket
            {
                Code = ticket.Code,
                Id = ticket.Id,
                ScheduleName = _context.schedules.SingleOrDefault(x => x.Id == ticket.ScheduleId).Name,
                SeatLine = _context.seats.SingleOrDefault(x => x.Id == ticket.SeatId).Line,
                SeatNumber = _context.seats.SingleOrDefault(x => x.Id == ticket.SeatId).Number,
                Price = ticket.PriceTicket
            };
        }
    }
}
