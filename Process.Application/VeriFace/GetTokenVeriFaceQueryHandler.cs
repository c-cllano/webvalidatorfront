using MediatR;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;

namespace Process.Application.VeriFace
{
    public class GetTokenVeriFaceQueryHandler(
        IVeriFaceApiService veriFaceApiService
    ) : IRequestHandler<GetTokenVeriFaceQuery, GetTokenVeriFaceResponse>
    {
        private readonly IVeriFaceApiService _veriFaceApiService = veriFaceApiService;

        public async Task<GetTokenVeriFaceResponse> Handle(GetTokenVeriFaceQuery request, CancellationToken cancellationToken)
        {
            VeriFaceResponse veriFaceResponse = await _veriFaceApiService.GetTokenVeriFaceAsync();

            return new()
            {
                Token = veriFaceResponse.Token,
                ExpiresAt = veriFaceResponse.ExpiresAt
            };
        }
    }
}
