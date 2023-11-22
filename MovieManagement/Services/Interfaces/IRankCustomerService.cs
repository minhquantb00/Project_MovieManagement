using MovieManagement.Payloads.DataRequests.RankCustomerRequest;
using MovieManagement.Payloads.DataResponses.DataRankCustomer;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IRankCustomerService
    {
        Task<ResponseObject<DataResponseRankCustomer>> CreateRankCustomer(Request_CreateRankCustomer request);
        Task<ResponseObject<DataResponseRankCustomer>> UpdateRankCustomer(Request_UpdateRankCustomer request);
    }
}
