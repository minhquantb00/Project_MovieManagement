using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataBill;

namespace MovieManagement.Payloads.Converters
{
    public class BillTicketConverter
    {
        private readonly AppDbContext _context;
        public BillTicketConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseBillTicket EntityToDTO(BillTicket ticket)
        {
            var seat = _context.seats.FirstOrDefault(s => s.Tickets.Any(t => t.Id == ticket.TicketId));
            if (seat == null)
            {
                return null;
            }

            return new DataResponseBillTicket
            {
                Id = ticket.Id,
                Quantity = ticket.Quantity,
                SeatLine = seat.Line,
                SeatNumber = seat.Number
            };
        }

    }
}
