using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandleGenerate;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.CinemaRequest;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class RoomService : IRoomService
    {
        private readonly ResponseObject<DataResponseRoom> _responseObject;
        private readonly ISeatService _seatService;
        private readonly RoomConverter _converter;
        public readonly AppDbContext _context;
        public RoomService(ResponseObject<DataResponseRoom> responseObject, RoomConverter converter, ISeatService seatService)
        {
            _responseObject = responseObject;
            _converter = converter;
            _seatService = seatService;
            _context = new AppDbContext();
        }

        public async Task<List<Room>> CreateListRoom(int cinemaId, List<Request_CreateRoom>? requests)
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
                    Capacity = request.Capacity,
                    CinemaId = cinemaId,
                    Code = GenerateCode.GenerateBillCode(),
                    Description = request.Description,
                    Name = request.Name,
                    Type = request.Type
                };

                await _context.rooms.AddAsync(room);
                room.Seats = _seatService.CreateListSeat(room.Id, request.Request_CreateSeats);
                list.Add(room);
            }
            await _context.SaveChangesAsync();

            return list;
        }


        public async Task<ResponseObject<DataResponseRoom>> CreateRoom(int cinemaId, Request_CreateRoom request)
        {
            var cinema = await _context.cinemas.SingleOrDefaultAsync(x => x.Id == cinemaId);
            if(cinema == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy rạp phim", null);
            }
            var room = new Room
            {
                Capacity = request.Capacity,
                CinemaId = cinemaId,
                Code = GenerateCode.GenerateBillCode(),
                Description = request.Description,
                Name = request.Name,
                Type = request.Type,
            };
            await _context.rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            room.Seats = request.Request_CreateSeats == null ? null : _seatService.CreateListSeat(room.Id, request.Request_CreateSeats);
            return _responseObject.ResponseSuccess("Thêm phòng thành công", _converter.EntityToDTO(room));
        }

        public async Task<string> DeleteRoom(int roomId)
        {
            var room = await _context.rooms.SingleOrDefaultAsync(x =>x.Id == roomId);
            if(room == null)
            {
                return "Phòng không tồn tại";
            }
            room.IsActive = false;
            _context.rooms.Update(room);
            await _context.SaveChangesAsync();
            return "Xóa phòng thành công";
        }

        public async Task<PageResult<DataResponseRoom>> GetRoomList(int? cinemaId, int pageSize, int pageNumber)
        {
            var query = await _context.rooms.Include(x => x.Seats).AsNoTracking().ToListAsync();
            if (cinemaId.HasValue)
            {
                query = query.Where(x => x.CinemaId == cinemaId).ToList();
            }
            var result = Pagination.GetPagedData(query.Select(x => _converter.EntityToDTO(x)).AsQueryable(), pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponseRoom>> UpdateRoom(Request_UpdateRoom request)
        {
            var room = await _context.rooms.Include(x => x.Seats).AsNoTracking().SingleOrDefaultAsync(x => x.Id == request.RoomId);

            if (room == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }
            room.Name = request.Name;
            room.Capacity = request.Capacity;
            room.Code = GenerateCode.GenerateBillCode();
            room.Description = request.Description;
            room.Type = request.Type;
            var listSeat = _context.seats.Include(x => x.Tickets).AsNoTracking().Where(x => x.RoomId == room.Id).ToList();
            foreach (var seat in listSeat)
            {
                var ticket = _context.tickets.Include(x => x.BillTickets).Include(x => x.Schedule).AsNoTracking().Where(x => x.SeatId == seat.Id).ToList();
                _context.tickets.RemoveRange(ticket);
                _context.seats.Remove(seat);
            }   

            room.Seats = request.Request_CreateSeats == null ? null : _seatService.CreateListSeat(room.Id, request.Request_CreateSeats);

            _context.rooms.Update(room);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Cập nhật thông tin phòng thành công", _converter.EntityToDTO(room));
        }

    }
}
