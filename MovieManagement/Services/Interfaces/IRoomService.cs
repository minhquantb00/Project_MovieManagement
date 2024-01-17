using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.CinemaRequest;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IRoomService
    {
        Task<ResponseObject<DataResponseRoom>> CreateRoom(int cinemaId, Request_CreateRoom request);
        Task<List<Room>> CreateListRoom(int cinemaId, List<Request_CreateRoom> requests);
        Task<ResponseObject<DataResponseRoom>> UpdateRoom(Request_UpdateRoom request);
        Task<string> DeleteRoom(int roomId);
        Task<PageResult<DataResponseRoom>> GetRoomList(int? cinemaId, int pageSize, int pageNumber);
    }
}
