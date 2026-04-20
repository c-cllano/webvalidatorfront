using MediatR;

namespace DigitalSignature.Application.UpdateTemplate
{
    public record UpdateTemplateCommand(
        long ClientId,
        string TemplateBase64,
        string TemplateName,
        Guid TemplateSerial
    ) : IRequest<UpdateTemplateResponse>;
}
