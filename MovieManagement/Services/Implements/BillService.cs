﻿using Microsoft.EntityFrameworkCore;
using MovieManagement.Entities;
using MovieManagement.Payloads.Converters;
using MovieManagement.Payloads.DataRequests.BillRequest;
using MovieManagement.Payloads.DataResponses.DataBill;
using MovieManagement.Payloads.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services.Implements
{
    public class BillService : BaseService, IBillService
    {
        private readonly ResponseObject<DataResponseBillFood> _responseBillFoodObject;
        private readonly ResponseObject<DataResponseBillTicket> _responseBillTicketObject;
        private readonly ResponseObject<DataResponseBill> _responseObject;
        private readonly BillConverter _billConverter;
        private readonly BillTicketConverter _billTicketConverter;
        private readonly BillFoodConverter _billFoodConverter;
        public BillService(ResponseObject<DataResponseBillFood> responseBillFoodObject, ResponseObject<DataResponseBillTicket> responseBillTicketObject, ResponseObject<DataResponseBill> responseObject, BillConverter billConverter, BillTicketConverter billTicketConverter, BillFoodConverter billFoodConverter)
        {
            _responseBillFoodObject = responseBillFoodObject;
            _responseBillTicketObject = responseBillTicketObject;
            _responseObject = responseObject;
            _billConverter = billConverter;
            _billTicketConverter = billTicketConverter;
            _billFoodConverter = billFoodConverter;
        }

        public Task<ResponseObject<DataResponseBill>> CreateBill(Request_CreateBill request)
        {
            throw new NotImplementedException();
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
