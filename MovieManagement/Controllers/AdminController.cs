using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.DataRequests.BannerRequest;
using MovieManagement.Payloads.DataRequests.CinemaRequest;
using MovieManagement.Payloads.DataRequests.MovieRequest;
using MovieManagement.Payloads.DataRequests.PromotionRequest;
using MovieManagement.Payloads.DataRequests.RankCustomerRequest;
using MovieManagement.Payloads.DataRequests.SeatRequest;
using MovieManagement.Payloads.DataRequests.TicketRequest;
using MovieManagement.Payloads.DataResponses.DataBanner;
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
        private readonly IBannerService _bannerService;
        public AdminController(
            ICinemaService iCinemaService, 
            ISeatService seatService, 
            ITicketService ticketService,
            IScheduleService scheduleService,
            IRoomService roomService,
            IRankCustomerService rankCustomerService,
            IPromotionService promotionService,
            IMovieService movieService,
            IFoodService foodService,
            IBannerService bannerService
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
            _bannerService = bannerService;
        }
        [HttpPost("CreateCinema")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateCinema(Request_CreateCinema request)
        {
            return Ok(await _iCinemaService.CreateCinema(request));
        }
        [HttpPost("UpdateCinema")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateCinema(Request_UpdateCinema request)
        {
            return Ok(await _iCinemaService.UpdateCinema(request));
        }
        [HttpPost("CreateRoom")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateRoom(int cinemaId, Request_CreateRoom request)
        {
            return Ok(await _roomService.CreateRoom(cinemaId, request));
        }
        [HttpPost("CreateListRoom")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateListRoom(int cinemaId, List<Request_CreateRoom> requests)
        {
            return Ok(await _roomService.CreateListRoom(cinemaId, requests));
        }
        [HttpPost("CreateSeat")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateSeat(int roomId, Request_CreateSeat request)
        {
            return Ok(await _seatService.CreateSeat(roomId, request));
        }
        [HttpPut("UpdateSeat")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateSeat(int roomId, List<Request_UpdateSeat> requests)
        {
            return Ok(await _seatService.UpdateSeat(roomId, requests));
        }
        [HttpPost("CreateMovie")]
        [Authorize(Roles = "Admin, Manager")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateMovie([FromForm] Request_CreateMovie request)
        {
            return Ok(await _movieService.CreateMovie(request));
        }
        [HttpPut("DeleteMovie")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            return Ok(await _movieService.DeleteMovie(movieId));
        }
        [HttpPut("UpdateMovie")]
        [Authorize(Roles = "Admin, Manager")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateMovie([FromForm] Request_UpdateMovie request)
        {
            return Ok(await _movieService.UpdateMovie(request));
        }
        [HttpPost("CreatePromotion")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreatePromotion(Request_CreatePromotion request)
        {
            return Ok(await _promotionService.CreatePromotion(request));
        }
        [HttpPut("UpdatePromotion")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdatePromotion(Request_UpdatePromotion request)
        {
            return Ok(await _promotionService.UpdatePromotion(request));
        }
        [HttpPost("CreateRankCustomer")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateRankCustomer(Request_CreateRankCustomer request)
        {
            return Ok(await _rankCustomerService.CreateRankCustomer(request));
        }
        [HttpPut("UpdateRankCustomer")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateRankCustomer(Request_UpdateRankCustomer request)
        {
            return Ok(await _rankCustomerService.UpdateRankCustomer(request));
        }
        [HttpPost("CreateMovieType")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateMovieType(Request_CreateMovieType request)
        {
            return Ok(await _movieService.CreateMovieType(request));
        }
        [HttpPut("UpdateMovieType")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateMovieType(Request_UpdateMovieType request)
        {
            return Ok(await _movieService.UpdateMovieType(request));
        }
        [HttpPut("DeleteMovieType/{movieTypeId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteMovieType([FromRoute] int movieTypeId)
        {
            return Ok(await _movieService.DeleteMovieType(movieTypeId));
        }
        [HttpPost("CreateBanner")]
        [Authorize(Roles = "Admin, Manager")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> CreateBanner(Request_CreateBanner request)
        {
            return Ok(await _bannerService.CreateBanner(request));
        }
        [HttpDelete("DeleteBanner/{bannerId}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteBanner([FromRoute] int bannerId)
        {
            return Ok(await _bannerService.DeleteBanner(bannerId));
        }
        [HttpGet("GetAllBanners")]
        public async Task<IActionResult> GetAllBanners(int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _bannerService.GetAllBanners(pageSize, pageNumber));
        }
        [HttpGet("GetBannerById/{bannerId}")]
        public async Task<IActionResult> GetBannerById([FromRoute] int bannerId)
        {
            return Ok(await _bannerService.GetBannerById(bannerId));
        }
        [HttpPut("UpdateBanner")]
        [Authorize(Roles = "Admin, Manager")]
        [Consumes(contentType: "multipart/form-data")]
        public async Task<IActionResult> UpdateBanner(Request_UpdateBanner request)
        {
            return Ok(await _bannerService.UpdateBanner(request));
        }
    }
}
