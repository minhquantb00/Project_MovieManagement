﻿namespace MovieManagement.Entities
{
    public class Promotion : BaseEntity
    {
        public int Percent { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public int RankCustomerId { get; set; }
        public RankCustomer? RankCustomer { get; set; }
        public IEnumerable<Bill>? Bills { get; set; }

    }
}
