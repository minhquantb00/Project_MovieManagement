using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandleImage;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.BannerRequest;
using MovieManagement.Payloads.DataResponses.DataBanner;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class BannerService : IBannerService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseBanner> _responseObject;
        private readonly BannerConverter _converter;
        public BannerService(ResponseObject<DataResponseBanner> responseObject, BannerConverter converter)
        {
            _context = new AppDbContext();
            _responseObject = responseObject;
            _converter = converter;
        }

        public async Task<ResponseObject<DataResponseBanner>> CreateBanner(Request_CreateBanner request)
        {
            Banner banner = new Banner
            {
                ImageUrl = await HandleUploadImage.Upfile(request.ImageUrl),
                Title = request.Title
            };
            await _context.banners.AddAsync(banner);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm banner thành công", _converter.EntityToDTO(banner));
        }

        public async Task<string> DeleteBanner(int bannerId)
        {
            var banner = await _context.banners.SingleOrDefaultAsync(x => x.Id == bannerId);
            if(banner == null)
            {
                return "Không tồn tại";
            }
            _context.banners.Remove(banner);
            await _context.SaveChangesAsync();
            return "Xóa banner thành công";
        }

        public async Task<PageResult<DataResponseBanner>> GetAllBanners(int pageSize, int pageNumber)
        {
            var query = _context.banners.Select(x => _converter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponseBanner>> GetBannerById(int bannerId)
        {
            var result = await _context.banners.SingleOrDefaultAsync(x => x.Id == bannerId);
            if(result == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy", null);
            }
            return _responseObject.ResponseSuccess("Lấy dữ liệu thành công", _converter.EntityToDTO(result));
        }

        public async Task<ResponseObject<DataResponseBanner>> UpdateBanner(Request_UpdateBanner request)
        {
            var banner = await _context.banners.SingleOrDefaultAsync(x => x.Id == request.BannerId);
            if(banner == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy", null);
            }
            banner.Title = request.Title;
            banner.ImageUrl = await HandleUploadImage.UpdateFile(banner.ImageUrl, request.ImageUrl);
            _context.banners.Update(banner);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật banner thành công", _converter.EntityToDTO(banner));
        }
    }
}
