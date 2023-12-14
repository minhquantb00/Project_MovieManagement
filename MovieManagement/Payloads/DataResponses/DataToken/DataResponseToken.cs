using MovieManagement.Payloads.DataResponses.DataUser;

namespace MovieManagement.Payloads.DataResponses.DataToken
{
    public class DataResponseToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DataResponseUser DataResponseUser { get; set; }
    }
}
