using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using DigitalSignature.Domain.Services;
using MediatR;

namespace DigitalSignature.Application.GenerateDocument
{
    public class GenerateDocumentCommandHandler(
        IDocumentService documentService
    ) : IRequestHandler<GenerateDocumentCommand, GenerateDocumentResponse>
    {
        private readonly IDocumentService _documentService = documentService;

        public async Task<GenerateDocumentResponse> Handle(GenerateDocumentCommand request, CancellationToken cancellationToken)
        {
            ResultResponse<GenerateDocumentSignatureResponse>? response = await _documentService
                .GenerateDocumentAsync(request.ClientId, request.TemplateSerial, request.Data) ?? throw new KeyNotFoundException("Error generando el documento: respuesta nula del servicio.");
            if (!response.Response)
            {
                string errorMessage = response.Message ?? "Error desconocido generando el documento.";
                throw new KeyNotFoundException($"Error generando el documento: {errorMessage}");
            }
            if (response.Data == null)
            {
                throw new KeyNotFoundException("Error generando el documento: datos nulos en la respuesta.");
            }
            return new GenerateDocumentResponse
            {
                Ok = response.Data.Ok,
                DocumentBase64 = (!string.IsNullOrEmpty(response.Data.documentoBase64) ? response.Data.documentoBase64 : response.Data.base64)
            };
        }
    }
}
