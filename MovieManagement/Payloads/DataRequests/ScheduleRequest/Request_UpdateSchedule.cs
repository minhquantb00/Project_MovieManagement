namespace MovieManagement.Payloads.DataRequests.ScheduleRequest
{
    public class Request_UpdateSchedule
    {
        public int ScheduleId { get; set; }
        public double Price { get; set; }
        public int MovieId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
    }
}
