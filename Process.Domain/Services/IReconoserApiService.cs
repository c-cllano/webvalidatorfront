using Microsoft.AspNetCore.Http;
using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface IReconoserApiService
    {
        Task<TokenReconoserIDResponse> GetTokenReconoserAsync();
        Task<SaveDocumentBothSidesReconoserIDResponse> SaveDocumentBothSidesReconoserAsync(object contentBody, object? contentBodyExternalRequest);
        Task<CompareFacesReconoserIDResponse> CompareFacesReconoserAsync(object contentBody, object? contentBodyExternalRequest);
        Task<SaveBiometricReconoserIDResponse> SaveBiometricReconoserAsync(object contentBody, object? contentBodyExternalRequest);
        Task<SaveBiometricReconoserIDResponse> SaveBiometricV3ReconoserAsync(object contentMultipartForm, object? contentBodyExternalRequest, IFormFile biometric, IFormFile biometricGesture);
        Task<StatusValidationRequestReconoserIDResponse> StatusValidationRequestReconoserAsync(object contentBody);
        Task<ValidateBiometricReconoserIDResponse> ValidateBiometricReconoserAsync(object contentBody, object? contentBodyExternalRequest);
        Task<ValidateBiometricReconoserIDResponse> ValidateBiometricV3ReconoserAsync(object contentMultipartForm, object? contentBodyExternalRequest, IFormFile biometric, IFormFile biometricGesture);
        Task<ConsultValidationReconoserIDResponse> ConsultValidationReconoserAsync(object contentBody);
        Task<string> ConsultValidationReconoserLegacyAsync(object contentBody);
        Task<ConsultAgreementProcessReconoserIDResponse> ConsultAgreementProcessReconoserAsync(Guid agreementProcessGuid);
        Task<TempKeysReconoserIDResponse> GetTempKeysAsync(Guid validationProcessGuid);
        Task<TransactionReconoserIDResponse> CancelProcessReconoserAsync(object contentBody);
    }
}
