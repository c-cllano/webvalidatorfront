using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface ICitizenBiometricsDocumentsRepository
    {
        Task<CitizenBiometricsDocuments?> GetBiometricsByCitizenIdAndServiceTypeAsync(long citizenId, int serviceType, int? serviceSubType = null);
        Task<CitizenBiometricsDocuments> SaveBiometricsAsync(CitizenBiometricsDocuments biometrics);
        Task<CitizenBiometricsDocuments> UpdateFileAsync(long citizenBiometricsDocumentsId, string urlFile);
    }
}
