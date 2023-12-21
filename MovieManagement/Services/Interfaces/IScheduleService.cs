﻿using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.ScheduleRequest;
using MovieManagement.Payloads.DataResponses.DataSchedule;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<ResponseObject<DataResponseSchedule>> CreateSchedule(Request_CreateSchedule request);
        Task<ResponseObject<DataResponseSchedule>> UpdateSchedule(Request_UpdateSchedule request);
        Task<PageResult<DataResponseSchedule>> GetSchedulesByMovie(int movieId, int pageSize, int pageNumber);
        Task<PageResult<DataResponseSchedule>> GetAlls(int pageSize, int pageNumber);
    }
}
