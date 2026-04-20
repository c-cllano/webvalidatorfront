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
    [AntiSpoofing(Constants.Ibeta2)]
    public class Ibeta2StrategyValidationBiometricService(
        IIBetaApiService ibetaApiService,
        ICompareFacesService compareFacesService,
        IValidationProcessScoresRepository scoreRepository
    ) : IIbetaStrategyValidationBiometricService
    {
        private readonly IIBetaApiService _ibetaApiService = ibetaApiService;
        private readonly ICompareFacesService _compareFacesService = compareFacesService;
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
            object multipartContentBody = CreateObject(urlBiometric, urlBiometricGesture);

            Ibeta2Response ibeta2Response = await _ibetaApiService
                .Ibeta2Async(request.Biometric, request.BiometricGesture, multipartContentBody);

            if (ibeta2Response != null && ibeta2Response.Summary != null && !ConstantsIbeta.ValidCodesOkIbeta2.Contains(ibeta2Response.Summary.Code))
            {
                string messageError = ConstantsIbeta.GetMessageIbeta2(ibeta2Response.Summary.Code);

                return new()
                {
                    IsValid = false,
                    Result = 0,
                    Score = 0,
                    IsHomologation = false,
                    IsSuccessful = false,
                    TransactionError = [
                        new ErrorTransactionResponse(){
                            Code = ibeta2Response.Summary.Code,
                            Description = messageError
                        }
                    ]
                };
            }

            CompareFacesCommand compareFacesCommand = new(
                ValidationProcessId: null,
                Face1: imageBiometricCitizen,
                Format1: mimeType,
                Face2: request.Biometric,
                Format2: request.Format,
                SaveTrace: true
            );

            CompareFacesResponse compareFacesResponse = await _compareFacesService
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
            string urlBiometric,
            string urlBiometricGesture
        )
        {
            object contentBodyExternalRequest = new
            {
                multipartContentFile1 = urlBiometric,
                multipartContentFile2 = urlBiometricGesture
            };

            return contentBodyExternalRequest;
        }
    }
}
