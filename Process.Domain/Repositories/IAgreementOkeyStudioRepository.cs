using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IAgreementOkeyStudioRepository
    {
        Task<AgreementOkeyStudio?> GetAgreementById(long id);
        Task<AgreementOkeyStudio?> GetAgreementByGuid(Guid agreementGuid);
        Task<IReadOnlyList<AgreementOkeyStudio>> GetAgreementsByClientId(Guid ClientToken);
    }
}
