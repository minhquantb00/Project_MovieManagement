namespace MovieManagement.Payloads.DataResponses.DataUser
{
    public class DataResponseUser : DataResponseBase
    {
        public int? Point { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
