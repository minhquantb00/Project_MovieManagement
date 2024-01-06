using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.SeatRequest;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.DataResponses.DataSeat;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface ISeatService
    {
        Task<ResponseObject<DataResponseSeat>> CreateSeat(int roomId, Request_CreateSeat request);
        List<Seat> CreateListSeat(int roomId, List<Request_CreateSeat> requests);
        Task<ResponseObject<DataResponseRoom>> UpdateSeat(int roomId, List<Request_UpdateSeat> requests);
        Task<PageResult<DataResponseSeat>> GetAllSeat(int pageSize, int pageNumber);
        Task<PageResult<DataResponseSeat>> GetSeatByStatus(int statusId, int pageSize, int pageNumber);
        Task<PageResult<DataResponseSeat>> GetSeatByRoom(int roomId, int pageSize, int pageNumber);
        
    }
}
