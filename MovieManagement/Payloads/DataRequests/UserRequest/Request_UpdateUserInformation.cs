﻿namespace MovieManagement.Payloads.DataRequests.UserRequest
{
    public class Request_UpdateUserInformation
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
