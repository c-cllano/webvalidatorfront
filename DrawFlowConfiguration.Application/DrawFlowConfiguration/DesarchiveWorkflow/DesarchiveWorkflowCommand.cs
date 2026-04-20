using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.DesarchiveWorkflow
{
    public record DesarchiveWorkflowCommand(DesarchiveWorkflowRequest Request) : IRequest<bool>;
}
