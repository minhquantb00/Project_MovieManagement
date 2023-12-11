using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.RankCustomerRequest;
using MovieManagement.Payloads.DataResponses.DataRankCustomer;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class RankCustomerService : IRankCustomerService
    {
        private readonly ResponseObject<DataResponseRankCustomer> _responseObject;
        private readonly RankCustomerConverter _converter;
        public readonly AppDbContext _context;
        public RankCustomerService(ResponseObject<DataResponseRankCustomer> responseObject, RankCustomerConverter converter)
        {
            _responseObject = responseObject;
            _converter = converter;
            _context = new AppDbContext();
        }

        public async Task<ResponseObject<DataResponseRankCustomer>> CreateRankCustomer(Request_CreateRankCustomer request)
        {
            var rankCustomer = new RankCustomer();
            rankCustomer.Name = request.Name;   
            rankCustomer.Description = request.Description;
            rankCustomer.Point = request.Point;
            await _context.rankCustomers.AddAsync(rankCustomer);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm hạng thành công", _converter.EntityToDTO(rankCustomer));
        }

        public async Task<ResponseObject<DataResponseRankCustomer>> UpdateRankCustomer(Request_UpdateRankCustomer request)
        {
            var rank = await _context.rankCustomers.SingleOrDefaultAsync(x => x.Id == request.RankCustomerId);
            if(rank == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy rank", null);
            }
            rank.Name = request.Name;
            rank.Description = request.Description;
            rank.Point = request.Point;
            _context.rankCustomers.Update(rank);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật thông tin rank thành công", _converter.EntityToDTO(rank));
        }
    }
}
