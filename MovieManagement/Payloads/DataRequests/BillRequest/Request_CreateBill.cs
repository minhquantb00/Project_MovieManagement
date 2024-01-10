using MovieManagement.Entities;

namespace MovieManagement.Payloads.DataRequests.BillRequest
{
    public class Request_CreateBill
    {
        public int CustomerId { get; set; }
        public string BillName { get; set; }
        public int? PromotionId { get; set; }
        public List<Request_CreateBillFood>? BillFoods { get; set; }
        public List<Request_CreateBillTicket> BillTickets { get; set; }
    }
}
