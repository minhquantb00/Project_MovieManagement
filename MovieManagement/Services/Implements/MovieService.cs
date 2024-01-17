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
        private readonly ResponseObject<DataResponseMovieType> _responseObjectMovieType;
        private readonly MovieTypeConverter _movieTypeConverter;
        private readonly MovieConverter _converter;
        public readonly AppDbContext _context;
        public MovieService(MovieConverter converter, ResponseObject<DataResponseMovie> responseObject, ResponseObject<DataResponseMovieType> responseObjectMovieType, MovieTypeConverter movieTypeConverter)
        {
            _converter = converter;
            _responseObject = responseObject;
            _context = new AppDbContext();
            _responseObjectMovieType = responseObjectMovieType;
            _movieTypeConverter = movieTypeConverter;
        }
        public async Task<ResponseObject<DataResponseMovie>> CreateMovie(Request_CreateMovie request)
        {
            var movieType = await _context.movieTypes.FindAsync(request.MovieTypeId);
            if(movieType == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thể loại phim", null);
            }
            var uploadTasks = new Task<string>[]
            {
                HandleUploadImage.Upfile(request.Image),
                HandleUploadImage.Upfile(request.HeroImage)
            };
            var uploadResult = await Task.WhenAll(uploadTasks); 
            var movie = new Movie
            {
                Description = request.Description,
                Director = request.Director,
                EndTime = request.EndTime,
                Image = uploadResult[0],
                HeroImage = uploadResult[1],
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

        public async Task<PageResult<DataResponseMovie>> GetAllMovie(InputFilter input, int pageSize, int pageNumber)
        {
            var query = _context.movies.Include(x => x.MovieType).AsNoTracking().Where(x => x.IsActive == true).ToList();
            if (input.PremiereDate.HasValue)
            {
                query = query.Where(x => x.PremiereDate == input.PremiereDate).ToList();
            }
            if (input.MovieTypeId.HasValue)
            {
                query = query.Where(x => x.MovieTypeId == input.MovieTypeId).ToList();
            }
            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(x => x.Name.Trim().ToLower().Contains(input.Name.Trim().ToLower())).ToList();
            }
            var queryResult = query.Select(x => _converter.EntityToDTO(x)).AsQueryable();
            var result = Pagination.GetPagedData(queryResult, pageSize, pageNumber);
            return result;  
        }

        public async Task<ResponseObject<DataResponseMovie>> GetMovieById(int movieId)
        {
            var movie =  await _context.movies.SingleOrDefaultAsync(x => x.Id == movieId && x.IsActive == true);
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

        public async Task<ResponseObject<DataResponseMovieType>> CreateMovieType(Request_CreateMovieType request)
        {
            if (string.IsNullOrWhiteSpace(request.MovieTypeName))
            {
                return _responseObjectMovieType.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            MovieType movieType = new MovieType
            {
                MovieTypeName = request.MovieTypeName,
                IsActive = true
            };
            await _context.movieTypes.AddAsync(movieType);
            await _context.SaveChangesAsync();
            return _responseObjectMovieType.ResponseSuccess("Tạo thể loại phim thành công", _movieTypeConverter.EntityToDTO(movieType));
        }

        public async Task<ResponseObject<DataResponseMovieType>> UpdateMovieType(Request_UpdateMovieType request)
        {
            var movieType = await _context.movieTypes.SingleOrDefaultAsync(x => x.Id == request.MovieTypeId);
            if (movieType == null)
            {
                return _responseObjectMovieType.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thể loại phim", null);
            }
            movieType.MovieTypeName = request.MovieTypeName;
            _context.movieTypes.Update(movieType);
            await _context.SaveChangesAsync();
            return _responseObjectMovieType.ResponseSuccess("Cập nhật thông tin thể loại phim thành công", _movieTypeConverter.EntityToDTO(movieType));
        }
        
        public async Task<string> DeleteMovieType(int movieTypeId)
        {
            var movieType = await _context.movieTypes.SingleOrDefaultAsync(x => x.Id == movieTypeId);
            if(movieType == null)
            {
                return "Không tìm thấy thể loại phim";
            }
            movieType.IsActive = false;
            _context.movieTypes.Update(movieType);
            await _context.SaveChangesAsync();
            return "Xóa thể loại phim thành công";
        }

        public async Task<PageResult<DataResponseMovieType>> GetAllMovieTypes(int pageSize, int pageNumber)
        {
            var query =  _context.movieTypes.Include(x => x.Movies).AsNoTracking().Where(x => x.IsActive == true).Select(x => _movieTypeConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }

        public async Task<ResponseObject<DataResponseMovieType>> GetMovieTypeById(int movieTypeId)
        {
            var movieType = await _context.movieTypes.SingleOrDefaultAsync(x => x.Id == movieTypeId);
            if(movieType == null)
            {
                return _responseObjectMovieType.ResponseError(StatusCodes.Status404NotFound, "Thể loại phim không tồn tại", null);
            }
            return _responseObjectMovieType.ResponseSuccess("Lấy dữ liệu thành công", _movieTypeConverter.EntityToDTO(movieType));
        }
    }
}
