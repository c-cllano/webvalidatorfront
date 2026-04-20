using MediatR;
using Process.Domain.Parameters.Document;

namespace Process.Application.ValidationProcesses.SaveDocumentBothSides
{
    public record SaveDocumentBothSidesCommand(
        long ValidationProcessId,
        Guid CitizenGUID,
        string AditionalData,
        DocumentBothSideRequest Frontal,
        DocumentBothSideRequest? Reverse,
        string User
    ) : IRequest<SaveDocumentBothSidesResponse>;
}
