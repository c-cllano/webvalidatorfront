using Process.Application.ValidationProcesses.SaveDocumentBothSides;
using Process.Domain.Entities;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface ISaveDocumentBothSidesService
    {
        Task<SaveDocumentBothSidesResponse> SaveDocumentBothSidesAsync(SaveDocumentBothSidesCommand request, ValidationProcess validationProcess);
    }
}
