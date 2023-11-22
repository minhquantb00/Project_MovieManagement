using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataPromotion;

namespace MovieManagement.Payloads.Converters
{
    public class PromotionConverter
    {
        public DataRepsonsePromotion EntityToDTO(Promotion promotion)
        {
            return new DataRepsonsePromotion
            {
                Description = promotion.Description,
                EndTime = promotion.EndTime,
                Id = promotion.Id,
                Name = promotion.Name,
                Percent = promotion.Percent,
                Quantity = promotion.Quantity,
                StartTime = promotion.StartTime,
                Type = promotion.Type
            };
        }
    }
}
