namespace MovieManagement.Payloads.DataResponses.DataSchedule
{
    public class DataResponseSchedule : DataResponseBase
    {
        public double Price { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Code { get; set; }
        public string MovieName { get; set; }
        public string Name { get; set; }
        public string RoomName { get; set; }
    }
}
