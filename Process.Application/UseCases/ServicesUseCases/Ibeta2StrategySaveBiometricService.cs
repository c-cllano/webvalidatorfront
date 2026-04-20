using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.SaveBiometric;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [AntiSpoofing(Constants.Ibeta2)]
    public class Ibeta2StrategySaveBiometricService(
        IIBetaApiService ibetaApiService,
        ICitizenBiometricsDocumentsRepository biometricsRepository
    ) : IIbetaStrategySaveBiometricService
    {
        private readonly IIBetaApiService _ibetaApiService = ibetaApiService;
        private readonly ICitizenBiometricsDocumentsRepository _biometricsRepository = biometricsRepository;

        public async Task<SaveBiometricResponse> ExecuteAsync(
            SaveBiometricCommand request,
            long citizenId,
            string urlBiometric,
            string urlBiometricGesture
        )
        {
            object multipartContentBody = CreateObject(urlBiometric, urlBiometricGesture);

            Ibeta2Response ibeta2Response = await _ibetaApiService
                .Ibeta2Async(request.Value, request.BiometricGesture!, multipartContentBody);

            if (ibeta2Response != null && ibeta2Response.Summary != null && !ConstantsIbeta.ValidCodesOkIbeta2.Contains(ibeta2Response.Summary.Code))
            {
                string messageError = ConstantsIbeta.GetMessageIbeta2(ibeta2Response.Summary.Code);

                return new()
                {
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
