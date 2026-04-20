namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.WorkflowNode.GetWorkflowNode
{
    internal class GetWorkflowNodeResponse
    {
        public string? Error { get; set; }
        public string? Token { get; set; }
        public Guid Id { get; set; }
        public bool IsValid { get; set; }
    }
}
