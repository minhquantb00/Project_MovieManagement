﻿namespace MovieManagement.Payloads.DataRequests.RankCustomerRequest
{
    public class Request_UpdateRankCustomer
    {
        public int RankCustomerId { get; set; }
        public int Point { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
