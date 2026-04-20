using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflow
{
     public class GetWorkflowHandler : IRequestHandler<GetWorkflowQuery, object>
    {
        private readonly IValidationTransaction _transaction;

        public GetWorkflowHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<object> Handle(GetWorkflowQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _transaction.GetAllWorkflows();
            return configuration;
        }

    }

}