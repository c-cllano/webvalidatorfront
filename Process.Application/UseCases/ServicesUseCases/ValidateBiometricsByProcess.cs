using System.Text.Json;
using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.ValidationProcesses.ValidateBiometric;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;
using Capture = Process.Domain.Entities.Capture;

namespace Process.Application.UseCases.ServicesUseCases
{
    [PlatformConnection(Constants.OkeyStudio)]
    public class ValidateBiometricsByProcess(
        IBlobStorageService blobStorageService,
        ICitizenRepository citizenRepository,
        ICitizenBiometricsDocumentsRepository biometricsRepository,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository,
        IValidationProcessScoresRepository validationProcessScoresRepository,
        ICompareFacesMegviApiService compareFacesMegviApiService) : IValidateBiometricsByProcess
    {

        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly ICitizenRepository _citizenRepository = citizenRepository;
        private readonly ICitizenBiometricsDocumentsRepository _biometricsRepository = biometricsRepository;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;
        private readonly ICompareFacesMegviApiService _compareFacesMegviApiService = compareFacesMegviApiService;
        private readonly IValidationProcessScoresRepository _validationProcessScoresRepository = validationProcessScoresRepository;

        public async Task<List<ValidateBiometricResponse>> ValidateBiometricByProcessAsync(int validationProcessId, int citizenId, int processType)
        {
            var result = new List<ValidateBiometricResponse>();
            var resultValidationByProcess = await _validationProcessDocumentsRepository.GetDocumentsByValidationProcessIdAsync(validationProcessId);
            var jsonDeserialize = DeserializeJson(resultValidationByProcess?.FirstOrDefault()?.Trazability);

            var documentFrontUrl = GetLatestUrl(jsonDeserialize, "SaveDocumentBothSidesReconoserService", "2");
            if (string.IsNullOrEmpty(documentFrontUrl))
                return result;

            var (imageDocumentFront, _) = await _blobStorageService.DownloadFileAsBase64ByUrlAsync(documentFrontUrl);

            if (processType == 2) // Validation
            {
                var photoUrl = GetLatestUrl(jsonDeserialize, "ValidateBiometricReconoserService");
                if (!string.IsNullOrEmpty(photoUrl))
                {
                    var (imagePhoto, _) = await _blobStorageService.DownloadFileAsBase64ByUrlAsync(photoUrl);
                    var response = await CompareAndBuildResponse(imagePhoto, imageDocumentFront);
                    result.Add(response);

                    await _validationProcessScoresRepository.SaveValidationProcessScoresAsync(validationProcessId, response.Score, ScoresCode.RostroSelfieVsRostroDocumento);
                }
            }

            var biometricCitizen = await _biometricsRepository
                    .GetBiometricsByCitizenIdAndServiceTypeAsync(citizenId, (int)ServiciosEnum.BIOMETRIA_FACIAL)
                    ?? throw new KeyNotFoundException($"No existe biometría del ciudadano con id: {citizenId}");

            var (imageBiometricCitizen, _) = await _blobStorageService.DownloadFileAsBase64ByUrlAsync(biometricCitizen.UrlFile);

            var responseEnrollment = await CompareAndBuildResponse(imageBiometricCitizen, imageDocumentFront);
            result.Add(responseEnrollment);

            await _validationProcessScoresRepository.SaveValidationProcessScoresAsync(validationProcessId, responseEnrollment.Score, ScoresCode.RostroEnroladoVsRostroDocumento);

            return result;
        }

        private string? GetLatestUrl(List<Capture> captures, string processName, string? serviceSubType = null)
        {
            return captures
                .Where(w => w.ProcessName == processName && (serviceSubType == null || w.ServiceSubType == serviceSubType))
                .OrderByDescending(x => x.Date)
                .Select(s => s.UrlFile)
                .FirstOrDefault();
        }

        private async Task<ValidateBiometricResponse> CompareAndBuildResponse(string imageOne, string imageTwo)
        {
            var resultValidation = await _compareFacesMegviApiService.CompareFacesMegviApiAsync(imageOne, imageTwo, null);

            if (!string.IsNullOrEmpty(resultValidation.Error))
                throw new KeyNotFoundException($"Error al comparar rostros: {resultValidation.Error}");

            decimal scoreConfidence = ScoreRulesHelper.HomologationScoreMegvi(resultValidation.Confidence);
            int resultC = ScoreRulesHelper.GetResult(scoreConfidence);
            bool isValid = resultC == (int)ResultadoBiometriaEnum.OK;

            return new ValidateBiometricResponse
            {
                Score = scoreConfidence,
                IsValid = isValid,
                Result = resultC.ToString()
            };
        }


        private List<Capture> DeserializeJson(string? jsonString)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = JsonSerializer.Deserialize<List<Capture>>(jsonString ?? string.Empty, options);

            return response ?? new List<Capture>();
        }
    }
}
