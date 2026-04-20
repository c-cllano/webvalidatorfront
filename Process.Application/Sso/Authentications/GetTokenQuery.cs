using MediatR;
using Process.Domain.Entities;

namespace Process.Application.Sso.Authentications
{
    public record GetTokenQuery(
        string GrantType,
        string ClientId,
        string UserName,
        string Password,
        string ClientSecret,
        string RefreshToken,
        string? ClientType
        ) : IRequest<SsoServiceResult<GetTokenResponse>>;
}
