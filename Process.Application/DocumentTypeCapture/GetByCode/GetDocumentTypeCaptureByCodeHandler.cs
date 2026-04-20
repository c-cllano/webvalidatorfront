using MediatR;
using Process.Domain.Entities;
using Process.Domain.Repositories;

namespace Process.Application.DocumentTypeCapture.GetByCode
{
    public class GetDocumentTypeCaptureByCodeHandler(IDocumentTypeCaptureRepository repository)
        : IRequestHandler<GetDocumentTypeCaptureByCodeQuery, SsoServiceResult<GetDocumentTypeCaptureByCodeResponse>>
    {
        private readonly IDocumentTypeCaptureRepository _repository = repository;

        public async Task<SsoServiceResult<GetDocumentTypeCaptureByCodeResponse>> Handle(GetDocumentTypeCaptureByCodeQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetByCodeOrDefaultAsync(request.Code);
            if (result is null)
            {
                return SsoServiceResult<GetDocumentTypeCaptureByCodeResponse>.Fail("No se encontró configuración para el código especificado", 404);
            }

            var response = new GetDocumentTypeCaptureByCodeResponse(
                result.DocumentTypeCaptureId,
                result.DocumentTypeId,
                result.Sides,
                result.InstantFeedback,
                result.CreatedDate,
                result.UpdatedDate,
                result.Active,
                result.IsDeleted
            );

            return SsoServiceResult<GetDocumentTypeCaptureByCodeResponse>.Ok(response);
        }
    }
}