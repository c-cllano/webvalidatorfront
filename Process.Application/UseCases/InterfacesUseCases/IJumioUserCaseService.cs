using Process.Application.ValidationProcesses.SaveDocumentBothSides;
using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface IJumioUserCaseService
    {
        Task<GetProcessJumioResponse> JumioAsync(SaveDocumentBothSidesCommand request);
    }
}
