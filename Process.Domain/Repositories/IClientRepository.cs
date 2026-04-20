using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IClientRepository
    {
        Task<Client?> GetClientByToken(Guid clientToken);
        Task<Client> GetClient(long clientId);
    }
}
