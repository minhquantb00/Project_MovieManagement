﻿namespace MovieManagement.Payloads.DataRequests.StatisticRequest
{
    public class InputFoodStatistics
    {
        public int? FoodId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set;}
    }
}
