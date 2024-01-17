using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.CinemaRequest;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface ICinemaService
    {
        Task<ResponseObject<DataResponseCinema>> CreateCinema(Request_CreateCinema request);
        Task<PageResult<DataResponseRoom>> GetListRoomInCinema(int cinema, int pageSize, int pageNumber);
        Task<PageResult<DataResponseCinema>> GetListCinema(int pageSize, int pageNumber);
        Task<PageResult<DataResponseCinema>> GetCinemaByMovie(int movieId, int pageSize, int pageNumber);
        Task<ResponseObject<DataResponseCinema>> UpdateCinema(Request_UpdateCinema request);
    }
}
