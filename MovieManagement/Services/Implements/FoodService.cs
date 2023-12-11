using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandleImage;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.FoodRequest;
using MovieManagement.Payloads.DataResponses.DataFood;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class FoodService : IFoodService
    {
        private readonly ResponseObject<DataResponseFood> _responseObject;
        private readonly FoodConverter _converter;
        public readonly AppDbContext _context;
        public FoodService(ResponseObject<DataResponseFood> responseObject, FoodConverter converter)
        {
            _responseObject = responseObject;
            _converter = converter;
            _context = new AppDbContext();
        }

        public async Task<ResponseObject<DataResponseFood>> CreateFood(Request_CreateFood request)
        {
            var food = new Food
            {
                Description = request.Description,
                NameOfFood = request.NameOfFood,
                Price = request.Price,
                Image = await HandleUploadImage.Upfile(request.Image)
            };
            await _context.foods.AddRangeAsync(food);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm thông tin đồ ăn thành công", _converter.EntityToDTO(food));
        }

        public async Task<PageResult<DataResponseFood>> GetAllFoods(int pageSize, int pageNumber)
        {
            var query = _context.foods.Select(x => _converter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponseFood>> UpdateFood(Request_UpdateFood request)
        {
            var food = await _context.foods.SingleOrDefaultAsync(x => x.Id == request.FoodId);
            if(food == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Thông tin đồ ăn không tồn tại", null);
            }
            food.Description = request.Description;
            food.Price = request.Price;
            food.NameOfFood = request.NameOfFood;
            food.Image = await HandleUploadImage.UpdateFile(food.Image, request.Image);
            _context.foods.Update(food);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật thông tin đồ ăn thành công", _converter.EntityToDTO(food));
        }
    }
}
