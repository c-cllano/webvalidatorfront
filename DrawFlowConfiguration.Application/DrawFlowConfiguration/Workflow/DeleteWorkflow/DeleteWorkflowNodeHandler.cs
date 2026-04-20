using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.PutWorkflow;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.DeleteWorkflow
{


    public class DeleteWorkflowNodeHandler : IRequestHandler<DeleteWorkflowNodeCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public DeleteWorkflowNodeHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(DeleteWorkflowNodeCommand request, CancellationToken cancellationToken)
            => await _transaction.UpdateWorkflowNode(request.Request);
    }
}
