using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataSchedule;

namespace MovieManagement.Payloads.Converters
{
    public class SchedulesConverter
    {
        private readonly AppDbContext _context;
        public SchedulesConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseSchedule EntityToDTO(Schedule schedule)
        {
            return new DataResponseSchedule
            {
                Id = schedule.Id,
                MovieName = _context.movies.SingleOrDefault(x => x.Id == schedule.MovieId).Name,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                Name = schedule.Name,
                Price = schedule.Price,
                RoomName = _context.rooms.SingleOrDefault(x => x.Id == schedule.RoomId).Name
            };
        }
    }
}
