namespace MovieManagement.Payloads.DataRequests.TicketRequest
{
    public class Request_UpdateTicket
    {
        public int TicketId { get; set; }
        public int ScheduleId { get; set; }
        public int SeatId { get; set; }
    }
}
