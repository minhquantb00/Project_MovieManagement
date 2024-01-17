namespace MovieManagement.Payloads.DataRequests.CinemaRequest
{
    public class Request_CreateCinema
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string NameOfCinema { get; set; }
        public List<Request_CreateRoom>? Request_CreateRooms { get; set; }
    }
}
