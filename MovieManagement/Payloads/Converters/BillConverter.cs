using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Payloads.DataResponses.DataBill;

namespace MovieManagement.Payloads.Converters
{
    public class BillConverter
    {
        private readonly AppDbContext _context;
        private readonly BillFoodConverter _billFoodConverter;
        private readonly BillTicketConverter _billTicketConverter;
        public BillConverter()
        {
            _context = new AppDbContext();
            _billFoodConverter = new BillFoodConverter();
            _billTicketConverter = new BillTicketConverter();
        }
        public DataResponseBill EntityToDTO(Bill bill)
        {
            return new DataResponseBill
            {
                CreateTime = bill.CreateTime,
                CustomerName = _context.users.SingleOrDefault(x => x.Id == bill.CustomerId).Name,
                Id = bill.Id,
                Name = bill.Name,
                TotalMoney = bill.TotalMoney,
                BillStatusName = _context.billStatuses.SingleOrDefault(x => x.Id == bill.BillStatusId).Name,
                PromotionPercent = _context.promotions.SingleOrDefault(x => x.Id == bill.PromotionId)?.Percent,
                TradingCode = bill.TradingCode,
                BillFoods = _context.billFoods.Where(x => x.BillId == bill.Id)?.Select(x => _billFoodConverter.EntityToDTO(x)),
                BillTickets = _context.billTickets.Where(x => x.BillId == bill.Id).Select(x => _billTicketConverter.EntityToDTO(x))
            };
        }
    }
}
