using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;
using MediatR;

namespace DigitalSignature.Application.GenerateDocument
{
    public record GenerateDocumentCommand(
        long ClientId,
        Guid TemplateSerial,
        IEnumerable<GenerateDocumentDataRequest> Data
    ) : IRequest<GenerateDocumentResponse>;
}
