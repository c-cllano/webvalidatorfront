using MediatR;
using Process.Domain.Entities;

namespace Process.Application.DocumentTypeCapture.GetByCode
{
    public record GetDocumentTypeCaptureByCodeQuery(string Code) : IRequest<SsoServiceResult<GetDocumentTypeCaptureByCodeResponse>>;
}