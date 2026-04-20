using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.ValidationProcesses.SaveBiometric;
using Process.Domain.Entities;
using Process.Domain.Parameters.ProcessDocuments;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [PlatformConnection(Constants.OkeyStudio)]
    public class SaveBiometricOkeyService(
        ICitizenRepository citizenRepository,
        IBlobStorageService blobStorageService,
        IAntiSpoofingStrategyService<IIbetaStrategySaveBiometricService> antiSpoofingStrategy,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository
    ) : ISaveBiometricService
    {
        private readonly ICitizenRepository _citizenRepository = citizenRepository;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IAntiSpoofingStrategyService<IIbetaStrategySaveBiometricService> _antiSpoofingStrategy = antiSpoofingStrategy;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;

        public async Task<SaveBiometricResponse> SaveBiometricAsync(SaveBiometricCommand request)
        {
            Citizen? citizen = await _citizenRepository.GetCitizenByGuidAsync(request.CitizenGUID)
                ?? throw new KeyNotFoundException($"No existe ciudadano con guid: {request.CitizenGUID}");

            string urlBiometric = await _blobStorageService
                .UploadFileAsync(request.CitizenGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, request.Value);

            string urlBiometricGesture = await _blobStorageService
                .UploadFileAsync(request.CitizenGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, request.BiometricGesture);

            await CreateValidationProcessDocuments(request, urlBiometric, (int)ServicioSubTipoEnum.SELFIE);
            await CreateValidationProcessDocuments(request, urlBiometricGesture, (int)ServicioSubTipoEnum.GESTURE);

            string antiSpoofing = string.IsNullOrEmpty(request.AntiSpoofing)
                ? Constants.Ibeta2
                : request.AntiSpoofing;

            var strategy = _antiSpoofingStrategy.Resolve(antiSpoofing);

            return await strategy.ExecuteAsync(
                request,
                citizen.CitizenId,
                urlBiometric,
                urlBiometricGesture
            );
        }

        private async Task CreateValidationProcessDocuments(
            SaveBiometricCommand request,
            string urlFace,
            int subType
        )
        {
            ProcessDocumentsRequest processDocumentsRequest = new()
            {
                UrlFile = urlFace,
                ProcessName = nameof(SaveBiometricOkeyService),
                ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                ServiceSubType = subType,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _validationProcessDocumentsRepository
                .SaveValidationProcessDocumentsAsync(request.ValidationProcessId, processDocumentsRequest);
        }
    }
}
