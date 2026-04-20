using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using MediatR;

namespace DigitalSignature.Application.GetTemplate
{
    public class GetTemplateQueryHandler(
        ITemplateService templateService
    ) : IRequestHandler<GetTemplateQuery, GetTemplateResponse>
    {
        private readonly ITemplateService _templateService = templateService;

        public async Task<GetTemplateResponse> Handle(GetTemplateQuery request, CancellationToken cancellationToken)
        {
            ResultResponse<GetTemplateSignatureResponse>? response = await _templateService
                .GetTemplateAsync(request.ClientId, request.TemplateSerial) ?? throw new KeyNotFoundException("Error obteniendo la plantilla: respuesta nula del servicio.");
            if (!response.Response)
            {
                throw new KeyNotFoundException($"Error obteniendo la plantilla: {response.Message ?? "Sin mensaje"}");
            }

            if (response.Data == null)
            {
                throw new KeyNotFoundException("Error obteniendo la plantilla: datos nulos en la respuesta.");
            }

            return new()
            {
                Ok = response.Data.Ok,
                DocumentBase64 = response.Data.DocumentBase64
            };
        }
    }
}
