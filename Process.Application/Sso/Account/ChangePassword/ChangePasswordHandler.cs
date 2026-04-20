using MediatR;
using Process.Domain.Services;

namespace Process.Application.Sso.Account.ChangePassword
{
    public class ChangePasswordHandler(ISsoService ssoService) : IRequestHandler<ChangePasswordCommand, object>
    {
        private readonly ISsoService _ssoService = ssoService;
        public async Task<object> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.ChangePassword changePassword = new()
            {
                Email = request.Email,
                OldPassword = request.OldPassword,
                NewPassword = request.NewPassword
            };
            var result = await _ssoService.ChangePassword(changePassword);
            return result!;
        }
    }
}
