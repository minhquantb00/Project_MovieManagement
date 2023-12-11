namespace MovieManagement.Services.Interfaces
{
    public interface IVNPayService
    {
        Task<string> CreatePaymentUrl(int billId, HttpContext httpContext, int id);
        Task<string> VNPayReturn(IQueryCollection vnpayData);
    }
}
