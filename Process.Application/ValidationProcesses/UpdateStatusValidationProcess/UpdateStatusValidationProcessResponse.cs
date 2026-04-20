namespace Process.Application.ValidationProcesses.UpdateStatusValidationProcess
{
    public class UpdateStatusValidationProcessResponse
    {
        public long ValidationProcessId { get; set; }
        public int StatusCode { get; set; }
        public long StatusValidationId { get; set; }
        public bool Approved { get; set; }
        public bool Active { get; set; }
    }
}
