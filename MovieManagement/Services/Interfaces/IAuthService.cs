using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.StatisticRequest;
using MovieManagement.Payloads.DataRequests.TokenRequest;
using MovieManagement.Payloads.DataRequests.UserRequest;
using MovieManagement.Payloads.DataResponses.DataToken;
using MovieManagement.Payloads.DataResponses.DataUser;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseObject<DataResponseUser>> Register(Request_Register request);
        Task<ResponseObject<DataResponseToken>> Login(Request_Login request);
        DataResponseToken GenerateAccessToken(User user);
        ResponseObject<DataResponseToken> RenewAccessToken(Request_RenewToken request);
        string GenerateRefreshToken();
        Task<ResponseObject<DataResponseUser>> ConfirmCreateNewAccount(Request_ConfirmCreateAccount request);
        Task<ResponseObject<DataResponseUser>> ChangePassword(int userId, Request_ChangePassword request);
        Task<string> ForgotPassword(Request_ForgotPassword request);
        Task<string> ConfirmCreateNewPassword(Request_ConfirmCreateNewPassword request);
        Task<ResponseObject<DataResponseUser>> ChangeDecentralization(Request_ChangeDecentralization request);
        Task<ResponseObject<DataResponseUser>> UpdateUserInformation(int userId, Request_UpdateUserInformation request);
        Task<PageResult<DataResponseUser>> GetAllUsers(InputUser input, int pageSize, int pageNumber);
        Task<PageResult<DataResponseUser>> GetListUserByRank(int pageSize, int pageNumber);
        Task<PageResult<DataResponseUser>> GetUserByName(string name, int pageSize, int pageNumber);
    }
}
