using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using MovieManagement.Services.Interfaces;
using MovieManagement.Services.Implements;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.Responses;
using MovieManagement.Payloads.DataResponses.DataUser;
using MovieManagement.Payloads.DataResponses.DataToken;
using Swashbuckle.AspNetCore.Filters;
using MovieManagement.Payloads.DataResponses.DataCinema;
using MovieManagement.Payloads.DataResponses.DataSeat;
using MovieManagement.Payloads.DataResponses.DataTicket;
using MovieManagement.Payloads.DataResponses.DataSchedule;
using MovieManagement.Payloads.DataResponses.DataPromotion;
using MovieManagement.Payloads.DataResponses.DataRankCustomer;
using MovieManagement.Payloads.DataResponses.DataMovie;
using MovieManagement.Payloads.DataResponses.DataFood;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using BcryptNet = BCrypt.Net.BCrypt;
using MovieManagement.Payloads.DataResponses.DataBill;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MovieManagement.Payloads.DataResponses.DataBanner;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Auth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Làm theo mẫu này. Example: Bearer {Token} ",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    x.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddControllers().AddJsonOptions(options =>

{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
//builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICinemaService, CinemaService>();
builder.Services.AddScoped<ISeatService , SeatService>();  
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IRankCustomerService, RankCustomerService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IVNPayService, VNPayService>();
builder.Services.AddScoped<IBillService, BillService>();

builder.Services.AddSingleton<ResponseObject<DataResponseUser>>();
builder.Services.AddSingleton<ResponseObject<DataResponseToken>>();
builder.Services.AddSingleton<ResponseObject<DataResponseMovieType>>();
builder.Services.AddSingleton<ResponseObject<DataResponseCinema>>();
builder.Services.AddSingleton<ResponseObject<DataResponseMovie>>();
builder.Services.AddSingleton<ResponseObject<DataResponseRoom>>();
builder.Services.AddSingleton<ResponseObject<DataResponseFood>>();
builder.Services.AddSingleton<ResponseObject<DataResponseBanner>>();
builder.Services.AddSingleton<ResponseObject<DataRepsonsePromotion>>();
builder.Services.AddSingleton<ResponseObject<DataResponseRankCustomer>>();
builder.Services.AddSingleton<ResponseObject<DataResponseMovie>>();
builder.Services.AddSingleton<ResponseObject<DataResponseCinema>>();
builder.Services.AddSingleton<ResponseObject<DataResponseSeat>>();
builder.Services.AddSingleton<ResponseObject<DataResponseTicket>>();
builder.Services.AddSingleton<ResponseObject<DataResponseSchedule>>();
builder.Services.AddSingleton<ResponseObject<DataResponseBill>>();
builder.Services.AddSingleton<ResponseObject<DataResponseBillFood>>();
builder.Services.AddSingleton<ResponseObject<DataResponseBillTicket>>();

//builder.Services.AddScoped<UserConverter>();
//builder.Services.AddScoped<TicketConverter>();
//builder.Services.AddScoped<SeatConverter>();
//builder.Services.AddScoped<SchedulesConverter>();
//builder.Services.AddScoped<RoomConverter>();
//builder.Services.AddScoped<RankCustomerConverter>();
//builder.Services.AddScoped<PromotionConverter>();
//builder.Services.AddScoped<MovieConverter>();
//builder.Services.AddScoped<CinemaConverter>();
//builder.Services.AddScoped<FoodConverter>();
//builder.Services.AddScoped<BillConverter>();
//builder.Services.AddScoped<BillTicketConverter>();
//builder.Services.AddScoped<BillFoodConverter>();
builder.Services.AddScoped<AuthService>();


builder.Services.AddSingleton<UserConverter>();
builder.Services.AddSingleton<TicketConverter>();
builder.Services.AddSingleton<SeatConverter>();
builder.Services.AddSingleton<SchedulesConverter>();
builder.Services.AddSingleton<RoomConverter>();
builder.Services.AddSingleton<RankCustomerConverter>();
builder.Services.AddSingleton<BannerConverter>();
builder.Services.AddSingleton<PromotionConverter>();
builder.Services.AddSingleton<MovieTypeConverter>();
builder.Services.AddSingleton<MovieConverter>();
builder.Services.AddSingleton<CinemaConverter>();
builder.Services.AddSingleton<FoodConverter>();
builder.Services.AddSingleton<BillConverter>();
builder.Services.AddSingleton<BillTicketConverter>();
builder.Services.AddSingleton<BillFoodConverter>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:SecretKey").Value!))
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();
