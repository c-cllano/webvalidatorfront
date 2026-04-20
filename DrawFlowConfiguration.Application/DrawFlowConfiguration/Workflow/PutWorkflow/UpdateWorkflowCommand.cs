using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.PutWorkflow
{
    public record UpdateWorkflowCommand(SaveWorflowRequest Request) : IRequest<bool>;
}
