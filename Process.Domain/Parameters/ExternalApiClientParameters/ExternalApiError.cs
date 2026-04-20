namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class ExternalApiError
    {
        public ExternalApiErrorData? Data { get; set; } = default!;
        public int? Code { get; set; }
        public string? CodeName { get; set; } = string.Empty;
    }

    public class ExternalApiErrorData
    {
        public string? Mesage { get; set; } = string.Empty;
        public object? Inner { get; set; } = default!;
        public object? Trace { get; set; } = default!;
    }
}
