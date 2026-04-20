using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PauseWorkflow
{

    public record PauseWorkflowCommand(PauseWorkflowRequest Request) : IRequest<bool>;
}
