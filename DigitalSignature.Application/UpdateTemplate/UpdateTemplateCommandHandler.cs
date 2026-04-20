using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using MediatR;

namespace DigitalSignature.Application.UpdateTemplate
{
    public class UpdateTemplateCommandHandler(
        ITemplateService templateService
    ) : IRequestHandler<UpdateTemplateCommand, UpdateTemplateResponse>
    {
        private readonly ITemplateService _templateService = templateService;

        public async Task<UpdateTemplateResponse> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            ResultResponse<UpdateTemplateSignatureResponse> response = await _templateService
                .UpdateTemplateAsync(request.ClientId, request.TemplateSerial, request.TemplateBase64, request.TemplateName);

            if (response == null || !response.Response)
            {
                throw new KeyNotFoundException($"Error actualizando la plantilla: {response!.Code} - {response.Message}");
            }

            return new()
            {
                Ok = response.Data.Ok
            };
        }
    }
}
