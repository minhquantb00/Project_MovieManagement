using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.ScheduleRequest;
using MovieManagement.Payloads.DataResponses.DataSchedule;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class ScheduleService : IScheduleService
    {
        private readonly ResponseObject<DataResponseSchedule> _responseObject;
        private readonly SchedulesConverter _converter;
        public readonly AppDbContext _context;
        public ScheduleService(SchedulesConverter converter, ResponseObject<DataResponseSchedule> responseObject)
        {
            _converter = converter;
            _responseObject = responseObject;
            _context = new AppDbContext();
        }

        public async Task<ResponseObject<DataResponseSchedule>> CreateSchedule(Request_CreateSchedule request)
        {
            var room = await _context.rooms.SingleOrDefaultAsync(x => x.Id == request.RoomId);
            if(room == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }
            var movie = await _context.movies.SingleOrDefaultAsync(x => x.Id == request.MovieId);
            if(movie == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thông tin", null);
            }
            var schedule = new Schedule();
            schedule.RoomId = room.Id;
            schedule.MovieId = movie.Id;
            schedule.Code = "MyBugs__" + DateTime.Now.Ticks.ToString() + "xyz_" + new Random().Next(1000,9999);
            schedule.StartAt = request.StartAt;
            schedule.EndAt = request.EndAt;
            schedule.Price = request.Price;
            schedule.Name = request.Name;
            if(_context.schedules.Any(x => !((request.StartAt < x.StartAt && request.EndAt < x.StartAt) || (request.StartAt > x.EndAt && request.EndAt > x.EndAt)) && x.RoomId == request.RoomId ))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Lịch chiếu bị trùng", null);
            }
            await _context.schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm lịch trình thành công", _converter.EntityToDTO(schedule));
        }

        public async Task<ResponseObject<DataResponseSchedule>> UpdateSchedule(Request_UpdateSchedule request)
        {
            var schedule = await _context.schedules.SingleOrDefaultAsync(x => x.Id == request.ScheduleId);
            if(schedule == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Lịch trình không tồn tại", null);
            }
            if(!_context.rooms.Any(x => x.Id == request.RoomId))
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy phòng", null);
            }
            if(!_context.movies.Any(x => x.Id == request.MovieId))
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy phim", null);
            }
            schedule.StartAt = request.StartAt;
            schedule.Price = request.Price;
            schedule.Price = request.Price;
            schedule.Name = request.Name;
            schedule.Code = "MyBugs__" + DateTime.Now.Ticks.ToString() + "_xyz_" + new Random().Next(100, 999);
            schedule.MovieId = request.MovieId;
            schedule.RoomId = request.RoomId;
            _context.schedules.Update(schedule);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật thông tin lịch trình thành công", _converter.EntityToDTO(schedule));
        }

        public async Task<PageResult<DataResponseSchedule>> GetSchedulesByMovie(int movieId, int pageSize, int pageNumber)
        {
            var query = _context.schedules.Where(x => x.MovieId == movieId).Select(x => _converter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<PageResult<DataResponseSchedule>> GetAlls(InputScheduleData input, int pageSize, int pageNumber)
        {
            var query = await _context.schedules.Include(x => x.Room).ToListAsync();
            if (input.RoomId.HasValue)
            {
                query = query.Where(x => x.RoomId == input.RoomId).ToList();
            }
            var result = Pagination.GetPagedData(query.Select(x => _converter.EntityToDTO(x)).AsQueryable(), pageSize, pageNumber);
            return result;
        }

        public async Task<string> DeleteSchedule(int scheduleId)
        {
            var schedule = await _context.schedules.SingleOrDefaultAsync(x => x.Id == scheduleId);
            if(schedule.EndAt < DateTime.Now && schedule != null)
            {
                schedule.IsActive = false;
                _context.schedules.Update(schedule);
                await _context.SaveChangesAsync();
                return "Xóa bản ghi thành công";
            }
            return "Lịch trình không tồn tại";

        }

        public async Task<PageResult<DataResponseSchedule>> GetSchedulesByDay(DateTime startAt, int pageSize, int pageNumber)
        {
            var query = _context.schedules
                .Include(x => x.Movie)
                .AsNoTracking()
                .Where(x => x.StartAt.Date == startAt.Date)
                .Select(x => _converter.EntityToDTO(x));

            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

    }
}
