using MediatR;
using DrawFlowConfiguration.Domain.Parameters.Transaction;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow;


namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PostWorkflows
{
    public record SaveWorflowCommand(WorkflowsEntry request) : IRequest<bool>;
}



