using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using MediatR;

namespace DigitalSignature.Application.CreateTemplate
{
    public class CreateTemplateCommandHandler(
        ITemplateService templateService
    ) : IRequestHandler<CreateTemplateCommand, CreateTemplateResponse>
    {
        private readonly ITemplateService _templateService = templateService;

        public async Task<CreateTemplateResponse> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
        {
            ResultResponse<CreateTemplateSignatureResponse>? response = await _templateService
                .CreateTemplateAsync(request.ClientId, request.TemplateBase64, request.TemplateName) ?? throw new KeyNotFoundException("Error creando la plantilla: respuesta nula del servicio.");
            if (!response.Response)
            {
                string errorMessage = response.Message ?? "Error desconocido al crear la plantilla.";
                throw new KeyNotFoundException($"Error creando la plantilla: {errorMessage}");
            }
            if (response.Data == null)
            {
                throw new KeyNotFoundException("Error creando la plantilla: datos de respuesta nulos.");
            }

            return new()
            {
                Ok = response.Data.Ok,
                TemplateSerial = response.Data.TemplateSerial
            };
        }
    }
}
