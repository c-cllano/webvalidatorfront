namespace Process.Domain.Entities
{
    public class ValidationProcessAuditLogs
    {
        public long ValidationProcessAuditLogsId { get; set; }
        public long? ValidationProcessId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public int StatusCode { get; set; }
        public decimal DurationMs { get; set; }
        public string? Exception { get; set; }
    }
}
