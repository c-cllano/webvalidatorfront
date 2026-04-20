using MediatR;
using DrawFlowConfiguration.Domain.Parameters.Transaction;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow;

namespace DrawFlowConfiguration.Application.ValidationTransaction.SaveTransaction
{
    public record SaveTransactionCommand(WorkflowsEntry request) : IRequest<bool>;
}
