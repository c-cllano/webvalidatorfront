using MediatR;

namespace Process.Application.Sso.Account.ResetPassword
{
    public class ResetPasswordCommand : IRequest<object>
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
