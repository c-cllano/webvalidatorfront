using MediatR;
using Process.Domain.Repositories;
using Process.Domain.Services;

namespace Process.Application.Sso.Account.ResetPassword
{
    public class ResetPasswordHandler(ISsoService ssoService,
        IPasswordRecoveryHistoryRepository passwordRecoveryHistory) : IRequestHandler<ResetPasswordCommand, object>
    {
        private readonly ISsoService _ssoService = ssoService;
        private readonly IPasswordRecoveryHistoryRepository _passwordRecoveryHistory = passwordRecoveryHistory;
        public async Task<object> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.ResetPassword resetPassword = new()
            {
                Email = request.Email,
                Token = request.Token,
                NewPassword = request.NewPassword
            };

            if (!await _passwordRecoveryHistory.GetByToken(resetPassword.Token, request.Email))
            {
                await _passwordRecoveryHistory.SaveToken(new Domain.Entities.PasswordRecoveryHistory { Email = request.Email, Token = request.Token });

                var result = await _ssoService.ResetPassword(resetPassword);
                return result!;
            }
            else
            {
                return null!;
            }
        }
    }
}
