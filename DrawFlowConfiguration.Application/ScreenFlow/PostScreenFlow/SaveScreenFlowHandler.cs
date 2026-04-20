using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.ScreenFlow.PostScreenFlow
{
 

    public class SaveScreenFlowHandler : IRequestHandler<SaveScreenFlowCommand, bool>
    {
        private readonly IScreenFlowRepository _transaction;

        public SaveScreenFlowHandler(IScreenFlowRepository transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(SaveScreenFlowCommand request, CancellationToken cancellationToken)
            => await _transaction.SaveScreenFlow(request.request);
    }
}
