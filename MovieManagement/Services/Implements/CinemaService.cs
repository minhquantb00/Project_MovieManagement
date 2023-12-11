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
        public readonly AppDbContext _context;
        public CinemaService(AppDbContext context, ISeatService iSeatService, CinemaConverter cinemaConverter, ResponseObject<DataResponseCinema> responseObject, ResponseObject<DataResponseRoom> responseObjectRoom, RoomConverter roomConverter)
        {
            _iSeatService = iSeatService;
            _cinemaConverter = cinemaConverter;
            _responseObject = responseObject;
            _responseObjectRoom = responseObjectRoom;
            _roomConverter = roomConverter;
            _context = context;
        }

        public async Task<ResponseObject<DataResponseCinema>> CreateCinema(Request_CreateCinema request)
        {
            if (string.IsNullOrWhiteSpace(request.Address) || string.IsNullOrWhiteSpace(request.NameOfCinema) || string.IsNullOrWhiteSpace(request.Description))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }

            Cinema cinema = new Cinema
            {
                NameOfCinema = request.NameOfCinema,
                Address = request.Address,
                Code = new Random().Next(100000, 999999).ToString(),
                Description = request.Description,
                Room = null
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _context.cinemas.AddAsync(cinema);
                    await _context.SaveChangesAsync();

                    cinema.Room = await CreateListRoom(cinema.Id, request.Request_CreateRooms);
                    _context.cinemas.Update(cinema);
                    await _context.SaveChangesAsync();

                    scope.Complete();

                    return _responseObject.ResponseSuccess("Thêm thông tin rạp thành công", _cinemaConverter.EntityToDTO(cinema));
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, or return an error response.
                    return _responseObject.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null);
                }
            }
        }

        // Other methods remain the same


        public async Task<List<Room>> CreateListRoom(int cinemaId, List<Request_CreateRoom> requests)
        {
            var cinema = await _context.cinemas.SingleOrDefaultAsync(x => x.Id == cinemaId);
            if (cinema == null)
            {
                return null;
            }

            List<Room> list = new List<Room>();
            foreach (var request in requests)
            {
                Room room = new Room
                {
                    Name = request.Name,
                    Description = request.Description,
                    Capacity = request.Capacity,
                    Type = request.Type,
                    CinemaId = cinemaId,
                    Code = new Random().Next(100000, 999999).ToString(),
                    Seats = null
                };

                await _context.rooms.AddAsync(room);
                await _context.SaveChangesAsync();

                room.Seats = _iSeatService.CreateListSeat(room.Id, request.Request_CreateSeats);
                _context.rooms.Update(room);
                await _context.SaveChangesAsync();

                list.Add(room);
            }
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
