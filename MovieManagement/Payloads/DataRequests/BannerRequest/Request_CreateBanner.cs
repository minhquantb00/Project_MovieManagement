using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Payloads.DataRequests.BannerRequest
{
    public class Request_CreateBanner
    {
        [DataType(DataType.Upload)]
        public IFormFile ImageUrl { get; set; }
        public string Title { get; set; }
    }
}
