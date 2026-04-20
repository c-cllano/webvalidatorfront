using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IParametersAgreementRepository
    {
        Task<IEnumerable<ParametersAgreement>> GetAllAsync();
        Task<IEnumerable<ParametersAgreement>> GetByAgreementIdAsync(long agreementId);
        Task<IEnumerable<ParametersAgreement>> GetParametersAgreementByAgreementGuidAsync(Guid agreementGuid, IEnumerable<string>? parameterCodes = null);
    }
}
