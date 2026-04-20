namespace Process.Domain.Parameters.ProcessDocuments
{
    public class ProcessDocumentsRequest
    {
        public string UrlFile { get; set; } = string.Empty;
        public string? ProcessName { get; set; }
        public int ServiceType { get; set; }
        public int? ServiceSubType { get; set; }
        public DateTime Date { get; set; }
    }
}
