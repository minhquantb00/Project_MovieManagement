using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.FoodRequest;
using MovieManagement.Payloads.DataResponses.DataFood;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IFoodService
    {
        Task<ResponseObject<DataResponseFood>> CreateFood(Request_CreateFood request);
        Task<ResponseObject<DataResponseFood>> UpdateFood(Request_UpdateFood request);
        Task<PageResult<DataResponseFood>> GetAllFoods(int pageSize, int pageNumber);
    }
}
