namespace MovieManagement.Payloads.DataRequests.TicketRequest
{
    public class Request_CreateTicket
    {
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
    }
}
