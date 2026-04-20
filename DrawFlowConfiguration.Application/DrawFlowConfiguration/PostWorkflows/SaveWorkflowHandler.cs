using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PostWorkflows
{
  


    public class SaveWorkflowHandler : IRequestHandler<SaveWorflowCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public SaveWorkflowHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(SaveWorflowCommand request, CancellationToken cancellationToken)
            => await _transaction.SaveValidationTransaction(request.request);
    }
}
