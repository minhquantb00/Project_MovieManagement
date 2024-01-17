using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.PaginationInputRequest;
using MovieManagement.Payloads.DataRequests.StatisticRequest;
using MovieManagement.Payloads.DataRequests.TokenRequest;
using MovieManagement.Payloads.DataRequests.UserRequest;
using MovieManagement.Payloads.DataResponses.DataUser;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _iAuthService;
        public AuthController(IAuthService iAuthService)
        {
            _iAuthService = iAuthService;
        }
        [HttpPost("/api/auth/Register")]
        public async Task<IActionResult> Register([FromBody] Request_Register request)
        {
            var result = await _iAuthService.Register(request);
            if(result.Status == 404)
            {
                return NotFound(result);
            }
            else if(result.Status == 400)
            {
                return BadRequest(result);
            }
            return Ok(request);
        }
        [HttpPost("/api/auth/Login")]
        public async Task<IActionResult> Login([FromBody] Request_Login request)
        {
            var result = await _iAuthService.Login(request);
            if (result.Status == 404)
            {
                return NotFound(result);
            }
            else if (result.Status == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("/api/auth/ConfirmCreateNewAccount")]
        public async Task<IActionResult> ConfirmCreateNewAccount([FromBody] Request_ConfirmCreateAccount request)
        {
            return Ok(await _iAuthService.ConfirmCreateNewAccount(request));
        }
        [HttpPost("/api/auth/RenewAccessToken")]
        public IActionResult RenewAccessToken([FromBody] Request_RenewToken request)
        {
            return Ok( _iAuthService.RenewAccessToken(request));
        }
        [HttpPut("/api/auth/ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(Request_ChangePassword request)
        {
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _iAuthService.ChangePassword(id, request));
        }
        [HttpPut("/api/auth/UpdateUserInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserInformation(Request_UpdateUserInformation request)
        {
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _iAuthService.UpdateUserInformation(id, request));
        }
        [HttpPut("/api/auth/ChangeDecentralization")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeDecentralization(Request_ChangeDecentralization request)
        {
            return Ok(await _iAuthService.ChangeDecentralization(request));
        }
        [HttpPut("/api/auth/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(Request_ForgotPassword request)
        {
            return Ok(await _iAuthService.ForgotPassword(request));
        }
        [HttpPut("/api/auth/ConfirmCreateNewPassword")]
        public async Task<IActionResult> ConfirmCreateNewPassword(Request_ConfirmCreateNewPassword request)
        {
            return Ok(await _iAuthService.ConfirmCreateNewPassword(request));
        }

        [HttpGet("/api/auth/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] InputUser input, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iAuthService.GetAllUsers(input, pageSize, pageNumber));
        }
        [HttpPost("/api/auth/GetListUserByRank")]
        public async Task<IActionResult> GetListUserByRank([FromBody] PaginationInputUser input)
        {
            return Ok(await _iAuthService.GetListUserByRank(input.PageSize, input.PageNumber));
        }
        [HttpPost("/api/auth/GetUserByName")]
        public async Task<IActionResult> GetUserByName( string name, PaginationInputUser input)
        {
            return Ok(await _iAuthService.GetUserByName(name, input.PageSize, input.PageNumber));
        }
    }
}
