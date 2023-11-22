namespace MovieManagement.Payloads.DataRequests.UserRequest
{
    public class Request_ConfirmCreateNewPassword
    {
        public string ConfirmCode { get; set; }
        public string NewPassword { get; set; }
    }
}
