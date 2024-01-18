using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.PromotionRequest;
using MovieManagement.Payloads.DataResponses.DataPromotion;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class PromotionService : IPromotionService
    {
        private readonly ResponseObject<DataRepsonsePromotion> _responseObject;
        private readonly PromotionConverter _converter;
        public readonly AppDbContext _context;
        public PromotionService(ResponseObject<DataRepsonsePromotion> responseObject, PromotionConverter converter)
        {
            _responseObject = responseObject;
            _converter = converter;
            _context = new AppDbContext();
        }

        public async Task<ResponseObject<DataRepsonsePromotion>> CreatePromotion(Request_CreatePromotion request)
        {
            var rankCustomer = await _context.rankCustomers.SingleOrDefaultAsync(x => x.Id == request.RankCustomerId);
            if(rankCustomer == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy hạng", null);
            }
            var promotion = new Promotion
            {
                Description = request.Description,
                EndTime = request.EndTime,
                Name = request.Name,
                Percent = request.Percent,
                RankCustomerId = request.RankCustomerId,
                Quantity = request.Quantity,
                StartTime = request.StartTime,
                Type = request.Type
            };
            await _context.promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm khuyến mãi thành công", _converter.EntityToDTO(promotion));
        }

        public async Task<ResponseObject<DataRepsonsePromotion>> UpdatePromotion(Request_UpdatePromotion request)
        {
            var promotion = await _context.promotions.SingleOrDefaultAsync(x => x.Id == request.PromotionId);
            if(promotion == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy khuyến mãi", null);
            }
            promotion.Quantity = request.Quantity;
            promotion.StartTime = request.EndTime;
            promotion.Type = request.Type;
            promotion.EndTime = request.EndTime;
            promotion.Name = request.Name;
            promotion.Description = request.Description;
            promotion.Percent = request.Percent;
            promotion.RankCustomerId = request.RankCustomerId;
            _context.promotions.Update(promotion);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật thông tin khuyến mãi thành công", _converter.EntityToDTO(promotion));
        }

        public async Task<PageResult<DataRepsonsePromotion>> GetAllPromotions(int pageSize, int pageNumber)
        {
            var query = _context.promotions.Select(x => _converter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
    }
}
