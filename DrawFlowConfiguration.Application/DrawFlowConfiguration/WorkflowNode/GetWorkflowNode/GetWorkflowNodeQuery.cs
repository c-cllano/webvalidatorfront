using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.WorkflowNode.GetWorkflowNode
{

    public class GetWorkflowNodeQuery : IRequest<object>
    {
        public int? workFlowNodeID { get; set; }
        public string? name { get; set; }
    }
}
