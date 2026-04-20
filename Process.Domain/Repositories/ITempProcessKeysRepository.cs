using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface ITempProcessKeysRepository
    {
        Task<TempProcessKeys?> GetTempProcessKeysByValidationProcessGuidAsync(Guid validationProcessGuid);
        Task<TempProcessKeys> SaveTempProcessKeysAsync(TempProcessKeys tempProcessKeys);
    }
}
