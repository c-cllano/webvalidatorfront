using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.ScreenFlow.GetScreenFlow
{


    public class GetScreenFlowHandler : IRequestHandler<GetScreenFlowQuery, object>
    {
        private readonly IScreenFlowRepository _transaction;

        public GetScreenFlowHandler(IScreenFlowRepository transaction)
        {
            _transaction = transaction;
        }

        public async Task<object> Handle(GetScreenFlowQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _transaction.GetAllScreenFlow();
            return configuration;
        }

    }
}
