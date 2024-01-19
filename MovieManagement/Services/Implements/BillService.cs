using FluentEmail.Core;
using Microsoft.EntityFrameworkCore;
using MovieManagement.DataContext;
using MovieManagement.Entities;
using MovieManagement.Handle.HandleGenerate;
using MovieManagement.Handle.HandlePagination;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.BillRequest;
using MovieManagement.Payloads.DataRequests.StatisticRequest;
using MovieManagement.Payloads.DataResponses.DataBill;
using MovieManagement.Payloads.DataResponses.DataStatistics;
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

        //public async Task<ResponseObject<DataResponseBill>> CreateBill(Request_CreateBill request)
        //{
        //    var customer = await _context.users.SingleOrDefaultAsync(x => x.Id == request.CustomerId);
        //    if(customer == null)
        //    {
        //        return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thông tin khách hàng", null);
        //    }
        //    var promotion = await _context.promotions.SingleOrDefaultAsync(x => x.Id == request.PromotionId);
        //    var bill = new Bill();
        //    bill.CustomerId = request.CustomerId;
        //    bill.TradingCode = GenerateCode.GenerateBillCode();
        //    bill.CreateAt = DateTime.Now;
        //    bill.CreateTime = DateTime.Now;
        //    bill.Name = request.BillName;
        //    bill.BillStatusId = 1;
        //    bill.PromotionId = request.PromotionId == null ? null : request.PromotionId;
        //    bill.BillTickets = null;
        //    bill.BillFoods = null;
        //    bill.IsActive = true;
        //    bill.TotalMoney = 0;
        //    await _context.bills.AddAsync(bill);
        //    await _context.SaveChangesAsync();
        //    bill.BillTickets = await CreateListBillTicket(bill.Id, request.BillTickets);
        //    bill.BillFoods = request.BillFoods != null ? await CreateListBillFood(bill.Id, request.BillFoods) : null;
        //    double priceTicket = 0;
        //    double priceFood = 0;
        //    bill.BillTickets.ForEach(x =>
        //    {
        //        var ticket = _context.tickets.SingleOrDefault(y => y.Id == x.TicketId);
        //        priceTicket += ticket.PriceTicket * x.Quantity;
        //    });
        //    bill.BillFoods?.ForEach(x =>
        //    {
        //        var food = _context.foods.SingleOrDefault(y => y.Id == x.FoodId);
        //        priceFood += food.Price * x.Quantity;
        //    });
        //    if(request.BillFoods == null && request.PromotionId != null)
        //    {
        //        bill.TotalMoney = priceTicket -  ((priceTicket * promotion.Percent * 1.0)/100);
        //    }
        //    else if(request.BillFoods == null && request.PromotionId == null)
        //    {
        //        bill.TotalMoney = priceTicket;
        //    }
        //    bill.TotalMoney = (priceTicket + priceFood) - (((priceTicket + priceFood) * promotion?.Percent) * 1.0 / 100);
        //    _context.bills.Update(bill);
        //    await _context.SaveChangesAsync();
        //    return _responseObject.ResponseSuccess("Tạo hóa đơn thành công", _billConverter.EntityToDTO(bill));

        //}
        public async Task<ResponseObject<DataResponseBill>> CreateBill(Request_CreateBill request)
        {
            var customer = await _context.users.SingleOrDefaultAsync(x => x.Id == request.CustomerId);
            if (customer == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status404NotFound, "Không tìm thấy thông tin khách hàng", null);
            }

            var promotion = await _context.promotions.SingleOrDefaultAsync(x => x.Id == request.PromotionId);

            var bill = new Bill
            {
                CustomerId = request.CustomerId,
                TradingCode = GenerateCode.GenerateBillCode(),
                CreateAt = DateTime.Now,
                CreateTime = DateTime.Now,
                Name = request.BillName,
                BillStatusId = 1,
                PromotionId = request.PromotionId,
                BillTickets = null,
                BillFoods = null,
                IsActive = true,
                TotalMoney = 0
            };

            await _context.bills.AddAsync(bill);
            await _context.SaveChangesAsync();

            bill.BillTickets = await CreateListBillTicket(bill.Id, request.BillTickets);
            bill.BillFoods = request.BillFoods != null ? await CreateListBillFood(bill.Id, request.BillFoods) : null;

            double priceTicket = bill.BillTickets?.Sum(x => _context.tickets.SingleOrDefault(y => y.Id == x.TicketId).PriceTicket * x.Quantity) ?? 0;
            double priceFood = bill.BillFoods?.Sum(x => _context.foods.SingleOrDefault(y => y.Id == x.FoodId).Price * x.Quantity) ?? 0;
            double total = priceTicket + priceFood;

            if (promotion != null)
            {
                bill.TotalMoney = total - (total * promotion.Percent / 100.0);
            }
            else
            {
                bill.TotalMoney = total;
            }

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
            var bill = await _context.bills.Include(x => x.Promotion).AsNoTracking().SingleOrDefaultAsync(x => x.Id == billId);
            if (bill == null)
            {
                return _responseBillTicketObject.ResponseError(StatusCodes.Status404NotFound, "Hóa đơn không tồn tại", null);
            }
            var billTicket = new BillTicket
            {
                BillId = billId,
                Quantity = 1,
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
                    Quantity = 1,
                    TicketId = request.TicketId,
                };
                list.Add(billTicket);
            }
            await _context.billTickets.AddRangeAsync(list);
            await _context.SaveChangesAsync();
            return list;
        }

        public async Task<ResponseObject<DataResponseBill>> GetPaymentHistoryByBillId(int billId)
        {
            var result = await _context.bills.Include(x => x.BillFoods).Include(x => x.BillTickets).AsNoTracking().SingleOrDefaultAsync(x => x.Id == billId);
            if(result.BillStatusId == 1)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Hóa đơn vẫn chưa được thanh toán", null);
            }
            return _responseObject.ResponseSuccess("Thông tin thanh toán của hóa đơn", _billConverter.EntityToDTO(result));
        }

        public async Task<PageResult<DataResponseBill>> GetAllBills(int pageSize, int pageNumber)
        {
            var query = _context.bills.Include(x => x.BillTickets).Include(x => x.BillFoods).Where(x => x.BillStatusId == 2).Select(x => _billConverter.EntityToDTO(x));
            var result = Pagination.GetPagedData(query, pageSize, pageNumber);
            return result;
        }
        public async Task<IQueryable<DataStatisticSales>> SalesStatistics(InputStatistic input)
        {
            var query = _context.bills.Include(x => x.BillFoods).ThenInclude(x => x.Food)
                                      .Include(x => x.BillTickets).ThenInclude(x => x.Ticket).ThenInclude(x => x.Schedule).ThenInclude(x => x.Room).ThenInclude(x => x.Cinema)
                                      .AsNoTracking()
                                      .Where(x => x.BillStatusId == 2);
            if (input.CinemaId.HasValue)
            {
                query = query.Where(x => x.BillTickets.Any(y => y.Ticket.Schedule.Room.CinemaId == input.CinemaId));
            }
            if (input.StartAt.HasValue)
            {
                query = query.Where(x => x.CreateAt.Date >= input.StartAt.Value.Date);
            }
            if (input.EndAt.HasValue)
            {
                query = query.Where(x => x.CreateAt.Date <= input.EndAt.Value.Date);
            }

            var billStats = await query
                .GroupBy(x => new
                {
                    Month = x.CreateAt.Month,
                    CinemaId = input.CinemaId.HasValue ? x.BillTickets.FirstOrDefault().Ticket.Schedule.Room.CinemaId : (int?)null
                })
                .Select(group => new DataStatisticSales
                {
                    MonthNumber = group.Key.Month,
                    CinemaId = group.Key.CinemaId,
                    Sales = group.Sum(item => item.TotalMoney),
                })
                .ToListAsync();

            return billStats.AsQueryable();
        }
        //public async Task<IQueryable<DataStatisticsFood>> SalesStatisticsFood(InputFoodStatistics input)
        //{
        //    var query = _context.bills.Include(x => x.BillFoods).ThenInclude(x => x.Food)
        //                              .AsNoTracking()
        //                              .Where(x => x.BillStatusId == 2);

        //    if (input.FoodId.HasValue)
        //    {
        //        query = query.Where(x => x.BillFoods.Any(y => y.FoodId == input.FoodId));
        //    }

        //    if (input.StartAt.HasValue)
        //    {
        //        query = query.Where(x => x.CreateAt.Date >= input.StartAt.Value.Date);
        //    }

        //    if (input.EndAt.HasValue)
        //    {
        //        query = query.Where(x => x.CreateAt.Date <= input.EndAt.Value.Date);
        //    }

        //    var billFoodStats = await query
        //                               .SelectMany(x => x.BillFoods)
        //                               .Where(bf => !input.FoodId.HasValue || bf.FoodId == input.FoodId)
        //                               .GroupBy(bf => bf.FoodId)
        //                               .Select(group => new DataStatisticsFood
        //                               {
        //                                   FoodId = group.Key,
        //                                   Sales = group.Sum(x => x.Quantity * (x.Food.Price)),
        //                                   SellNumber = group.Count()
        //                               }).ToListAsync();

        //    return billFoodStats.AsQueryable();
        //}
        public async Task<IQueryable<DataStatisticsFood>> SalesStatisticsFood(InputFoodStatistics input)
        {
            var query = _context.bills.Include(x => x.BillFoods).ThenInclude(x => x.Food)
                                      .AsNoTracking()
                                      .Where(x => x.BillStatusId == 2);

            if (input.FoodId.HasValue)
            {
                query = query.Where(x => x.BillFoods.Any(y => y.FoodId == input.FoodId));
            }

            if (input.StartAt.HasValue)
            {
                query = query.Where(x => x.CreateAt.Date >= input.StartAt.Value.Date);
            }

            if (input.EndAt.HasValue)
            {
                query = query.Where(x => x.CreateAt.Date <= input.EndAt.Value.Date);
            }

            var billFoodStats = await query
                .SelectMany(x => x.BillFoods)
                .Where(bf => !input.FoodId.HasValue || bf.FoodId == input.FoodId)
                .GroupBy(bf => new
                {
                    Month = bf.Bill.CreateAt.Month,
                    FoodId = bf.FoodId
                })
                .Select(group => new DataStatisticsFood
                {
                    MonthNumber = group.Key.Month,
                    FoodId = group.Key.FoodId,
                    Sales = group.Sum(x => x.Quantity * x.Food.Price),
                    SellNumber = group.Count()
                }).ToListAsync();

            return billFoodStats.AsQueryable();
        }

    }
}
