using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.ScreenFlow.PutScreenFlow
{
 

    public class UpdateScreenFlowHandler : IRequestHandler<UpdateScreenFlowCommand, bool>
    {
        private readonly IScreenFlowRepository _transaction;

        public UpdateScreenFlowHandler(IScreenFlowRepository transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(UpdateScreenFlowCommand request, CancellationToken cancellationToken)
            => await _transaction.UpdateScreenFlow(request.Request);
    }
}
