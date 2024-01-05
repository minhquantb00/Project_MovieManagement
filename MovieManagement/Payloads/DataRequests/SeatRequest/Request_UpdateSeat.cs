namespace MovieManagement.Payloads.DataRequests.SeatRequest
{
    public class Request_UpdateSeat
    {
        public int SeatId { get; set; }
        public int Number { get; set; }
        public string Line { get; set; }
        public int SeatTypeId { get; set; }
        public int SeatStatusId { get; set; }
    }
}
