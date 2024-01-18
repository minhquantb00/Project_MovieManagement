using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.PromotionRequest;
using MovieManagement.Payloads.DataResponses.DataPromotion;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<ResponseObject<DataRepsonsePromotion>> CreatePromotion(Request_CreatePromotion request);
        Task<ResponseObject<DataRepsonsePromotion>> UpdatePromotion(Request_UpdatePromotion request);
        Task<PageResult<DataRepsonsePromotion>> GetAllPromotions(int pageSize, int pageNumber);
    }
}
