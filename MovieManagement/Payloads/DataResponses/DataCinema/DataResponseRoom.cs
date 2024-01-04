using MovieManagement.Payloads.DataResponses.DataSchedule;
using MovieManagement.Payloads.DataResponses.DataSeat;

namespace MovieManagement.Payloads.DataResponses.DataCinema
{
    public class DataResponseRoom : DataResponseBase
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public IQueryable<DataResponseSeat>  DataResponseSeats { get; set; }
        public IQueryable<DataResponseSchedule> DataResponseSchedules { get; set;}
    }
}
