using Process.Domain.Entities;

namespace Process.Application.UseCases.InterfacesUseCases
{
    public interface ITempKeysService
    {
        Task<TempProcessKeys> ValidateKeysAsync(Guid validationProcessGuid);
    }
}
