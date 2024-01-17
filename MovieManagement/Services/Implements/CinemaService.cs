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
            cinema.Room = request.Request_CreateRooms == null ? null : await _roomService.CreateListRoom(cinema.Id, request.Request_CreateRooms);
            _context.cinemas.Update(cinema);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm rạp thành công", _cinemaConverter.EntityToDTO(cinema));
        }

        public async Task<ResponseObject<DataResponseCinema>> UpdateCinema(Request_UpdateCinema request)
        {
            var cinema = await _context.cinemas.SingleOrDefaultAsync(x => x.Id == request.CinemaId);
            if (cinema == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy rạp", null);
            }

            cinema.Address = request.Address;
            cinema.NameOfCinema = request.NameOfCinema;
            cinema.Code = request.Code;
            cinema.Description = request.Description;

            if (cinema.Room != null)
            {
                _context.rooms.RemoveRange(cinema.Room);
            }

            _context.cinemas.Update(cinema);
            await _context.SaveChangesAsync();

            if (request.Request_UpdateRooms != null)
            {
                var rooms = await _roomService.CreateListRoom(cinema.Id, request.Request_UpdateRooms);
                cinema.Room = rooms;
            }
            else
            {
                cinema.Room = null;
            }

            _context.cinemas.Update(cinema);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Cập nhật thông tin rạp thành công", _cinemaConverter.EntityToDTO(cinema));
        }


        public async Task<PageResult<DataResponseCinema>> GetCinemaByMovie(int movieId, int pageSize, int pageNumber)
        {
            var movie = await _context.movies.Include(x => x.Schedules).SingleOrDefaultAsync(x => x.Id == movieId);
            if (movie == null)
            {
                throw new ArgumentNullException("Phim không tồn tại");
            }

            var schedules = await _context.schedules.Include(x => x.Room)
                                                    .ThenInclude(r => r.Cinema)
                                                    .Where(x => x.MovieId == movieId)
                                                    .ToListAsync();

            var groupedByCinema = schedules.GroupBy(s => s.Room.CinemaId);

            var cinemas = new List<DataResponseCinema>();

            foreach (var group in groupedByCinema)
            {
                var firstSchedule = group.First();
                var cinema = firstSchedule.Room.Cinema;
                var cinemaDTO = new DataResponseCinema
                {
                    NameOfCinema = cinema.NameOfCinema,
                    Address = cinema.Address,
                    Description = cinema.Description,
                    Room = group.Select(s => _roomConverter.EntityToDTO(s.Room)).AsQueryable(),
                    Id = firstSchedule.Id,
                };

                cinemas.Add(cinemaDTO);
            }

            var result = Pagination.GetPagedData(cinemas.AsQueryable(), pageSize, pageNumber);
            return result;
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
