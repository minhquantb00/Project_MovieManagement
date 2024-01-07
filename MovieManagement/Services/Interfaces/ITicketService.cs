using MovieManagement.Entities;
using MovieManagement.Payloads.DataRequests.TicketRequest;
using MovieManagement.Payloads.DataResponses.DataTicket;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface ITicketService
    {
        Task<ResponseObject<DataResponseTicket>> CreateTicket(int scheduleId, Request_CreateTicket request);
        Task<ResponseObject<DataResponseTicket>> UpdateTicket(Request_UpdateTicket request);
        List<Ticket> CreateListTicket(int scheduleId, List<Request_CreateTicket> requests);
    }
}
