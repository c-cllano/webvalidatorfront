using MediatR;

namespace DigitalSignature.Application.CreateTemplate
{
    public record CreateTemplateCommand(
        long ClientId,
        string TemplateBase64,
        string TemplateName
    ) : IRequest<CreateTemplateResponse>;
}
