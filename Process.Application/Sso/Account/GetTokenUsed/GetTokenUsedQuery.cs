using MediatR;

namespace Process.Application.Sso.Account.GetTokenUsed
{
    public class GetTokenUsedQuery : IRequest<bool>
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; }
    }
}
