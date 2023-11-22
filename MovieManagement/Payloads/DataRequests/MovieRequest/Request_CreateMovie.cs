﻿using MovieManagement.Payloads.DataRequests.ScheduleRequest;
using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Payloads.DataRequests.MovieRequest
{
    public class Request_CreateMovie
    {
        public int MovieDuration { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime PremiereDate { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
        public string Language { get; set; }
        public int MovieTypeId { get; set; }
        public string Name { get; set; }
        public int RateId { get; set; }
        public string Trailer { get; set; }
    }
}
