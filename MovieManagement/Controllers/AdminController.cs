using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Payloads.DataRequests.CinemaRequest;
using MovieManagement.Payloads.DataRequests.MovieRequest;
using MovieManagement.Payloads.DataRequests.PromotionRequest;
using MovieManagement.Payloads.DataRequests.RankCustomerRequest;
using MovieManagement.Payloads.DataRequests.SeatRequest;
using MovieManagement.Payloads.DataRequests.TicketRequest;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.DataResponses.DataMovie;
using MovieManagement.Payloads.DataResponses.DataPromotion;
using MovieManagement.Payloads.DataResponses.DataRankCustomer;
using MovieManagement.Payloads.DataResponses.DataSeat;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
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
        public AdminController(
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
        [HttpPost("CreateCinema")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateCinema(Request_CreateCinema request)
        {
            return Ok(await _iCinemaService.CreateCinema(request));
        }
        [HttpPost("CreateRoom")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateRoom(int cinemaId, Request_CreateRoom request)
        {
            return Ok(await _roomService.CreateRoom(cinemaId, request));
        }
        [HttpPost("CreateListRoom")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateListRoom(int cinemaId, List<Request_CreateRoom> requests)
        {
            return Ok(await _roomService.CreateListRoom(cinemaId, requests));
        }
        [HttpPost("CreateSeat")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateSeat(int roomId, Request_CreateSeat request)
        {
            return Ok(await _seatService.CreateSeat(roomId, request));
        }
        [HttpPut("UpdateSeat")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateSeat(int roomId, List<Request_UpdateSeat> requests)
        {
            return Ok(await _seatService.UpdateSeat(roomId, requests));
        }
        [HttpPost("CreateMovie")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateMovie([FromForm] Request_CreateMovie request)
        {
            return Ok(await _movieService.CreateMovie(request));
        }
        [HttpPut("DeleteMovie")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            return Ok(await _movieService.DeleteMovie(movieId));
        }
        [HttpPut("UpdateMovie")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateMovie(Request_UpdateMovie request)
        {
            return Ok(await _movieService.UpdateMovie(request));
        }
        [HttpPost("CreatePromotion")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreatePromotion(Request_CreatePromotion request)
        {
            return Ok(await _promotionService.CreatePromotion(request));
        }
        [HttpPut("UpdatePromotion")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdatePromotion(Request_UpdatePromotion request)
        {
            return Ok(await _promotionService.UpdatePromotion(request));
        }
        [HttpPost("CreateRankCustomer")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateRankCustomer(Request_CreateRankCustomer request)
        {
            return Ok(await _rankCustomerService.CreateRankCustomer(request));
        }
        [HttpPut("UpdateRankCustomer")]
        //[Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateRankCustomer(Request_UpdateRankCustomer request)
        {
            return Ok(await _rankCustomerService.UpdateRankCustomer(request));
        }


    }
}
