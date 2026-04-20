using MediatR;

namespace Process.Application.Auth.GetToken
{
    public class GetTokenQuery : IRequest<GetTokenResponse> 
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
