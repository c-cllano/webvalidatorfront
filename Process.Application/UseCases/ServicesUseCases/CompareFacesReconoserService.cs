using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.ValidationProcesses.CompareFaces;
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
    public class CompareFacesReconoserService(
        IValidationProcessRepository validationProcessRepository,
        IReconoserApiService reconoserApiService,
        IBlobStorageService blobStorageService,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository,
        IExtractImageService extractImageService
    ) : ICompareFacesService
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;
        private readonly IExtractImageService _extractImageService = extractImageService;

        public async Task<CompareFacesResponse> CompareFacesAsync(CompareFacesCommand request, Guid guidValue)
        {
            ValidationProcess? validationProcess = await _validationProcessRepository
                .GetValidationProcessById(request.ValidationProcessId!.Value)
                    ?? throw new KeyNotFoundException("El proceso del convenio no fue encontrado.");

            string face1 = await _extractImageService
                .ExtractImageAsync(validationProcess.AgreementGUID!.Value, validationProcess.ValidationProcessGUID!.Value, request.Face1);

            string face2 = await _extractImageService
                .ExtractImageAsync(validationProcess.AgreementGUID!.Value, validationProcess.ValidationProcessGUID!.Value, request.Face2);

            string urlFace1 = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, face1);

            string urlFace2 = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, face2);

            await CreateValidationProcessDocuments(request, urlFace1, (int)ServicioSubTipoEnum.SELFIE);
            await CreateValidationProcessDocuments(request, urlFace2, (int)ServicioSubTipoEnum.GESTURE);

            (object contentBody, object contentBodyExternalRequest) = CreateObjects(validationProcess, request, urlFace1, urlFace2);

            CompareFacesReconoserIDResponse response = await _reconoserApiService.CompareFacesReconoserAsync(
                contentBody,
                contentBodyExternalRequest
            );

            return new()
            {
                IsValid = response.Data.IsValid,
                Result = response.Data.Result,
                Score = (decimal)response.Data.Score,
                Provider = response.Data.Provider,
                Code = response.Data.Code,
                Message = response.Data.Message
            };
        }

        private async Task CreateValidationProcessDocuments(
            CompareFacesCommand request,
            string urlFace,
            int subType
        )
        {
            if (request.ValidationProcessId != null && request.ValidationProcessId > 0)
            {
                ProcessDocumentsRequest processDocumentsRequest = new()
                {
                    UrlFile = urlFace,
                    ProcessName = nameof(CompareFacesReconoserService),
                    ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                    ServiceSubType = subType,
                    Date = DateTime.UtcNow.AddHours(-5)
                };

                await _validationProcessDocumentsRepository
                    .SaveValidationProcessDocumentsAsync(request.ValidationProcessId!.Value, processDocumentsRequest);
            }
        }

        private static (object, object) CreateObjects(
            ValidationProcess validationProcess,
            CompareFacesCommand request,
            string urlFace1,
            string urlFace2
        )
        {
            object contentBody = new
            {
                guidConvenio = validationProcess.AgreementGUID,
                rostro1 = request.Face1,
                formato1 = request.Format1,
                rostro2 = request.Face2,
                formato2 = request.Format2,
                guardarTraza = request.SaveTrace,
                procesoConvenioGuid = validationProcess.ValidationProcessGUID
            };

            object contentBodyExternalRequest = new
            {
                guidConvenio = validationProcess.AgreementGUID,
                rostro1 = urlFace1,
                formato1 = request.Format1,
                rostro2 = urlFace2,
                formato2 = request.Format2,
                guardarTraza = request.SaveTrace,
                procesoConvenioGuid = validationProcess.ValidationProcessGUID
            };

            return (contentBody, contentBodyExternalRequest);
        }
    }
}
