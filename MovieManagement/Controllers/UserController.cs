using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.BillRequest;
using MovieManagement.Payloads.DataRequests.ScheduleRequest;
using MovieManagement.Payloads.DataResponses.DataBanner;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.DataResponses.DataMovie;
using MovieManagement.Payloads.DataResponses.DataPromotion;
using MovieManagement.Payloads.DataResponses.DataSchedule;
using MovieManagement.Payloads.DataResponses.DataSeat;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Implements;
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
        private readonly IBillService _billService;
        private readonly IVNPayService _vnpayService;
        public UserController(
            ICinemaService iCinemaService,
            ISeatService seatService,
            ITicketService ticketService,
            IScheduleService scheduleService,
            IRoomService roomService,
            IRankCustomerService rankCustomerService,
            IPromotionService promotionService,
            IMovieService movieService,
            IFoodService foodService,
            IBillService billService,
            IVNPayService vnpayService
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
            _vnpayService = vnpayService;
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

        [HttpPost("CreateListBillTicket")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> CreateListBillTicket(int billId, List<Request_CreateBillTicket> requests)
        {
            return Ok(await _billService.CreateListBillTicket(billId, requests));
        }
        [HttpPost("CreateListBillFood")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> CreateListBillFood(int billId, List<Request_CreateBillFood> requests)
        {
            return Ok(await _billService.CreateListBillFood(billId, requests));
        }
        [HttpPost("CreateBillTicket")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> CreateBillTicket(int billId, Request_CreateBillTicket request)
        {
            return Ok(await _billService.CreateBillTicket(billId, request));
        }
        [HttpPost("CreateBillFood")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> CreateBillFood(int billId, Request_CreateBillFood request)

        {
            return Ok(await _billService.CreateBillFood(billId, request));
        }
        [HttpPost("CreateBill")]
        [Authorize(Roles = "Admin, Manager, Staff, User")]
        public async Task<IActionResult> CreateBill(Request_CreateBill request)
        {
            return Ok(await _billService.CreateBill(request));
        }

        [HttpGet]
        [Route("/Vnpay/return")]
        public async Task<IActionResult> Return()
        {
            var vnpayData = HttpContext.Request.Query;

            return Ok(await _vnpayService.VNPayReturn(vnpayData));
        }
        [HttpPost]
        [Route("/Vnpay/CreatePaymentUrl")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreatePaymentUrl(int billId)
        {
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _vnpayService.CreatePaymentUrl(billId, HttpContext, id));
        }
        [HttpGet("GetAllMovieTypes")]
        public async Task<IActionResult> GetAllMovieTypes(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _movieService.GetAllMovieTypes(pageSize, pageNumber));
        }
        [HttpGet("GetMovieTypeById/{movieTypeId}")]
        public async Task<IActionResult> GetMovieTypeById([FromRoute] int movieTypeId)
        {
            return Ok(await _movieService.GetMovieTypeById(movieTypeId));
        }
        [HttpGet("GetAllSchedules")]
        public async Task<IActionResult> GetAllSchedules([FromQuery] InputScheduleData input, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _scheduleService.GetAlls(input, pageSize, pageNumber));
        }

        [HttpGet("GetSchedulesByDay")]
        public async Task<IActionResult> GetSchedulesByDay(DateTime startAt, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _scheduleService.GetSchedulesByDay(startAt, pageSize, pageNumber));
        }
        [HttpGet("GetCinemaByMovie")]
        public async Task<IActionResult> GetCinemaByMovie(int movieId, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iCinemaService.GetCinemaByMovie(movieId, pageSize, pageNumber));
        }
        [HttpGet("GetAllPromotions")]
        public async Task<IActionResult> GetAllPromotions(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _promotionService.GetAllPromotions(pageSize, pageNumber));
        }
    }
}
