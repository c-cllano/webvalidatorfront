namespace Process.Application.ValidationProcessExecutions.UpdateLastStepAndTrazability
{
    public class UpdateLastStepAndTrazabilityResponse
    {
        public long ValidationProcessExecutionId { get; set; }
        public long? ValidationProcessId { get; set; }
        public string? LastStep { get; set; }
        public string? Trazability { get; set; }
    }
}
