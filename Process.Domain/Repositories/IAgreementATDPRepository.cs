using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IAgreementATDPRepository
    {
        Task<AgreementATDP?> GetMaxAgreement(long agreementId);
    }
}
