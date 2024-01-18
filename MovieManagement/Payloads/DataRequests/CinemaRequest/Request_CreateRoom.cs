using MovieManagement.Payloads.DataRequests.SeatRequest;

namespace MovieManagement.Payloads.DataRequests.CinemaRequest
{
    public class Request_CreateRoom
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<Request_CreateSeat>? Request_CreateSeats { get; set; }
    }
}
