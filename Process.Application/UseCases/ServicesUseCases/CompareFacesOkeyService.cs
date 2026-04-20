using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.ValidationProcesses.CompareFaces;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Parameters.ProcessDocuments;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [PlatformConnection(Constants.OkeyStudio)]
    public class CompareFacesOkeyService(
        IBlobStorageService blobStorageService,
        ICompareFacesMegviApiService compareFacesMegviApiService,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository
    ) : ICompareFacesService
    {
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly ICompareFacesMegviApiService _compareFacesMegviApiService = compareFacesMegviApiService;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;

        public async Task<CompareFacesResponse> CompareFacesAsync(
            CompareFacesCommand request,
            Guid guidValue
        )
        {
            string urlFace1 = await _blobStorageService
                .UploadFileAsync(guidValue.ToString()!, AzureBlobStorageFolders.ValidationFiles, request.Face1);

            string urlFace2 = await _blobStorageService
                .UploadFileAsync(guidValue.ToString()!, AzureBlobStorageFolders.ValidationFiles, request.Face2);

            await CreateValidationProcessDocuments(request, urlFace1);
            await CreateValidationProcessDocuments(request, urlFace2);

            object multipartContentBody = CreateObject(urlFace1, urlFace2);

            FaceCompareMegviResponse res = await _compareFacesMegviApiService
                .CompareFacesMegviApiAsync(request.Face1, request.Face2, multipartContentBody);

            if (!string.IsNullOrEmpty(res.Error))
            {
                throw new KeyNotFoundException($"Error al comparar rostros: {res.Error}");
            }

            decimal scoreConfidence = ScoreRulesHelper.HomologationScoreMegvi(res.Confidence);
            int result = ScoreRulesHelper.GetResult(scoreConfidence);
            bool isValid = result == (int)ResultadoBiometriaEnum.OK;

            return new()
            {
                IsValid = isValid,
                Result = result.ToString(),
                Score = scoreConfidence,
                Provider = (int)ProvidersEnum.MEGVI,
                Message = isValid
                    ? "Validado correctamente"
                    : "No fue posible validar su rostro, por favor reintentar nuevamente"
            };
        }

        private async Task CreateValidationProcessDocuments(
            CompareFacesCommand request,
            string urlFace
        )
        {
            if (request.ValidationProcessId != null && request.ValidationProcessId > 0)
            {
                ProcessDocumentsRequest processDocumentsRequest = new()
                {
                    UrlFile = urlFace,
                    ProcessName = nameof(CompareFacesOkeyService),
                    ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                    Date = DateTime.UtcNow.AddHours(-5)
                };

                await _validationProcessDocumentsRepository
                    .SaveValidationProcessDocumentsAsync(request.ValidationProcessId!.Value, processDocumentsRequest);
            }
        }

        private static object CreateObject(
            string urlFace1,
            string urlFace2
        )
        {
            object contentBodyExternalRequest = new
            {
                multipartContentFile1 = urlFace1,
                multipartContentFile2 = urlFace2
            };

            return contentBodyExternalRequest;
        }
    }
}
