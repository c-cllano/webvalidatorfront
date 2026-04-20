using DigitalSignature.Domain.Entities;

namespace DigitalSignature.Domain.Repositories
{
    public interface IParameterClientRepository
    {
        Task<IEnumerable<ParameterClient>> GetParameterClientByClientId(long clientId);
    }
}
