using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataBill;

namespace MovieManagement.Payloads.Converters
{
    public class BillFoodConverter
    {
        private readonly AppDbContext _context;
        public BillFoodConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseBillFood EntityToDTO(BillFood billFood)
        {
            return new DataResponseBillFood
            {
                Id = billFood.Id,
                NameOfFood = _context.foods.SingleOrDefault(x => x.Id == billFood.FoodId).NameOfFood,
                Quantity = billFood.Quantity
            };
        }
    }
}
