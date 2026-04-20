using MediatR;

namespace Process.Application.ValidationProcesses.StatusValidationRequest
{
    public record StatusValidationRequestCommand(
        int? CountryId,
        long AgreementId,
        int? ValidationType,
        string DocumentType,
        string DocumentNumber,
        string? User,
        long? WorkflowId,
        string? Name,
        string? Email,
        string? Phone,
        string? Indicative,
        CitizenDataRequest? CitizenData,
        bool? IsWhatsapp = false,
        int? RequestChannel = 0
    ) : IRequest<StatusValidationRequestResponse>;
}
