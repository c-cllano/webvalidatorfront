using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.SaveDocumentBothSides;
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
    public class SaveDocumentBothSidesReconoserService(
        IReconoserApiService reconoserApiService,
        IBlobStorageService blobStorageService,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository,
        IExtractImageService extractImageService
    ) : ISaveDocumentBothSidesService
    {
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;
        private readonly IExtractImageService _extractImageService = extractImageService;

        public async Task<SaveDocumentBothSidesResponse> SaveDocumentBothSidesAsync(
            SaveDocumentBothSidesCommand request,
            ValidationProcess validationProcess
        )
        {
            string imageFrontal = await _extractImageService
                .ExtractImageAsync(validationProcess.AgreementGUID!.Value, validationProcess.ValidationProcessGUID!.Value, request.Frontal.Value);

            string imageReverse = await _extractImageService
                .ExtractImageAsync(validationProcess.AgreementGUID!.Value, validationProcess.ValidationProcessGUID!.Value, request.Reverse!.Value);

            string urlFrontal = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, imageFrontal);

            string urlReverese = await _blobStorageService
                .UploadFileAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, imageReverse);

            await CreateValidationProcessDocuments(request, urlFrontal, (int)ServicioSubTipoEnum.ANVERSO);
            await CreateValidationProcessDocuments(request, urlReverese, (int)ServicioSubTipoEnum.REVERSO);

            (object contentBody, object contentBodyExternalRequest) = CreateObjects(validationProcess, request, urlFrontal, urlReverese);

            SaveDocumentBothSidesReconoserIDResponse response = await _reconoserApiService
                .SaveDocumentBothSidesReconoserAsync(contentBody, contentBodyExternalRequest);

            return new()
            {
                FrontalGUID = response.FrontalGUID,
                ReverseGUID = response.ReverseGUID,
                FrontalSuccessful = response.FrontalSuccessful,
                ReverseSuccessful = response.ReverseSuccessful,
                FrontalMessage = response.FrontalMessage,
                ReverseMessage = response.ReverseMessage,
                DocumentTypeDescription = response.Data?.Data?.DocumentTypeDescription,
                DocumentType = response.Data?.Data?.DocumentType,
                DocumentNumber = response.Data?.Data?.DocumentNumber,
                FirstName = response.Data?.Data?.FirstName,
                SecondName = response.Data?.Data?.SecondName,
                LastName = response.Data?.Data?.LastName,
                SecondLastName = response.Data?.Data?.SecondLastName,
                Sex = response.Data?.Data?.Sex,
                Rh = response.Data?.Data?.Rh,
                PlaceBirth = response.Data?.Data?.PlaceBirth,
                DateBirth = response.Data?.Data?.DateBirth,
                IsHomologation = response.RespuestaTransaccion?.IsHomologacion,
                IsSuccessful = response.RespuestaTransaccion?.EsExitosa,
                PlaceOfIssue = response.Data?.Data?.PlaceOfIssue,
                DateOfIssue = response.Data?.Data?.DateOfIssue,
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
            SaveDocumentBothSidesCommand request,
            string urlFace,
            int subType
        )
        {
            ProcessDocumentsRequest processDocumentsRequest = new()
            {
                UrlFile = urlFace,
                ProcessName = nameof(SaveDocumentBothSidesReconoserService),
                ServiceType = (int)ServiciosEnum.BIOMETRIA_DOCUMENTO,
                ServiceSubType = subType,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _validationProcessDocumentsRepository
                .SaveValidationProcessDocumentsAsync(request.ValidationProcessId, processDocumentsRequest);
        }

        private static (object, object) CreateObjects(
            ValidationProcess validationProcess,
            SaveDocumentBothSidesCommand request,
            string urlFrontal,
            string urlReverese
        )
        {
            object contentBody = new
            {
                guidProcesoConvenio = validationProcess.ValidationProcessGUID,
                guidCiu = request.CitizenGUID,
                datosAdi = request.AditionalData,
                anverso = new
                {
                    valor = request.Frontal.Value,
                    formato = request.Frontal.Format,
                },
                reverso = new
                {
                    valor = request.Reverse!.Value,
                    formato = request.Reverse.Format,
                },
                usuario = request.User
            };

            object contentBodyExternalRequest = new
            {
                guidProcesoConvenio = validationProcess.ValidationProcessGUID,
                guidCiu = request.CitizenGUID,
                datosAdi = request.AditionalData,
                anverso = new
                {
                    valor = urlFrontal,
                    formato = request.Frontal.Format,
                },
                reverso = new
                {
                    valor = urlReverese,
                    formato = request.Reverse.Format,
                },
                usuario = request.User
            };

            return (contentBody, contentBodyExternalRequest);
        }
    }
}
