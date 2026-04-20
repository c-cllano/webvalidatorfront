using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface ICitizenRepository
    {
        Task<Citizen?> GetCitizenByDocumentNumberAndAgreementIdAsync(string documentNumber, long agreementId);
        Task<Citizen?> GetCitizenByGuidAsync(Guid citizenGuid);
        Task<Citizen> SaveCitizenAsync(Citizen citizen);
        Task<Citizen> UpdateCitizenAsync(Citizen citizen);
    }
}
