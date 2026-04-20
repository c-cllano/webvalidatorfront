using MediatR;
using Process.Domain.Entities;
using Process.Domain.Services;

namespace Process.Application.Sso.Authentications
{
    public class GetTokenHandler(ISsoService ssoService) : IRequestHandler<GetTokenQuery, SsoServiceResult<GetTokenResponse>>
    {
        private readonly ISsoService _ssoService = ssoService;

        public async Task<SsoServiceResult<GetTokenResponse>> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            Token token = new()
            {
                GrantType = request.GrantType,
                ClientId = request.ClientId,
                UserName = request.UserName,
                Password = request.Password,
                ClientSecret = request.ClientSecret,
                RefreshToken = request.RefreshToken,
                ClientType = request.ClientType
            };

            var result = await _ssoService.GetToken(token);

            if (!result.Success || result.Data == null)
            { 
                return SsoServiceResult<GetTokenResponse>.Fail(result.Error ?? "Error desconocido", result.StatusCode ?? 400);
            } 
           
            var response = new GetTokenResponse
            {
                access_token = result.Data.access_token,
                refresh_token = result.Data.refresh_token,
                expires_in = result.Data.expires_in,
                scope = result.Data.scope,
                id_token = result.Data.id_token,
                token_type = result.Data.token_type
            };

            return SsoServiceResult<GetTokenResponse>.Ok(response);
        }
    }
}
