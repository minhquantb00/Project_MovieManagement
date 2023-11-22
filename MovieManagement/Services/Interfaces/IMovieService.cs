﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.MovieRequest;
using MovieManagement.Payloads.DataResponses.DataMovie;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IMovieService
    {
        Task<ResponseObject<DataResponseMovie>> CreateMovie(Request_CreateMovie request);
        Task<ResponseObject<DataResponseMovie>> UpdateMovie(Request_UpdateMovie request);
        Task<string> DeleteMovie(int movieId);
        Task<PageResult<DataResponseMovie>> GetAllMovie(int pageSize, int pageNumber);
        Task<ResponseObject<DataResponseMovie>> GetMovieById(int movieId);
    }
}
