namespace Process.Application.DocumentTypeCapture.GetByCode
{
    public record GetDocumentTypeCaptureByCodeResponse(
        int DocumentTypeCaptureId,
        int? DocumentTypeId,
        int? Sides,
        bool InstantFeedback,
        DateTime CreatedDate,
        DateTime? UpdatedDate,
        bool Active,
        bool IsDeleted
    );
}