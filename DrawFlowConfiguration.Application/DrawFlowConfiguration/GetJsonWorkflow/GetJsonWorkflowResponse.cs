namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetJsonWorkflow
{
    public class GetJsonWorkflowResponse
    {

        public string? Error { get; set; }
        public string? Token { get; set; }
        public Guid Id { get; set; }
        public bool IsValid { get; set; }
    }
}
