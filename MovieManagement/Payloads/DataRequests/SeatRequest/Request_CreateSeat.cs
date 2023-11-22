namespace MovieManagement.Payloads.DataRequests.SeatRequest
{
    public class Request_CreateSeat
    {
        public int Number { get; set; }
        public int Line { get; set; }
        public int SeatTypeId { get; set; }
    }
}
