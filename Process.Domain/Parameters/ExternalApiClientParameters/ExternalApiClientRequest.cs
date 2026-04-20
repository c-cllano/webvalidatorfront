using static Domain.Enums.Enumerations;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class ExternalApiClientRequest
    {
        public ApiName Section { get; set; }
        public ApiName? BaseUrl { get; set; }
        public ApiName ApiName { get; set; }
        public ApiName? Port { get; set; }
        public object? Body { get; set; }
        public string? Token { get; set; }
        public object? BodyExternalRequest { get; set; }
        public Dictionary<string, string>? QueryParams { get; set; }
        public string? AuthorizationValue { get; set; }
        public Dictionary<string, byte[]>? Files { get; set; }
        public Dictionary<string, string>? CustomHeaders { get; set; }
        public bool? IsFormUrlEncoded { get; set; }
        public string? Url { get; set; }
        public string? BaseUrlReconoser { get; set; }
    }
}
