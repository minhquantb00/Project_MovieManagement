using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Payloads.DataRequests.FoodRequest
{
    public class Request_CreateFood
    {
        public double Price { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        public string NameOfFood { get; set; }
    }
}
