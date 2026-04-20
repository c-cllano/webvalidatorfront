namespace DigitalSignature.API.Response
{
    public class ApiErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Code { get; set; }
        public int Status { get; set; }
    }
}
