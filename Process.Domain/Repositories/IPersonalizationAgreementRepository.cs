using Process.Domain.Parameters.PersonalizationAgreement;

namespace Process.Domain.Repositories
{
    public interface IPersonalizationAgreementRepository
    {
       Task<List<GetFrontConfigurationOut>> GetFrontConfiguration(long procesoConvenioId); 
    }
}
