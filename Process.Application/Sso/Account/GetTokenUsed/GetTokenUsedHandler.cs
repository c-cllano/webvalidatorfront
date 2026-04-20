using MediatR;
using Process.Domain.Repositories;

namespace Process.Application.Sso.Account.GetTokenUsed
{
    public class GetTokenUsedHandler(IPasswordRecoveryHistoryRepository passwordRecoveryHistory) : IRequestHandler<GetTokenUsedQuery, bool>
    {
        private readonly IPasswordRecoveryHistoryRepository _passwordRecoveryHistory = passwordRecoveryHistory;

        public async Task<bool> Handle(GetTokenUsedQuery request, CancellationToken cancellationToken)
        {
            return await _passwordRecoveryHistory.GetByToken(request.Token, request.Email);
        }
    }
}
