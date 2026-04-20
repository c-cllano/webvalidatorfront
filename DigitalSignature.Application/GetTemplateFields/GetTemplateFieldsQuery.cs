using MediatR;

namespace DigitalSignature.Application.GetTemplateFields
{
    public record GetTemplateFieldsQuery(
        long ClientId,
        Guid TemplateSerial
    ) : IRequest<GetTemplateFieldsResponse>;
}
