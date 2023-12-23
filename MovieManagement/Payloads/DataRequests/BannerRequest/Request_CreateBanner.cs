namespace MovieManagement.Payloads.DataRequests.BannerRequest
{
    public class Request_CreateBanner
    {
        public IFormFile ImageUrl { get; set; }
        public string Title { get; set; }
    }
}
