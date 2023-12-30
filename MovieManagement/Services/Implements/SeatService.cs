using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.SeatRequest;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.DataResponses.DataSeat;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class SeatService : ISeatService
    {
        private readonly SeatConverter _seatConverter;
        private readonly ResponseObject<DataResponseRoom> _responseObjectRoom;
        private readonly RoomConverter _roomConverter;
        private readonly ResponseObject<DataResponseSeat> _responseObject;
        public readonly AppDbContext _context;
        public SeatService(SeatConverter seatConverter, ResponseObject<DataResponseSeat> responseObject, ResponseObject<DataResponseRoom> responseObjectRoom, RoomConverter roomConverter)
        {
            _seatConverter = seatConverter;
            _responseObject = responseObject;
            _responseObjectRoom = responseObjectRoom;
            _roomConverter = roomConverter;
            _context = new AppDbContext();
        }

        public List<Seat> CreateListSeat(int roomId, List<Request_CreateSeat> requests)
        {
            var room = _context.rooms.SingleOrDefault(x => x.Id == roomId);
            if(room == null)
            {
                return null;
            }
            List<Seat> list = new List<Seat>();
            foreach (var request in requests)
            {
                Seat seat = new Seat();
                seat.SeatStatusId = 1;
                seat.Line = request.Line;
                seat.Number = request.Number;
                seat.RoomId = roomId;
                seat.SeatTypeId = request.SeatTypeId;
                seat.IsActive = true;
                list.Add(seat);
            }
            _context.seats.AddRange(list);
            _context.SaveChanges();
            return list;
        }

        public async Task<ResponseObject<DataResponseSeat>> CreateSeat(int roomId, Request_CreateSeat request)
        {
            var room = await _context.rooms.SingleOrDefaultAsync(x => x.Id == roomId);
            if(room == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }
            Seat seat = new Seat();
            seat.Line = request.Line;
            seat.Number = request.Number;
            seat.SeatTypeId = request.SeatTypeId;
            seat.RoomId = roomId;
            seat.SeatStatusId = 1; // ghế còn trống
            await _context.seats.AddAsync(seat);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm ghế thành công", _seatConverter.EntityToDTO(seat));
        }

        public async Task<PageResult<DataResponseSeat>> GetAllSeat(int pageSize, int pageNumber)
        {
            var query = _context.seats.Select(x => _seatConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<DataResponseSeat>> GetSeatByRoom(int roomId, int pageSize, int pageNumber)
        {
            var query = _context.seats.Where(x => x.RoomId == roomId).Select(x => _seatConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<DataResponseSeat>> GetSeatByStatus(int statusId, int pageSize, int pageNumber)
        {
            var query = _context.seats.Where(x => x.SeatStatusId == statusId).Select(x => _seatConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponseRoom>> UpdateSeat(int roomId, List<Request_UpdateSeat> requests)
        {
            var room = await _context.rooms.Include(x => x.Seats).SingleOrDefaultAsync(x => x.Id == roomId);
            if (room == null)
            {
                return _responseObjectRoom.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }

            var seatDict = room.Seats.ToDictionary(s => s.Id, s => s);

            foreach (var request in requests)
            {
                if (!seatDict.TryGetValue(request.SeatId, out var seat))
                {
                    return _responseObjectRoom.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy ghế", null);
                }
                seat.SeatStatusId = request.SeatStatusId;
                seat.RoomId = roomId;
                seat.Number = request.Number;
                seat.Line = request.Line;
                seat.SeatTypeId = request.SeatTypeId;

                _context.seats.Update(seat);
            }

            await _context.SaveChangesAsync();
            return _responseObjectRoom.ResponseSuccess("Cập nhật thông tin ghế trong phòng thành công", _roomConverter.EntityToDTO(room));
        }


    }
}
