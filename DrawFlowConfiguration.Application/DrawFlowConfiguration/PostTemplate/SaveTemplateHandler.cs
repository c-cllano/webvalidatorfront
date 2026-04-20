using DrawFlowConfiguration.Application.DrawFlowConfiguration.PostWorkflows;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PostTemplate
{
 

    public class SaveTemplateHandler : IRequestHandler<SaveTemplateCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public SaveTemplateHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(SaveTemplateCommand request, CancellationToken cancellationToken)
            => await _transaction.SaveTemplateTransaction(request.request);
    }
}
