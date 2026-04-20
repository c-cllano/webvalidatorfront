using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.CompareFaces;
using Process.Application.ValidationProcesses.ValidateBiometric;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [AntiSpoofing(Constants.Ibeta1)]
    public class Ibeta1StrategyValidationBiometricService(
        ICompareFacesService compareFacesService,
        IIBetaApiService ibetaApiService,
        IValidationProcessScoresRepository scoreRepository
    ) : IIbetaStrategyValidationBiometricService
    {
        private readonly ICompareFacesService _compareFacesService = compareFacesService;
        private readonly IIBetaApiService _ibetaApiService = ibetaApiService;
        private readonly IValidationProcessScoresRepository _scoreRepository = scoreRepository;

        public async Task<ValidateBiometricResponse> ExecuteAsync(
            ValidateBiometricCommand request,
            Guid agreementGuid,
            string urlBiometric,
            string urlBiometricGesture,
            string imageBiometricCitizen,
            string mimeType
        )
        {
            CompareFacesCommand compareFacesCommand = new(
                ValidationProcessId: null,
                Face1: request.Biometric,
                Format1: request.Format,
                Face2: request.BiometricGesture,
                Format2: request.FormatGesture,
                SaveTrace: true
            );

            CompareFacesResponse compareFacesResponse = await _compareFacesService
                .CompareFacesAsync(compareFacesCommand, agreementGuid);

            await _scoreRepository.SaveValidationProcessScoresAsync(request.ValidationProcessId, compareFacesResponse.Score, ScoresCode.Liveness);

            if (!compareFacesResponse.IsValid)
            {
                return new()
                {
                    IsValid = compareFacesResponse.IsValid,
                    Result = compareFacesResponse.Result,
                    Score = compareFacesResponse.Score,
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
            Ibeta1Response responseBiometric = await _ibetaApiService.Ibeta1Async(request.Biometric, multipartContentBody);

            if (responseBiometric != null && responseBiometric.Summary != null && !ConstantsIbeta.ValidCodesOkIbeta1.Contains(responseBiometric.Summary.Code))
            {
                string messageError = ConstantsIbeta.GetMessageIbeta1(responseBiometric.Summary.Code);

                return new()
                {
                    IsValid = false,
                    Result = 0,
                    Score = 0,
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
                    IsValid = false,
                    Result = 0,
                    Score = 0,
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

            compareFacesCommand = new(
                ValidationProcessId: null,
                Face1: imageBiometricCitizen,
                Format1: mimeType,
                Face2: request.Biometric,
                Format2: request.Format,
                SaveTrace: true
            );

            compareFacesResponse = await _compareFacesService
                .CompareFacesAsync(compareFacesCommand, agreementGuid);

            await _scoreRepository.SaveValidationProcessScoresAsync(request.ValidationProcessId, compareFacesResponse.Score, ScoresCode.RostroSelfieVsRostroEnrolado);

            if (!compareFacesResponse.IsValid)
            {
                return new()
                {
                    IsValid = compareFacesResponse.IsValid,
                    Result = compareFacesResponse.Result,
                    Score = compareFacesResponse.Score,
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

            return new()
            {
                IsValid = compareFacesResponse.IsValid,
                Result = compareFacesResponse.Result,
                Score = compareFacesResponse.Score,
                IsHomologation = true,
                IsSuccessful = true,
                TransactionError = []
            };
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
