using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandleGenerate;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.BillRequest;
using MovieManagement.Payloads.DataResponses.DataBill;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class BillService : IBillService
    {
        private readonly ResponseObject<DataResponseBillFood> _responseBillFoodObject;
        private readonly ResponseObject<DataResponseBillTicket> _responseBillTicketObject;
        private readonly ResponseObject<DataResponseBill> _responseObject;
        private readonly BillConverter _billConverter;
        private readonly BillTicketConverter _billTicketConverter;
        private readonly BillFoodConverter _billFoodConverter;
        public readonly AppDbContext _context;
        public BillService(ResponseObject<DataResponseBillFood> responseBillFoodObject, ResponseObject<DataResponseBillTicket> responseBillTicketObject, ResponseObject<DataResponseBill> responseObject, BillConverter billConverter, BillTicketConverter billTicketConverter, BillFoodConverter billFoodConverter)
        {
            _responseBillFoodObject = responseBillFoodObject;
            _responseBillTicketObject = responseBillTicketObject;
            _responseObject = responseObject;
            _billConverter = billConverter;
            _billTicketConverter = billTicketConverter;
            _billFoodConverter = billFoodConverter;
            _context = new AppDbContext();
        }

        public async Task<ResponseObject<DataResponseBill>> CreateBill(Request_CreateBill request)
        {
            var customer = await _context.users.SingleOrDefaultAsync(x => x.Id == request.CustomerId);
            if(customer == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thông tin khách hàng", null);
            }
            var promotion = await _context.promotions.SingleOrDefaultAsync(x => x.Id == request.PromotionId);
            var bill = new Bill();
            bill.CustomerId = request.CustomerId;
            bill.TradingCode = GenerateCode.GenerateBillCode();
            bill.CreateAt = DateTime.Now;
            bill.CreateTime = DateTime.Now;
            bill.Name = request.BillName;
            bill.BillStatusId = 1;
            bill.PromotionId = promotion == null ? 0 : request.PromotionId;
            bill.BillTickets = null;
            bill.BillFoods = null;
            bill.IsActive = true;
            bill.TotalMoney = 0;
            await _context.bills.AddAsync(bill);
            await _context.SaveChangesAsync();
            bill.BillTickets = await CreateListBillTicket(bill.Id, request.BillTickets);
            bill.BillFoods = await CreateListBillFood(bill.Id,request.BillFoods);
            double priceTicket = 0;
            double priceFood = 0;
            bill.BillTickets.ForEach(x =>
            {
                var ticket = _context.tickets.SingleOrDefault(y => y.Id == x.TicketId);
                priceTicket += ticket.PriceTicket * x.Quantity;
            });
            bill.BillFoods.ForEach(x =>
            {
                var food = _context.foods.SingleOrDefault(y => y.Id == x.FoodId);
                priceFood += food.Price * x.Quantity;
            });
            bill.TotalMoney = (priceTicket + priceFood) - ((priceTicket + priceFood) * promotion.Percent) * 1.0/100;
            _context.bills.Update(bill);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Tạo hóa đơn thành công", _billConverter.EntityToDTO(bill));

        }

        public async Task<ResponseObject<DataResponseBillFood>> CreateBillFood(int billId, Request_CreateBillFood request)
        {
            var bill = await _context.bills.SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return _responseBillFoodObject.ResponseError(StatusCodes.Status404NotFound, "Hóa đơn không tồn tại", null);
            }
            var billFood = new BillFood
            {
                BillId = billId,
                Quantity = request.Quantity,
                FoodId = request.FoodId,
            };
            await _context.billFoods.AddAsync(billFood);
            await _context.SaveChangesAsync();
            return _responseBillFoodObject.ResponseSuccess("Thêm bill food thành công", _billFoodConverter.EntityToDTO(billFood));
        }

        public async Task<ResponseObject<DataResponseBillTicket>> CreateBillTicket(int billId, Request_CreateBillTicket request)
        {
            var bill = await _context.bills.SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return _responseBillTicketObject.ResponseError(StatusCodes.Status404NotFound, "Hóa đơn không tồn tại", null);
            }
            var billTicket = new BillTicket
            {
                BillId = billId,
                Quantity = request.Quantity,
                TicketId = request.TicketId,
            };
            await _context.billTickets.AddAsync(billTicket);
            await _context.SaveChangesAsync();
            return _responseBillTicketObject.ResponseSuccess("Thêm bill ticket thành công", _billTicketConverter.EntityToDTO(billTicket));
        }

        public async Task<List<BillFood>> CreateListBillFood(int billId, List<Request_CreateBillFood> requests)
        {
            var bill = await _context.bills.SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return null;
            }
            List<BillFood> list = new List<BillFood>();
            foreach (Request_CreateBillFood request in requests)
            {
                BillFood billTicket = new BillFood
                {
                    BillId = billId,
                    Quantity = request.Quantity,
                    FoodId = request.FoodId,
                };
                list.Add(billTicket);
            }
            await _context.billFoods.AddRangeAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task<List<BillTicket>> CreateListBillTicket(int billId, List<Request_CreateBillTicket> requests)
        {
            var bill = await _context.bills.SingleOrDefaultAsync(x => x.Id == billId);
            if(bill == null)
            {
                return null;
            }
            List<BillTicket> list = new List<BillTicket>();
            foreach(Request_CreateBillTicket request in requests)
            {
                BillTicket billTicket = new BillTicket
                {
                    BillId = billId,
                    Quantity = request.Quantity,
                    TicketId = request.TicketId,
                };
                list.Add(billTicket);
            }
            await _context.billTickets.AddRangeAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }
    }
}
