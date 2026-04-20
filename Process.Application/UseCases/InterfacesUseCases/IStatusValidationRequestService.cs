using Process.Application.ValidationProcesses.StatusValidationRequest;
using Process.Domain.Entities;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IStatusValidationRequestService
    {
        Task<StatusValidationRequestResponse> StatusValidationRequestAsync(StatusValidationRequestCommand request, AgreementOkeyStudio agreementOkeyStudio);
    }
}
