namespace DrawFlowProcess.Application.DrawFlowProcess.GteJsonFilter
{
    public class GetJsonFilterResponse
    {
        public string? Error { get; set; }
        public string? Token { get; set; }
        public Guid Id { get; set; }
        public bool IsValid { get; set; }
    }
}
