﻿using Microsoft.AspNetCore.Mvc.RazorPages;
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
        Task<ResponseObject<DataResponseRoom>> CreateRoom(int cinemaId, Request_CreateRoom request);
        List<Room> CreateListRoom(int cinemaId, List<Request_CreateRoom> requests);
        Task<PageResult<DataResponseRoom>> GetListRoomInCinema(int cinema, int pageSize, int pageNumber);
        Task<PageResult<DataResponseCinema>> GetListCinema(int pageSize, int pageNumber);
    }
}
