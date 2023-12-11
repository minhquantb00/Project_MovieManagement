using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandleGenerate;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.TicketRequest;
using MovieManagement.Payloads.DataResponses.DataTicket;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class TicketService : ITicketService
    {
        private readonly ResponseObject<DataResponseTicket> _responseObject;
        private readonly TicketConverter _ticketConverter;
        public readonly AppDbContext _context;
        public TicketService(ResponseObject<DataResponseTicket> responseObject, TicketConverter ticketConverter)
        {
            _responseObject = responseObject;
            _ticketConverter = ticketConverter;
            _context = new AppDbContext();
        }

        public async Task<ResponseObject<DataResponseTicket>> CreateTicket(Request_CreateTicket request)
        {
            var seat = await _context.seats.SingleOrDefaultAsync(x => x.Id == request.SeatId);
            if (seat == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy ghế", null);
            }
            var schedule = await _context.schedules.SingleOrDefaultAsync(x => x.Id == request.ScheduleId);
            if(schedule == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thông tin", null);
            }
            var ticket = new Ticket();
            ticket.ScheduleId = request.ScheduleId;
            ticket.SeatId = request.SeatId;
            ticket.Code = "MyBugs_" + DateTime.Now.Ticks.ToString() + new Random().Next(1000, 9999).ToString();
            await _context.tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Tạo vé thành công", _ticketConverter.EntityToDTO(ticket));
        }

        public async Task<ResponseObject<DataResponseTicket>> UpdateTicket(Request_UpdateTicket request)
        {
            var ticket = await _context.tickets.SingleOrDefaultAsync(x => x.Id == request.TicketId);
            if(ticket == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thông tin", null);
            }
            ticket.Id = request.TicketId;
            ticket.ScheduleId = request.ScheduleId;
            ticket.SeatId= request.SeatId;
            _context.tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật thông tin thành công", _ticketConverter.EntityToDTO(ticket));
        }
    }
}
