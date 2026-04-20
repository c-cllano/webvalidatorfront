namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflow
{
    internal class GetWorkflowResponse
    {
        public string? Error { get; set; }
        public string? Token { get; set; }
        public Guid Id { get; set; }
        public bool IsValid { get; set; }
    }
}
