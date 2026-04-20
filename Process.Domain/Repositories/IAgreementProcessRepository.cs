using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IAgreementProcessRepository
    {
        Task<AgreementProcess> GetProcess(Guid procesoConvenioGuid);
    }
}
