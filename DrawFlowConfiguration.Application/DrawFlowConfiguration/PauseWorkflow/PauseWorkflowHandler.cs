using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PauseWorkflow
{
    public class PauseWorkflowHandler(IValidationTransaction transaction) : IRequestHandler<PauseWorkflowCommand, bool>
    {
        private readonly IValidationTransaction _transaction = transaction;

        public async Task<bool> Handle(PauseWorkflowCommand request, CancellationToken cancellationToken)
            => await _transaction.PauseWorkflow(request.Request);
    }
}
