using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using MediatR;

namespace DigitalSignature.Application.Sign
{
    public class SignCommandHandler(
        ISignService signService
    ) : IRequestHandler<SignCommand, SignResponse>
    {
        private readonly ISignService _signService = signService;

        public async Task<SignResponse> Handle(SignCommand request, CancellationToken cancellationToken)
        {
            ResultResponse<SignSignatureResponse> response = await _signService
                .SignAsync(request.ClientId, request.Base64, request.Tsa, request.NombreFirma, request.LogoDefault, request.Base64LogoPersonalizado);

            if (response == null || !response.Response)
            {
                throw new KeyNotFoundException($"Error firmando el documento: {response!.Code} - {response.Message}");
            }

            return new()
            {
                Base64 = response.Data.Base64
            };
        }
    }
}
