namespace Process.Domain.Entities
{
    public class Capture
    {
        public string? UrlFile { get; set; }
        public string? ProcessName { get; set; }
        public int ServiceType { get; set; }
        public string? ServiceSubType { get; set; }
        public DateTime Date { get; set; }
    }
}
