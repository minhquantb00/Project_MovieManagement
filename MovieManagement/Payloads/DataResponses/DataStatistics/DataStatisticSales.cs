namespace MovieManagement.Payloads.DataResponses.DataStatistics
{
    public class DataStatisticSales
    {
        public int MonthNumber { get; set; }
        public int? CinemaId { get; set; }
        public double? Sales { get; set; }
    }
}
