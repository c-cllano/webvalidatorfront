namespace Process.Application.ValidationProcesses.CompareFaces
{
    public class CompareFacesResponse
    {
        public bool IsValid { get; set; }
        public string Result { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public int Provider { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
