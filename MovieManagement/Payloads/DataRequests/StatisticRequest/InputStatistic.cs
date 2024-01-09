namespace MovieManagement.Payloads.DataRequests.StatisticRequest
{
    public class InputStatistic
    {
        public int? CinemaId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set;}
    }
}
