using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.ScheduleRequest;
using MovieManagement.Payloads.DataResponses.DataSchedule;
using MovieManagement.Payloads.DataResponses.DataSeat;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICinemaService _iCinemaService;
        private readonly ISeatService _seatService;
        private readonly ITicketService _ticketService;
        private readonly IScheduleService _scheduleService;
        private readonly IRoomService _roomService;
        private readonly IRankCustomerService _rankCustomerService;
        private readonly IPromotionService _promotionService;
        private readonly IMovieService _movieService;
        private readonly IFoodService _foodService;
        public UserController(
            ICinemaService iCinemaService,
            ISeatService seatService,
            ITicketService ticketService,
            IScheduleService scheduleService,
            IRoomService roomService,
            IRankCustomerService rankCustomerService,
            IPromotionService promotionService,
            IMovieService movieService,
            IFoodService foodService
            )
        {
            _iCinemaService = iCinemaService;
            _seatService = seatService;
            _ticketService = ticketService;
            _scheduleService = scheduleService;
            _roomService = roomService;
            _rankCustomerService = rankCustomerService;
            _promotionService = promotionService;
            _movieService = movieService;
            _foodService = foodService;
        }
        [HttpGet("GetListCinema")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> GetListCinema(int pageSize = 10, int pageNumber = 1)
        {
            pageSize = -1;
            return Ok(await _iCinemaService.GetListCinema(pageSize, pageNumber));
        }
        [HttpGet("GetAllFoods")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> GetAllFoods(int pageSize = 10, int pageNumber = 1)
        {
            pageSize = -1;
            return Ok(await _foodService.GetAllFoods(pageSize, pageNumber));
        }
        [HttpPost("CreateSchedule")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> CreateSchedule(Request_CreateSchedule request)
        {
            return Ok(await _scheduleService.CreateSchedule(request));
        }
        [HttpPut("UpdateSchedule")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> UpdateSchedule(Request_UpdateSchedule request)
        {
            return Ok(await _scheduleService.UpdateSchedule(request));
        }
        [HttpGet("GetSeatByRoom")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> GetSeatByRoom(int roomId, int pageSize = 10, int pageNumber = 1)
        {
            pageSize = -1;
            return Ok(await _seatService.GetSeatByRoom(roomId, pageSize, pageNumber));
        }
        [HttpGet("GetSeatByStatus")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> GetSeatByStatus(int statusId, int pageSize, int pageNumber)
        {
            pageSize = -1;
            return Ok(await _seatService.GetSeatByStatus(statusId, pageSize, pageNumber));
        }


    }
}
