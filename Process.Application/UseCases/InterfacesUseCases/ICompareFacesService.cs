using Process.Application.ValidationProcesses.CompareFaces;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface ICompareFacesService
    {
        Task<CompareFacesResponse> CompareFacesAsync(CompareFacesCommand request, Guid guidValue);
    }
}
