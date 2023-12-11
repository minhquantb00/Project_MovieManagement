using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandleImage;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.MovieRequest;
using MovieManagement.Payloads.DataResponses.DataMovie;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class MovieService : IMovieService
    {
        private readonly ResponseObject<DataResponseMovie> _responseObject;
        private readonly MovieConverter _converter;
        public readonly AppDbContext _context;
        public MovieService(MovieConverter converter, ResponseObject<DataResponseMovie> responseObject)
        {
            _converter = converter;
            _responseObject = responseObject;
            _context = new AppDbContext();
        }
        public async Task<ResponseObject<DataResponseMovie>> CreateMovie(Request_CreateMovie request)
        {
            var movieType = await _context.movieTypes.SingleOrDefaultAsync(x => x.Id == request.MovieTypeId);
            if(movieType == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thể loại phim", null);
            }
            var movie = new Movie
            {
                Description = request.Description,
                Director = request.Director,
                EndTime = request.EndTime,
                Image = await HandleUploadImage.Upfile(request.Image),
                Language = request.Language,
                MovieDuration = request.MovieDuration,
                MovieTypeId = request.MovieTypeId,
                Name = request.Name,
                PremiereDate = request.PremiereDate,
                RateId = request.RateId,
                Trailer = request.Trailer
            };
            await _context.movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thêm thông tin phim thành công", _converter.EntityToDTO(movie));
        }

        public async Task<string> DeleteMovie(int movieId)
        {
            var movie = await _context.movies.SingleOrDefaultAsync(x => x.Id == movieId);
            if(movie == null)
            {
                return "Phim không tồn tại";
            }
            movie.IsActive = false;
            _context.movies.Update(movie);
            await _context.SaveChangesAsync();
            return "Xóa thông tin thành công";
        }

        public async Task<PageResult<DataResponseMovie>> GetAllMovie(int pageSize, int pageNumber)
        {
            var query = _context.movies.Select(x => _converter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;  
        }

        public async Task<ResponseObject<DataResponseMovie>> GetMovieById(int movieId)
        {
            var movie =  await _context.movies.SingleOrDefaultAsync(x => x.Id == movieId);
            return _responseObject.ResponseSuccess("Lấy thông tin thành công", _converter.EntityToDTO(movie));
        }

        public async Task<ResponseObject<DataResponseMovie>> UpdateMovie(Request_UpdateMovie request)
        {
            var movie = await _context.movies.SingleOrDefaultAsync(x => x.Id == request.MovieId);
            if(movie == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy phim", null);
            }
            if(!_context.movieTypes.Any(x => x.Id == request.MovieTypeId))
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thể loại", null);
            }
            movie.Director = request.Director;
            movie.MovieDuration = request.MovieDuration;
            movie.Description = request.Description;
            movie.EndTime = request.EndTime;
            movie.Image = await HandleUploadImage.UpdateFile(movie.Image, request.Image);
            movie.Language = request.Language;
            movie.MovieTypeId = request.MovieTypeId;
            movie.Name = request.Name;
            movie.RateId = request.RateId;
            movie.Trailer = request.Trailer;
            _context.movies.Update(movie);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật thông tin phim thành công", _converter.EntityToDTO(movie));
        }
    }
}
