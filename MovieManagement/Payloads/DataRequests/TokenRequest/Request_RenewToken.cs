namespace MovieManagement.Payloads.DataRequests.TokenRequest
{
    public class Request_RenewToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
