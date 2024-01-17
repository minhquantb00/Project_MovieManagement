using MovieManagement.Entities;
using MovieManagement.Handle.HandleEmail;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.UserRequest;
using MovieManagement.Payloads.DataResponses.DataToken;
using MovieManagement.Payloads.DataResponses.DataUser;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using BcryptNet = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using MovieManagement.Payloads.DataRequests.TokenRequest;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.DataContext;
using MovieManagement.Payloads.DataRequests.StatisticRequest;

namespace MovieManagement.Services.Implements
{
    public class AuthService : IAuthService
    {
        public readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseUser> _responseObject;
        private readonly UserConverter _userConverter;
        private readonly ResponseObject<DataResponseToken> _responseTokenObject;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(ResponseObject<DataResponseUser> responseObject, UserConverter userConverter, ResponseObject<DataResponseToken> responseTokenObject, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _responseObject = responseObject;
            _userConverter = userConverter;
            _responseTokenObject = responseTokenObject;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _context = new AppDbContext();
        }
        #region Xử lý đăng ký và xác nhận đăng ký tài khoản
        public async Task<ResponseObject<DataResponseUser>> ConfirmCreateNewAccount(Request_ConfirmCreateAccount request)
        {
            ConfirmEmail confirmEmail = _context.confirmEmails.Where(x => x.ConfirmCode.Equals(request.ConfirmCode)).SingleOrDefault();
            if (confirmEmail == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Xác nhận đăng ký tài khoản thất bại", null);
            }
            if (confirmEmail.ExpiredDateTime < DateTime.Now)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Mã xác nhận đã hết hạn", null);
            }
            User user = await _context.users.FirstOrDefaultAsync(x => x.Id == confirmEmail.UserId);
            user.UserStatusId = 2;
            _context.confirmEmails.Remove(confirmEmail);
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Xác nhận đăng ký tài khoản thành công", _userConverter.EntityToDTO(user));
        }
        public async Task<ResponseObject<DataResponseUser>> Register(Request_Register request)
        {
            if(string.IsNullOrWhiteSpace(request.Username) 
                || string.IsNullOrWhiteSpace(request.Password) 
                || string.IsNullOrWhiteSpace(request.PhoneNumber) 
                || string.IsNullOrWhiteSpace(request.Email))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            if(!Validate.IsValidEmail(request.Email))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Định dạng email không hợp lệ", null);
            }
            if (!Validate.IsValidPhoneNumber(request.PhoneNumber))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Định dạng số điện thoại không hợp lệ", null);
            }
            if(_context.users.Any(x => x.Username.Equals(request.Username)))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên tài khoản đã tồn tại trên hệ thống", null);
            }
            if (_context.users.Any(x => x.Email.Equals(request.Email)))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Email đã tồn tại trên hệ thống", null);
            }
            if (_context.users.Any(x => x.PhoneNumber.Equals(request.PhoneNumber)))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Số điện thoại đã tồn tại trên hệ thống", null);
            }
            try
            {
                User user = new User();
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.Username = request.Username;
                user.Password = BcryptNet.HashPassword(request.Password);
                user.Name = request.Name;
                user.RoleId = 4;
                user.UserStatusId = 1;
                user.IsActive = false;
                await _context.users.AddAsync(user);
                await _context.SaveChangesAsync();
                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    UserId = user.Id,
                    IsConfirm = false,
                    ExpiredDateTime = DateTime.Now.AddHours(24),
                    ConfirmCode = "MyBugs" + "_" + GenerateCodeActive().ToString(),
                    RequiredDateTime = DateTime.Now
                };
                await _context.confirmEmails.AddAsync(confirmEmail);
                await _context.SaveChangesAsync();
                string message = SendEmail(new EmailTo
                {
                    Mail = request.Email,
                    Subject = "Nhận mã xác nhận để xác nhận đăng ký tài khoản mới từ đây: ",
                    Content = $"Mã kích hoạt của bạn là: {confirmEmail.ConfirmCode}, mã này có hiệu lực là 24 tiếng"
                });
                return _responseObject.ResponseSuccess("Đăng ký tài khoản thành công, nhận mã xác nhận gửi về email để đăng ký tài khoản", _userConverter.EntityToDTO(user));

            }catch(Exception ex)
            {
                return _responseObject.ResponseError(StatusCodes.Status500InternalServerError, ex.Message, null);
            }
        }
        #endregion
        #region Xử lý việc đăng nhập và GenerateToken
        public DataResponseToken GenerateAccessToken(User user)

        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value!);
            var role = _context.roles.SingleOrDefault(x => x.Id == user.RoleId);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Name", user.Name),
                    new Claim("Username", user.Username),
                    new Claim(ClaimTypes.Role, role?.Code ?? "")
                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            RefreshToken rf = new RefreshToken
            {
                Token = refreshToken,
                ExpiredTime = DateTime.Now.AddHours(10),
                UserId = user.Id
            };

            _context.refreshTokens.Add(rf);
            _context.SaveChanges();

            DataResponseToken data = new DataResponseToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                DataResponseUser = _userConverter.EntityToDTO(user)
            };
            return data;
        }
        public async Task<ResponseObject<DataResponseToken>> Login(Request_Login request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            var user = await _context.users.SingleOrDefaultAsync(x => x.Username.Equals(request.Username));
            if (user == null)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
            }
            if(user.UserStatusId == 1)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status401Unauthorized, "Tài khoản của bạn vẫn chưa được kích hoạt, vui lòng kích hoạt tài khoản", null);
            }
            bool checkPass = BcryptNet.Verify(request.Password, user.Password);
            if (!checkPass)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không chính xác", null);
            }
            if(user.IsActive == false)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản đã bị xóa, vui lòng thử lại", null);
            }
            else
            {
                return _responseTokenObject.ResponseSuccess("Đăng nhập tài khoản thành công", GenerateAccessToken(user));
            }
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public ResponseObject<DataResponseToken> RenewAccessToken(Request_RenewToken request)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration.GetSection("AppSettings:SecretKey").Value;

                var tokenValidation = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
                };

                var tokenAuthentication = jwtTokenHandler.ValidateToken(request.AccessToken, tokenValidation, out var validatedToken);
                if(!(validatedToken is JwtSecurityToken jwtSecurityToken)|| jwtSecurityToken.Header.Alg != SecurityAlgorithms.HmacSha256)
                {
                    return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Token không hợp lệ", null);
                }
                var refreshToken = _context.refreshTokens.SingleOrDefault(x => x.Token.Equals(request.RefreshToken));
                if(refreshToken == null)
                {
                    return _responseTokenObject.ResponseError(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database", null);
                }
                if(refreshToken.ExpiredTime < DateTime.Now)
                {
                    return _responseTokenObject.ResponseError(StatusCodes.Status401Unauthorized, "RefreshToken đã hết hạn", null);
                }
                var user = _context.users.SingleOrDefault(x => x.Id == refreshToken.UserId);
                if(user == null)
                {
                    return _responseTokenObject.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);
                }
                var newToken = GenerateAccessToken(user);
                return _responseTokenObject.ResponseSuccess("Token đã được làm mới thành công", newToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Lỗi xác thực token: " + ex.Message, null);
            }
            catch (Exception ex)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status500InternalServerError, "Lỗi không xác định: " + ex.Message, null);
            }
        }
        #endregion
        #region Xử lý vấn đề liên quan đến gửi email

        public string SendEmail(EmailTo emailTo)
        {
            if (!Validate.IsValidEmail(emailTo.Mail))
            {
                return "Định dạng email không hợp lệ";
            }
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("minhquantb00@gmail.com", "jvztzxbtyugsiaea"),
                EnableSsl = true
            };
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("minhquantb00@gmail.com");
                message.To.Add(emailTo.Mail);
                message.Subject = emailTo.Subject;
                message.Body = emailTo.Content;
                message.IsBodyHtml = true;
                smtpClient.Send(message);

                return "Xác nhận gửi email thành công, lấy mã để xác thực";
            }
            catch (Exception ex)
            {
                return "Lỗi khi gửi email: " + ex.Message;
            }
        }
        private int GenerateCodeActive()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }


        #endregion
        #region Xử lý việc đổi mật khẩu và quên mật khẩu
        public async Task<ResponseObject<DataResponseUser>> ChangePassword(int userId, Request_ChangePassword request)
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.Id == userId);

            bool checkOldPass = BcryptNet.Verify(request.OldPassword, user.Password);
            if(!checkOldPass)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không chính xác", null);
            }
            if(!request.NewPassword.Equals(request.ConfirmNewPassword))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không trùng khớp", null);
            }
            user.Password = BcryptNet.HashPassword(request.NewPassword);
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Đổi mật khẩu thành công", _userConverter.EntityToDTO(user));
        }
        public async Task<string> ForgotPassword(Request_ForgotPassword request)
        {
            User user = await _context.users.FirstOrDefaultAsync(x => x.Email.Equals(request.Email));
            if (user is null)
            {
                return "Email không tồn tại trong hệ thống";
            }
            else
            {
                var confirms = _context.confirmEmails.Where(x => x.UserId == user.Id).ToList();
                _context.confirmEmails.RemoveRange(confirms);
                await _context.SaveChangesAsync();
                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    UserId = user.Id,
                    IsConfirm = false,
                    ExpiredDateTime = DateTime.Now.AddHours(4),
                    ConfirmCode = "MyBugs" + "_" + GenerateCodeActive().ToString()
                };
                await _context.confirmEmails.AddAsync(confirmEmail);
                await _context.SaveChangesAsync();
                string message = SendEmail(new EmailTo
                {
                    Mail = request.Email,
                    Subject = "Nhận mã xác nhận để tạo mật khẩu mới từ đây: ",
                    Content = $"Mã kích hoạt của bạn là: {confirmEmail.ConfirmCode}, mã này sẽ hết hạn sau 4 tiếng"
                });
                return "Gửi mã xác nhận về email thành công, vui lòng kiểm tra email";
            }
        }
        public async Task<string> ConfirmCreateNewPassword(Request_ConfirmCreateNewPassword request)
        {
            ConfirmEmail confirm = await _context.confirmEmails.SingleOrDefaultAsync(x => x.ConfirmCode.Equals(request.ConfirmCode));
            if(confirm is null)
            {
                return "Mã xác nhận không chính xác";
            }
            if(confirm.ExpiredDateTime < DateTime.Now)
            {
                return "Mã xác nhận đã hết hạn";
            }
            User user = await _context.users.SingleOrDefaultAsync(x => x.Id == confirm.UserId);
            user.Password = BcryptNet.HashPassword(request.NewPassword);
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return "Tạo mật khẩu mới thành công";
        }
        #endregion
        #region Xử lý việc thay đổi quyền hạn của người dùng
        public async Task<ResponseObject<DataResponseUser>> ChangeDecentralization(Request_ChangeDecentralization request)
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.Id == request.UserId);
            if (user is null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy id người dùng", null);
            }
            var currentUser = _httpContextAccessor.HttpContext.User;

            if (!currentUser.Identity.IsAuthenticated)
            {
                return _responseObject.ResponseError(StatusCodes.Status401Unauthorized, "Người dùng không được xác thực hoặc không được xác định", null);
            }

            if (!currentUser.IsInRole("Admin"))
            {
                return _responseObject.ResponseError(StatusCodes.Status403Forbidden, "Người dùng không có quyền sử dụng chức năng này", null);
            }
            user.RoleId = request.RoleId;
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Thay đổi quyền người dùng thành công", _userConverter.EntityToDTO(user));
        }
        #endregion
        #region Cập nhật thông tin người dùng
        public async Task<ResponseObject<DataResponseUser>> UpdateUserInformation(int userId, Request_UpdateUserInformation request)
        {
            var user = await _context.users.SingleOrDefaultAsync(x => x.Id == userId);

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.Email))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);
            }
            if (!Validate.IsValidEmail(request.Email))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Định dạng email không hợp lệ", null);
            }
            if (!Validate.IsValidEmail(request.PhoneNumber))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Định dạng số điện thoại không hợp lệ", null);
            }
            user.Username = request.Username;
            user.Name = request.Name;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            _context.users.Update(user);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật thông tin thành công", _userConverter.EntityToDTO(user));
        }
        #endregion
        #region Xử lý việc lấy dữ liệu người dùng
        public async Task<PageResult<DataResponseUser>> GetAllUsers(InputUser input, int pageSize, int pageNumber)
        {
            var query = await _context.users.Include(x => x.RankCustomer).AsNoTracking().OrderBy(x => x.RankCustomer.Point).Where(x => x.IsActive == true).ToListAsync();
            if(!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(input.Name.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(input.Email))
            {
                query = query.Where(x => x.Email.ToLower().Contains(input.Email.ToLower())).ToList();
            }
            if (input.RoleId.HasValue)
            {
                query = query.Where(x => x.RoleId == input.RoleId).ToList();
            }
            var result = query.Select(x => _userConverter.EntityToDTO(x)).AsQueryable();
            var data = Pagination.GetPagedData(result, pageSize, pageNumber);
            return data;
        }
        public async Task<PageResult<DataResponseUser>> GetListUserByRank(int pageSize, int pageNumber)
        {
            var query = _context.users.Include(x => x.RankCustomer).AsNoTracking().OrderBy(x => x.RankCustomer.Point).Select(x => _userConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
        public async Task<PageResult<DataResponseUser>> GetUserByName(string name, int pageSize, int pageNumber)
        {
            var query = _context.users.Where(x => x.Name.Equals(name)).Select(x => _userConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
        #endregion

    }
}
