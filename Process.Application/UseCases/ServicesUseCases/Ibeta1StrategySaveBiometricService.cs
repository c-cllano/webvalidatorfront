using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.CompareFaces;
using Process.Application.ValidationProcesses.SaveBiometric;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [AntiSpoofing(Constants.Ibeta1)]
    public class Ibeta1StrategySaveBiometricService(
        IIBetaApiService ibetaApiService,
        ICompareFacesService compareFacesService,
        ICitizenBiometricsDocumentsRepository biometricsRepository,
        IValidationProcessScoresRepository scoreRepository
    ) : IIbetaStrategySaveBiometricService
    {
        private readonly IIBetaApiService _ibetaApiService = ibetaApiService;
        private readonly ICompareFacesService _compareFacesService = compareFacesService;
        private readonly ICitizenBiometricsDocumentsRepository _biometricsRepository = biometricsRepository;
        private readonly IValidationProcessScoresRepository _scoreRepository = scoreRepository;

        public async Task<SaveBiometricResponse> ExecuteAsync(
            SaveBiometricCommand request,
            long citizenId,
            string urlBiometric,
            string urlBiometricGesture
        )
        {
            CompareFacesCommand compareFacesCommand = new(
                ValidationProcessId: null,
                Face1: request.Value,
                Format1: request.Format,
                Face2: request.BiometricGesture,
                Format2: request.FormatGesture,
                SaveTrace: true
            );

            CompareFacesResponse compareFacesResponse = await _compareFacesService
                .CompareFacesAsync(compareFacesCommand, request.CitizenGUID);

            await _scoreRepository.SaveValidationProcessScoresAsync(request.ValidationProcessId, compareFacesResponse.Score, ScoresCode.Liveness);

            if (!compareFacesResponse.IsValid)
            {
                return new()
                {
                    IsHomologation = false,
                    IsSuccessful = false,
                    TransactionError = [
                        new ErrorTransactionResponse(){
                            Code = compareFacesResponse.Code,
                            Description = compareFacesResponse.Message
                        }
                    ]
                };
            }

            object multipartContentBody = CreateObject(urlBiometric);
            Ibeta1Response responseBiometric = await _ibetaApiService.Ibeta1Async(request.Value, multipartContentBody);

            if (responseBiometric != null && responseBiometric.Summary != null && !ConstantsIbeta.ValidCodesOkIbeta1.Contains(responseBiometric.Summary.Code))
            {
                string messageError = ConstantsIbeta.GetMessageIbeta1(responseBiometric.Summary.Code);

                return new()
                {
                    IsHomologation = false,
                    IsSuccessful = false,
                    TransactionError = [
                        new ErrorTransactionResponse(){
                            Code = responseBiometric.Summary.Code,
                            Description = messageError
                        }
                    ]
                };
            }

            multipartContentBody = CreateObject(urlBiometricGesture);
            Ibeta1Response responseBiometricGesture = await _ibetaApiService.Ibeta1Async(request.BiometricGesture, multipartContentBody);

            if (responseBiometricGesture != null && responseBiometricGesture.Summary != null && !ConstantsIbeta.ValidCodesOkIbeta1.Contains(responseBiometricGesture.Summary.Code))
            {
                string messageError = ConstantsIbeta.GetMessageIbeta1(responseBiometricGesture.Summary.Code);

                return new()
                {
                    IsHomologation = false,
                    IsSuccessful = false,
                    TransactionError = [
                        new ErrorTransactionResponse(){
                            Code = responseBiometricGesture.Summary.Code,
                            Description = messageError
                        }
                    ]
                };
            }

            await SaveOrUpdateBiometricAsync(citizenId, urlBiometric);

            return new()
            {
                IsHomologation = true,
                IsSuccessful = true,
                TransactionError = []
            };
        }

        private async Task SaveOrUpdateBiometricAsync(
            long citizenId,
            string urlBiometric
        )
        {
            CitizenBiometricsDocuments? biometrics = await _biometricsRepository
                .GetBiometricsByCitizenIdAndServiceTypeAsync(citizenId, (int)ServiciosEnum.BIOMETRIA_FACIAL);

            if (biometrics == null)
            {
                biometrics = new()
                {
                    CitizenBiometricsDocumentsGuid = Guid.NewGuid(),
                    CitizenId = citizenId,
                    UrlFile = urlBiometric,
                    ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                    CreatedDate = DateTime.UtcNow.AddHours(-5),
                    Active = true,
                    IsDeleted = false
                };

                await _biometricsRepository.SaveBiometricsAsync(biometrics);
            }
            else
            {
                await _biometricsRepository.UpdateFileAsync(biometrics.CitizenBiometricsDocumentsId, urlBiometric);
            }
        }

        private static object CreateObject(
            string urlImage
        )
        {
            object contentBodyExternalRequest = new
            {
                multipartContentFile = urlImage
            };

            return contentBodyExternalRequest;
        }
    }
}
