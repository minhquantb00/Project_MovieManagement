﻿using MovieManagement.Entities;
using MovieManagement.Payloads.DataRequests.BillRequest;
using MovieManagement.Payloads.DataResponses.DataBill;
using MovieManagement.Payloads.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface IBillService
    {
        Task<ResponseObject<DataResponseBillTicket>> CreateBillTicket(int billId, Request_CreateBillTicket request);
        Task<List<BillTicket>> CreateListBillTicket(int billId, List<Request_CreateBillTicket> requests);
        Task<ResponseObject<DataResponseBillFood>> CreateBillFood(int billId, Request_CreateBillFood request);
        Task<List<BillFood>> CreateListBillFood(int billId, List<Request_CreateBillFood> requests);
        Task<ResponseObject<DataResponseBill>> CreateBill(Request_CreateBill request);
    }
}
