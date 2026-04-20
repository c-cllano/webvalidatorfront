using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.SaveBiometric;
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
    public class SaveBiometricReconoserService(
        IReconoserApiService reconoserApiService,
        IBlobStorageService blobStorageService,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository,
        IExtractImageService extractImageService,
        IValidationProcessRepository validationProcessRepository
    ) : ISaveBiometricService
    {
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;
        private readonly IExtractImageService _extractImageService = extractImageService;
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;

        public async Task<SaveBiometricResponse> SaveBiometricAsync(SaveBiometricCommand request)
        {
            ValidationProcess? validationProcess = await _validationProcessRepository
                .GetValidationProcessById(request.ValidationProcessId)
                    ?? throw new KeyNotFoundException("El proceso del convenio no fue encontrado.");

            string biometric = await _extractImageService
                .ExtractImageAsync(validationProcess.AgreementGUID!.Value, validationProcess.ValidationProcessGUID!.Value, request.Value);

            string urlImage = await _blobStorageService
                .UploadFileAsync(request.CitizenGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, biometric);

            await CreateValidationProcessDocuments(request, urlImage);

            (object contentBody, object contentBodyExternalRequest) = CreateObjects(request, urlImage);

            SaveBiometricReconoserIDResponse response = await _reconoserApiService
                .SaveBiometricReconoserAsync(contentBody, contentBodyExternalRequest);

            return new()
            {
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
            SaveBiometricCommand request,
            string urlFace
        )
        {
            ProcessDocumentsRequest processDocumentsRequest = new()
            {
                UrlFile = urlFace,
                ProcessName = nameof(SaveBiometricReconoserService),
                ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _validationProcessDocumentsRepository
                .SaveValidationProcessDocumentsAsync(request.ValidationProcessId, processDocumentsRequest);
        }

        private static (object, object) CreateObjects(
            SaveBiometricCommand request,
            string urlImage
        )
        {
            object contentBody = new
            {
                guidCiu = request.CitizenGUID,
                idServicio = request.ServiceId,
                subtipo = request.SubType,
                valor = request.Value,
                formato = request.Format,
                datosAdi = request.AditionalData,
                usuario = request.User,
                actualizar = request.Update,
                codeParameter = request.CodeParameter,
                biometriaGesto = request.BiometricGesture,
                formatoGesto = request.FormatGesture
            };

            object contentBodyExternalRequest = new
            {
                guidCiu = request.CitizenGUID,
                idServicio = request.ServiceId,
                subtipo = request.SubType,
                valor = urlImage,
                formato = request.Format,
                datosAdi = request.AditionalData,
                usuario = request.User,
                actualizar = request.Update,
                codeParameter = request.CodeParameter,
                biometriaGesto = request.BiometricGesture,
                formatoGesto = request.FormatGesture
            };

            return (contentBody, contentBodyExternalRequest);
        }
    }
}
