using MovieManagement.Entities;

namespace MovieManagement.Payloads.DataResponses.DataMovie
{
    public class DataResponseMovieType : DataResponseBase
    {
        public string MovieTypeName { get; set; }
        public IQueryable<DataResponseMovie> Movies { get; set; }
    }
}
