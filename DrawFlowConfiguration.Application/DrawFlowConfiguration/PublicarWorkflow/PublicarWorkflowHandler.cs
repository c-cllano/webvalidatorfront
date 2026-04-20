using DrawFlowConfiguration.Application.DrawFlowConfiguration.ArchiveWorkflow;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PublicarWorkflow
{

    public class PublicarWorkflowHandler : IRequestHandler<PublicarWorkflowCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public PublicarWorkflowHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(PublicarWorkflowCommand request, CancellationToken cancellationToken)
            => await _transaction.PublicarWorkflow(request.Request);
    }
}
