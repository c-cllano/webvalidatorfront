namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflowsByFilter
{

    internal class GetWorkflowsByFilterResponse
    {
        public string? Error { get; set; }
        public string? Token { get; set; }
        public Guid Id { get; set; }
        public bool IsValid { get; set; }
    }
}
