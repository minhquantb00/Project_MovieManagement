using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.BillRequest;
using MovieManagement.Payloads.DataRequests.CinemaRequest;
using MovieManagement.Payloads.DataRequests.FoodRequest;
using MovieManagement.Payloads.DataRequests.MovieRequest;
using MovieManagement.Payloads.DataRequests.SeatRequest;
using MovieManagement.Payloads.DataRequests.StatisticRequest;
using MovieManagement.Payloads.DataRequests.TicketRequest;
using MovieManagement.Payloads.DataResponses.DataBill;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.DataResponses.DataFood;
using MovieManagement.Payloads.DataResponses.DataMovie;
using MovieManagement.Payloads.DataResponses.DataSchedule;
using MovieManagement.Payloads.DataResponses.DataSeat;
using MovieManagement.Payloads.DataResponses.DataStatistics;
using MovieManagement.Payloads.DataResponses.DataTicket;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
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
        private readonly IBillService _billService;
        public StaffController(
            ICinemaService iCinemaService,
            ISeatService seatService,
            ITicketService ticketService,
            IScheduleService scheduleService,
            IRoomService roomService,
            IRankCustomerService rankCustomerService,
            IPromotionService promotionService,
            IMovieService movieService,
            IFoodService foodService,
            IBillService billService
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
            _billService = billService;
        }

        [HttpPost("CreateTicket")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> CreateTicket(int scheduleId, Request_CreateTicket request)
        {
            return Ok(await _ticketService.CreateTicket(scheduleId, request));
        }
        [HttpPost("UpdateTicket")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> UpdateTicket(Request_UpdateTicket request)
        {
            return Ok(await _ticketService.UpdateTicket(request));
        }


        [HttpGet("GetListRoomInCinema")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> GetListRoomInCinema(int cinema, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iCinemaService.GetListRoomInCinema(cinema, pageSize, pageNumber));
        }
        [HttpPost("CreateFood")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateFood([FromForm] Request_CreateFood request)
        {
            return Ok(await _foodService.CreateFood(request));
        }

        [HttpPut("UpdateFood")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateFood(Request_UpdateFood request)
        {
            return Ok(await _foodService.UpdateFood(request));
        }
        [HttpGet("GetMovieById")]
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            return Ok(await _movieService.GetMovieById(movieId));
        }
        [HttpGet("GetAllMovie")]
        public async Task<IActionResult> GetAllMovie([FromQuery] InputFilter input, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieService.GetAllMovie(input, pageSize, pageNumber));
        }
        [HttpPost("CreateListRoom")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> CreateListRoom(int cinemaId, List<Request_CreateRoom> requests)
        {
            return Ok(await _roomService.CreateListRoom(cinemaId, requests));
        }
        [HttpPost("CreateRoom")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> CreateRoom(int cinemaId, Request_CreateRoom request)
        {
            return Ok(await _roomService.CreateRoom(cinemaId, request));
        }
        [HttpGet("GetRoomList")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> GetRoomList(int? cinemaId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _roomService.GetRoomList(cinemaId, pageSize, pageNumber));
        }
        [HttpPut("UpdateRoom")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> UpdateRoom(Request_UpdateRoom request)
        {
            return Ok(await _roomService.UpdateRoom(request));
        }
        [HttpPost("CreateListSeat")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public IActionResult CreateListSeat(int roomId, List<Request_CreateSeat> requests)
        {
            return Ok(_seatService.CreateListSeat(roomId, requests));
        }
        [HttpPost("CreateSeat")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> CreateSeat(int roomId, Request_CreateSeat request)
        {
            return Ok(await _seatService.CreateSeat(roomId, request));
        }
        [HttpPut("UpdateSeat")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> UpdateSeat(int roomId, List<Request_UpdateSeat> requests)
        {
            return Ok(await _seatService.UpdateSeat(roomId, requests));
        }
        [HttpGet("GetSchedulesByMovie")]
        public async Task<IActionResult> GetSchedulesByMovie(int movieId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _scheduleService.GetSchedulesByMovie(movieId, pageSize, pageNumber));
        }
        [HttpPut("DeleteSchedule/{scheduleId}")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> DeleteSchedule([FromRoute] int scheduleId)
        {
            return Ok(await _scheduleService.DeleteSchedule(scheduleId));
        }
        [HttpGet("GetPaymentHistoryByBillId")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> GetPaymentHistoryByBillId(int billId)
        {
            return Ok(await _billService.GetPaymentHistoryByBillId(billId));
        }
        [HttpGet("GetAllBills")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> GetAllBills(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _billService.GetAllBills(pageSize, pageNumber));
        }
        [HttpPost("CreateListTicket")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public IActionResult CreateListTicket(int scheduleId, List<Request_CreateTicket> requests)
        {
            return Ok(_ticketService.CreateListTicket(scheduleId, requests));
        }
        [HttpGet("SalesStatistics")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> SalesStatistics([FromQuery] InputStatistic input)
        {
            return Ok(await _billService.SalesStatistics(input));
        }
        [HttpGet("SalesStatisticsFood")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> SalesStatisticsFood([FromQuery] InputFoodStatistics input)
        {
            return Ok(await _billService.SalesStatisticsFood(input));
        }
    }
}
