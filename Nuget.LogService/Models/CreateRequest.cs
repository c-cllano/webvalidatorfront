namespace Nuget.LogService.Models
{
    public class CreateRequest
    {
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? Type { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public int? Status { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime? FinalTime { get; set; }
        public string? Component { get; set; }
        public string? TransactionID { get; set; }
        public string? Machine { get; set; }
        public string? UserID { get; set; }
        public DateTime Date { get; set; }
    }
}
