using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.ArchiveWorkflow
{

    public record ArchiveWorkflowCommand(ArchiveWorkflowRequest Request) : IRequest<bool>;
}
