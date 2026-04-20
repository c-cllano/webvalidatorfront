using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.DeleteWorkflow
{

    public record DeleteWorkflowNodeCommand(DeleteWorflowNodeRequest Request) : IRequest<bool>;
}
