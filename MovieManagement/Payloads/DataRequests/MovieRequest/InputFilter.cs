namespace MovieManagement.Payloads.DataRequests.MovieRequest
{
    public class InputFilter
    {
        public int? MovieTypeId { get; set; }
        public DateTime? PremiereDate { get; set; }
        public string? Name { get; set; }
    }
}
