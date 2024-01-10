using MovieManagement.Entities;

namespace MovieManagement.Payloads.DataResponses.DataBill
{
    public class DataResponseBill : DataResponseBase
    {
        public double? TotalMoney { get; set; }
        public string TradingCode { get; set; }
        public DateTime CreateTime { get; set; }
        public string CustomerName { get; set; }

        public string BillStatusName { get; set; }
        public string Name { get; set; }
        public int? PromotionPercent { get; set; }
        public IQueryable<DataResponseBillFood>? BillFoods { get; set; }
        public IQueryable<DataResponseBillTicket>? BillTickets { get; set; }
    }
}
