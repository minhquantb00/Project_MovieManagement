using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.CinemaRequest;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;
using System.Transactions;

namespace MovieManagement.Services.Implements
{
    public class CinemaService : ICinemaService
    {
        private readonly ISeatService _iSeatService;
        private readonly CinemaConverter _cinemaConverter;
        private readonly ResponseObject<DataResponseCinema> _responseObject;
        private readonly ResponseObject<DataResponseRoom> _responseObjectRoom;
        private readonly RoomConverter _roomConverter;
        private readonly AppDbContext _context;
        private readonly IRoomService _roomService;

        public CinemaService(ISeatService iSeatService, CinemaConverter cinemaConverter, ResponseObject<DataResponseCinema> responseObject, ResponseObject<DataResponseRoom> responseObjectRoom, RoomConverter roomConverter, IRoomService roomService)
        {
            _iSeatService = iSeatService;
            _cinemaConverter = cinemaConverter;
            _responseObject = responseObject;
            _responseObjectRoom = responseObjectRoom;
            _roomConverter = roomConverter;
            _context = new AppDbContext();
            _roomService = roomService;
        }

        public async Task<ResponseObject<DataResponseCinema>> CreateCinema(Request_CreateCinema request)
        {
            if(string.IsNullOrWhiteSpace(request.Address) || string.IsNullOrWhiteSpace(request.Description) || string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.NameOfCinema))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            var cinema = new Cinema
            {
                Address = request.Address,
                Code = request.Code,
                Description = request.Description,
                NameOfCinema = request.NameOfCinema,
                Room = null,
                IsActive = true,
            };
            await _context.cinemas.AddAsync(cinema);
            await _context.SaveChangesAsync();
            cinema.Room = await _roomService.CreateListRoom(cinema.Id, request.Request_CreateRooms);
            _context.cinemas.Update(cinema);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm rạp thành công", _cinemaConverter.EntityToDTO(cinema));
        }

        public async Task<PageResult<DataResponseCinema>> GetListCinema(int pageSize, int pageNumber)
        {
            var query = _context.cinemas.Include(x => x.Room).Select(x => _cinemaConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<DataResponseRoom>> GetListRoomInCinema(int cinema, int pageSize, int pageNumber)
        {
            var query = _context.rooms.Include(x => x.Cinema).Include(x => x.Seats).Include(x => x.Schedules).AsNoTracking().Select(x => _roomConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
    }
}
