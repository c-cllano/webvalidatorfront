using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IValidationProcessRepository
    {
        Task<ValidationProcess?> GetValidationProcessById(long id);
        Task<ValidationProcess?> GetValidationProcessByValidationProcessGuid(Guid validationProcessGuid);
        Task<ValidationProcess> SaveValidationProcessAsync(ValidationProcess validationProcess);
        Task<ValidationProcess> UpdateValidationProcessAsync(ValidationProcess validationProcess);
    }
}
