using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.BannerRequest;
using MovieManagement.Payloads.DataResponses.DataBanner;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IBannerService
    {
        Task<ResponseObject<DataResponseBanner>> CreateBanner(Request_CreateBanner request);
        Task<ResponseObject<DataResponseBanner>> UpdateBanner(Request_UpdateBanner request);
        Task<string> DeleteBanner(int bannerId);
        Task<PageResult<DataResponseBanner>> GetAllBanners(int pageSize, int pageNumber);
        Task<ResponseObject<DataResponseBanner>> GetBannerById(int bannerId);
    }
}
