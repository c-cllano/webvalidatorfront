using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IAgreementRepository
    {
        Task<Agreement?> AgreementById(long agreementId);
    }
}
