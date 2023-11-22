using Microsoft.EntityFrameworkCore;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.CinemaRequest;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class CinemaService : BaseService, ICinemaService
    {
        private readonly ISeatService _iSeatService;
        private readonly CinemaConverter _cinemaConverter;
        private readonly ResponseObject<DataResponseCinema> _responseObject;
        private readonly ResponseObject<DataResponseRoom> _responseObjectRoom;
        private readonly RoomConverter _roomConverter;
        public CinemaService(ISeatService iSeatService, CinemaConverter cinemaConverter, ResponseObject<DataResponseCinema> responseObject, ResponseObject<DataResponseRoom> responseObjectRoom, RoomConverter roomConverter)
        {
            _iSeatService = iSeatService;
            _cinemaConverter = cinemaConverter;
            _responseObject = responseObject;
            _responseObjectRoom = responseObjectRoom;
            _roomConverter = roomConverter;
        }

        public async Task<ResponseObject<DataResponseCinema>> CreateCinema(Request_CreateCinema request)
        {
            if(string.IsNullOrWhiteSpace(request.Address) || string.IsNullOrWhiteSpace(request.NameOfCinema) || string.IsNullOrWhiteSpace(request.Description))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            Cinema cinema = new Cinema();
            cinema.NameOfCinema = request.NameOfCinema;
            cinema.Address = request.Address;
            cinema.Code = new Random().Next(100000, 999999).ToString();
            cinema.Description = request.Description;
            cinema.Room = CreateListRoom(cinema.Id, request.Request_CreateRooms);
            await _context.cinemas.AddAsync(cinema);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm thông tin rạp thành công", _cinemaConverter.EntityToDTO(cinema));
        }

        public List<Room> CreateListRoom(int cinemaId, List<Request_CreateRoom> requests)
        {
            var cinema =  _context.cinemas.SingleOrDefault(x => x.Id == cinemaId);
            if (cinema == null)
            {
                return null;
            }

            List<Room> list = new List<Room>();
            foreach (var request in requests)
            {
                Room room = new Room();
                room.Name = request.Name;
                room.Description = request.Description;
                room.Capacity = request.Capacity;
                room.Type = request.Type;
                room.CinemaId = cinemaId;
                room.Code = new Random().Next(100000, 999999).ToString();
                room.Seats = _iSeatService.CreateListSeat(room.Id, request.Request_CreateSeats);
                list.Add(room);
            }
            _context.rooms.AddRange(list);
            _context.SaveChanges();
            return list;
        }

        public async Task<ResponseObject<DataResponseRoom>> CreateRoom(int cinemaId, Request_CreateRoom request)
        {
            var cinema = await _context.cinemas.SingleOrDefaultAsync(x => x.Id == cinemaId);
            if(cinema == null)
            {
                return _responseObjectRoom.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thông tin rạp", null);
            }
            Room room = new Room();
            room.Name = request.Name;
            room.Description = request.Description;
            room.Capacity = request.Capacity;
            room.Type = request.Type;
            room.CinemaId = cinemaId;
            room.Code = new Random().Next(100000, 999999).ToString();
            room.Seats = _iSeatService.CreateListSeat(room.Id, request.Request_CreateSeats);
            await _context.rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return _responseObjectRoom.ResponseSuccess("Thêm room thành công", _roomConverter.EntityToDTO(room));
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
