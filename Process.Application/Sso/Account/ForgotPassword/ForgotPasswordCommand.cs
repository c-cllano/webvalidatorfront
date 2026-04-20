using MediatR;

namespace Process.Application.Sso.Account.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<object>
    {
        public required string Email { get; init; }
        public required Guid Token { get; init; }
    }
}
