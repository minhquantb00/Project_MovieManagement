using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataUser;

namespace MovieManagement.Payloads.DataResponses.DataRankCustomer
{
    public class DataResponseRankCustomer : DataResponseBase
    {
        public int Point { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public IQueryable<DataResponseUser> Users { get; set; }
    }
}
