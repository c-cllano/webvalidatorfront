using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.ArchiveWorkflow
{
    public class ArchiveWorkflowHandler : IRequestHandler<ArchiveWorkflowCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public ArchiveWorkflowHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(ArchiveWorkflowCommand request, CancellationToken cancellationToken)
            => await _transaction.ArchiveWorkflow(request.Request);
    }
}
