using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using MediatR;

namespace DigitalSignature.Application.GetTemplateFields
{
    public class GetTemplateFieldsQueryHandler(
        ITemplateService templateService
    ) : IRequestHandler<GetTemplateFieldsQuery, GetTemplateFieldsResponse>
    {
        private readonly ITemplateService _templateService = templateService;

        public async Task<GetTemplateFieldsResponse> Handle(GetTemplateFieldsQuery request, CancellationToken cancellationToken)
        {
            ResultResponse<GetTemplateFieldsSignatureResponse>? response = await _templateService
                .GetTemplateFieldsAsync(request.ClientId, request.TemplateSerial);

            if (response == null || !response.Response)
            {
                throw new KeyNotFoundException($"Error obteniendo los campos de la plantilla: {(response?.Message ?? "Respuesta nula")}");
            }

            if (response.Data == null)
            {
                throw new KeyNotFoundException("La respuesta no contiene datos de la plantilla.");
            }

            return new()
            {
                Ok = response.Data.Ok,
                Annotations = response.Data.Annotations
            };
        }
    }
}
