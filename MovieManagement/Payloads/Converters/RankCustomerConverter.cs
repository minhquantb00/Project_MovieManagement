using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataRankCustomer;

namespace MovieManagement.Payloads.Converters
{
    public class RankCustomerConverter
    {
        public DataResponseRankCustomer EntityToDTO(RankCustomer rankCustomer)
        {
            return new DataResponseRankCustomer
            {
                Description = rankCustomer.Description,
                Id = rankCustomer.Id,
                Name = rankCustomer.Name,
                Point = rankCustomer.Point,
            };
        }
    }
}
