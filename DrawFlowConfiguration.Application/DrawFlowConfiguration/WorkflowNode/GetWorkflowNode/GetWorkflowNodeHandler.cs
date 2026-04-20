using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.WorkflowNode.GetWorkflowNode
{

    public class GetWorkflowNodeHandler : IRequestHandler<GetWorkflowNodeQuery, object>
    {
        private readonly IValidationTransaction _transaction;

        public GetWorkflowNodeHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<object> Handle(GetWorkflowNodeQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _transaction.GetAllWorkflowNode(requets.workFlowNodeID,requets.name);
            return configuration;
        }

    }
}
