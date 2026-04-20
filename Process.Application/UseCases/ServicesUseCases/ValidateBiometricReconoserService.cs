using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.ValidateBiometric;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Parameters.ProcessDocuments;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [PlatformConnection(Constants.ReconoserID)]
    public class ValidateBiometricReconoserService(
        IReconoserApiService reconoserApiService,
        IBlobStorageService blobStorageService,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository,
        IExtractImageService extractImageService
    ) : IValidateBiometricService
    {
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;
        private readonly IExtractImageService _extractImageService = extractImageService;

        public async Task<ValidateBiometricResponse> ValidateBiometricAsync(
            ValidateBiometricCommand request,
            ValidationProcess validationProcess
        )
        {
            string biometric = await _extractImageService
                .ExtractImageAsync(validationProcess.AgreementGUID!.Value, validationProcess.ValidationProcessGUID!.Value, request.Biometric);

            string biometricGesture = await _extractImageService
                .ExtractImageAsync(validationProcess.AgreementGUID!.Value, validationProcess.ValidationProcessGUID!.Value, request.BiometricGesture);

            string urlImage = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, biometric);

            string urlImageGesture = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, biometricGesture);

            await CreateValidationProcessDocuments(request, urlImage, (int)ServicioSubTipoEnum.SELFIE);
            await CreateValidationProcessDocuments(request, urlImageGesture, (int)ServicioSubTipoEnum.GESTURE);

            (object contentBody, object contentBodyExternalRequest) = CreateObjects(validationProcess, request, urlImage, urlImageGesture);

            ValidateBiometricReconoserIDResponse response = await _reconoserApiService
                .ValidateBiometricReconoserAsync(contentBody, contentBodyExternalRequest);

            return new()
            {
                IsValid = response.IsValid!.Value,
                Result = response.Result!,
                Score = response.Score!.Value,
                IsHomologation = response.RespuestaTransaccion!.IsHomologacion!.Value,
                IsSuccessful = response.RespuestaTransaccion!.EsExitosa!.Value,
                TransactionError = response.RespuestaTransaccion?.ErrorEntransaccion?
                    .Select(r => new ErrorTransactionResponse
                    {
                        Code = r.Codigo,
                        Description = r.Descripcion
                    })
                    .ToList() ?? []
            };
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
                ProcessName = nameof(ValidateBiometricReconoserService),
                ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                ServiceSubType = subType,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _validationProcessDocumentsRepository
                .SaveValidationProcessDocumentsAsync(request.ValidationProcessId, processDocumentsRequest);
        }

        private static (object, object) CreateObjects(
            ValidationProcess validationProcess,
            ValidateBiometricCommand request,
            string urlImage,
            string urlImageGesture
        )
        {
            object contentBody = new
            {
                guidCiudadano = request.CitizenGUID,
                guidProcesoConvenio = validationProcess.ValidationProcessGUID,
                formato = request.Format,
                subtipo = request.SubType,
                idServicio = request.ServiceId,
                biometria = request.Biometric,
                biometriaGesto = request.BiometricGesture,
                formatoGesto = request.FormatGesture
            };

            object contentBodyExternalRequest = new
            {
                guidCiudadano = request.CitizenGUID,
                guidProcesoConvenio = validationProcess.ValidationProcessGUID,
                formato = request.Format,
                subtipo = request.SubType,
                idServicio = request.ServiceId,
                biometria = urlImage,
                biometriaGesto = urlImageGesture,
                formatoGesto = request.FormatGesture
            };

            return (contentBody, contentBodyExternalRequest);
        }
    }
}
