using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataBanner;

namespace MovieManagement.Payloads.Converters
{
    public class BannerConverter
    {
        public DataResponseBanner EntityToDTO(Banner banner)
        {
            return new DataResponseBanner()
            {
                Id = banner.Id,
                ImageUrl = banner.ImageUrl,
                Title = banner.Title,
            };
        }
    }
}
