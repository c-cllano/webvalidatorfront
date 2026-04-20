using MediatR;

namespace DigitalSignature.Application.GetTemplate
{
    public record GetTemplateQuery(
        long ClientId,
        Guid TemplateSerial
    ) : IRequest<GetTemplateResponse>;
}
