using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IBiometricKeysTempRepository
    {
        
        Task<long> AddNewItemAsync(BiometricKey data);
 
        Task<BiometricKey?> GetByProcessAgreementGuidAsync(Guid processAgreementGuid);
    }
}
