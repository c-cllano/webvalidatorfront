using MediatR;
using DrawFlowConfiguration.Domain.Repositories;

namespace DrawFlowConfiguration.Application.ValidationTransaction.SaveTransaction
{
    public class SaveTransactionHandler : IRequestHandler<SaveTransactionCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public SaveTransactionHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(SaveTransactionCommand request, CancellationToken cancellationToken) 
            => await _transaction.SaveValidationTransaction(request.request);
    }
}
