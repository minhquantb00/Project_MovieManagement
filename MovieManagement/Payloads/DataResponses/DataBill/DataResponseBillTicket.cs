namespace MovieManagement.Payloads.DataResponses.DataBill
{
    public class DataResponseBillTicket : DataResponseBase
    {
        public int Quantity { get; set; }
        public int SeatNumber { get; set; }
        public string SeatLine { get; set; }
    }
}
