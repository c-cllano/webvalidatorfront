using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.ValidationProcesses.ValidateBiometric;
using Process.Domain.Entities;
using Process.Domain.Parameters.ProcessDocuments;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [PlatformConnection(Constants.OkeyStudio)]
    public class ValidateBiometricOkeyService(
        ICitizenRepository citizenRepository,
        IBlobStorageService blobStorageService,
        ICitizenBiometricsDocumentsRepository biometricsRepository,
        IAntiSpoofingStrategyService<IIbetaStrategyValidationBiometricService> antiSpoofingStrategy,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository
    ) : IValidateBiometricService
    {
        private readonly ICitizenRepository _citizenRepository = citizenRepository;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly ICitizenBiometricsDocumentsRepository _biometricsRepository = biometricsRepository;
        private readonly IAntiSpoofingStrategyService<IIbetaStrategyValidationBiometricService> _antiSpoofingStrategy = antiSpoofingStrategy;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;

        public async Task<ValidateBiometricResponse> ValidateBiometricAsync(
            ValidateBiometricCommand request,
            ValidationProcess validationProcess
        )
        {
            Citizen? citizen = await _citizenRepository.GetCitizenByGuidAsync(request.CitizenGUID)
                ?? throw new KeyNotFoundException($"No existe ciudadano con guid: {request.CitizenGUID}");

            string urlBiometric = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, request.Biometric);

            string urlBiometricGesture = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, request.BiometricGesture);

            await CreateValidationProcessDocuments(request, urlBiometric, (int)ServicioSubTipoEnum.SELFIE);
            await CreateValidationProcessDocuments(request, urlBiometricGesture, (int)ServicioSubTipoEnum.GESTURE);

            CitizenBiometricsDocuments? biometricCitizen = await _biometricsRepository
                .GetBiometricsByCitizenIdAndServiceTypeAsync(citizen.CitizenId, (int)ServiciosEnum.BIOMETRIA_FACIAL)
                    ?? throw new KeyNotFoundException($"No existe biometría del ciudadano con guid: {request.CitizenGUID}");

            (string imageBiometricCitizen, string mimeType) = await _blobStorageService
                .DownloadFileAsBase64ByUrlAsync(biometricCitizen.UrlFile);

            string antiSpoofing = string.IsNullOrEmpty(request.AntiSpoofing)
                ? Constants.Ibeta2
                : request.AntiSpoofing;

            var strategy = _antiSpoofingStrategy.Resolve(antiSpoofing);

            return await strategy.ExecuteAsync(
                request,
                validationProcess.AgreementGUID!.Value,
                urlBiometric,
                urlBiometricGesture,
                imageBiometricCitizen,
                mimeType
            );
        }

        private async Task CreateValidationProcessDocuments(
            ValidateBiometricCommand request,
            string urlFace,
            int subType
        )
        {
            ProcessDocumentsRequest processDocumentsRequest = new()
            {
                UrlFile = urlFace,
                ProcessName = nameof(ValidateBiometricOkeyService),
                ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                ServiceSubType = subType,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _validationProcessDocumentsRepository
                .SaveValidationProcessDocumentsAsync(request.ValidationProcessId, processDocumentsRequest);
        }
    }
}
