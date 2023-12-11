using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataBill;

namespace MovieManagement.Payloads.Converters
{
    public class BillTicketConverter
    {
        private readonly AppDbContext _context;
        public BillTicketConverter(AppDbContext context)
        {
            _context = context;
        }
        public DataResponseBillTicket EntityToDTO(BillTicket ticket)
        {
            return new DataResponseBillTicket
            {
                Id = ticket.Id,
                Quantity = ticket.Quantity,
                SeatLine = _context.seats.Include(x => x.Tickets.Any(x => x.Id == ticket.TicketId)).SingleOrDefault().Line,
                SeatNumber = _context.seats.Include(x => x.Tickets.Any(x => x.Id == ticket.TicketId)).SingleOrDefault().Number
            };
        }
    }
}
