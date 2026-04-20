using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;


namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PublicarWorkflow
{
    public record PublicarWorkflowCommand(PublicarWorkflowRequest Request) : IRequest<bool>;
}
