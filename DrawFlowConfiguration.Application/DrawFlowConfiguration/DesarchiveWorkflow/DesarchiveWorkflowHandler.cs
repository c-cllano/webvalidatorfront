using DrawFlowConfiguration.Domain.Repositories;
using MediatR;


namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.DesarchiveWorkflow
{
    public class DesarchiveWorkflowHandler : IRequestHandler<DesarchiveWorkflowCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public DesarchiveWorkflowHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(DesarchiveWorkflowCommand request, CancellationToken cancellationToken)
            => await _transaction.DesarchiveWorkflow(request.Request);
    }
}
