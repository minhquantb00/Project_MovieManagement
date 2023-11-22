namespace MovieManagement.Payloads.DataRequests.FoodRequest
{
    public class Request_UpdateFood
    {
        public int FoodId { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string NameOfFood { get; set; }
    }
}
