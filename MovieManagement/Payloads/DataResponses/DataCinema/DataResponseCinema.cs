using MovieManagement.Entities;

namespace MovieManagement.Payloads.DataResponses.DataCinema
{
    public class DataResponseCinema : DataResponseBase
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public string NameOfCinema { get; set; }
        public IQueryable<DataResponseRoom> Room { get; set; }
    }
}
