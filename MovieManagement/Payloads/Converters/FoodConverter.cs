using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataFood;

namespace MovieManagement.Payloads.Converters
{
    public class FoodConverter
    {
        public DataResponseFood EntityToDTO(Food food)
        {
            return new DataResponseFood
            {
                Description = food.Description,
                Id = food.Id,
                Image = food.Image,
                NameOfFood = food.NameOfFood,
                Price = food.Price
            };
        }
    }
}
