using MediatR;
using UIConfiguration.Domain.Services;

namespace UIConfiguration.Application.Auth.GetToken
{
    public class GetTokenHandler(IAuthService authService) : IRequestHandler<GetTokenQuery, GetTokenResponse>
    {
        private readonly IAuthService _authService = authService;

        public async Task<GetTokenResponse> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            var token = await _authService.GetToken(request.ClientId, request.ClientSecret);
            return new GetTokenResponse
            {
                AccessToken = token.AccessToken,
                ErrorDescription = token.ErrorDescription,
                ExpiresIn = token.ExpiresIn,
                RefreshToken = token.RefreshToken
            };
        }
    }
}
