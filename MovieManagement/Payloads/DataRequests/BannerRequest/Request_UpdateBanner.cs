using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Payloads.DataRequests.BannerRequest
{
    public class Request_UpdateBanner
    {
        public int BannerId { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile ImageUrl { get; set; }
        public string Title { get; set; }
    }
}
