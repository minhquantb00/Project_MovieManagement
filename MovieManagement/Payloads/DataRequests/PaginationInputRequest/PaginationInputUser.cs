﻿namespace MovieManagement.Payloads.DataRequests.PaginationInputRequest
{
    public class PaginationInputUser
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
