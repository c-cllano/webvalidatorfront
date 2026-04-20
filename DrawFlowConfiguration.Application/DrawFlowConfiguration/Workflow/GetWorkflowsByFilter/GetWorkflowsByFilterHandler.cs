using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflow;
using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflowsByFilter
{
    public class GetWorkflowsByFilterHandler
        : IRequestHandler<GetWorkflowsByFilterQuery, IEnumerable<WorkflowsEntry>>
    {
        private readonly IValidationTransaction _transaction;

        public GetWorkflowsByFilterHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<IEnumerable<WorkflowsEntry>> Handle(
            GetWorkflowsByFilterQuery request,
            CancellationToken cancellationToken)
        {
            return await _transaction.GetWorkflowsByFilter(
                request.WorkFlowId,
                request.AgreementId,
                request.Status
            );
        }
    }

}
