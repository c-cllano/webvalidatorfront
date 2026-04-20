using MediatR;

namespace Process.Application.Sso.Account.ChangePassword
{
    public class ChangePasswordCommand : IRequest<object>
    {
        public string Email { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
