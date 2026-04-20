using MediatR;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.ValidateBiometric;
using Process.Domain.Entities;
using Process.Domain.Parameters.Context;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Parameters.ProcessDocuments;
using Process.Domain.Repositories;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

// Inicio código generado por GitHub Copilot
namespace Process.Application.ValidationProcesses.ValidateBiometricV3
{
    public class ValidateBiometricV3CommandHandler(
        IValidationProcessRepository validationProcessRepository,
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IReconoserApiService reconoserApiService,
        IBlobStorageService blobStorageService,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository,
        ReconoserContext context
    ) : IRequestHandler<ValidateBiometricV3Command, ValidateBiometricResponse>
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;
        private readonly ReconoserContext _context = context;

        public async Task<ValidateBiometricResponse> Handle(ValidateBiometricV3Command request, CancellationToken cancellationToken)
        {
            ValidationProcess? validationProcess = await _validationProcessRepository
                .GetValidationProcessById(request.ValidationProcessId)
                    ?? throw new KeyNotFoundException("El proceso del convenio no fue encontrado.");

            AgreementOkeyStudio? agreementOkeyStudio = await _agreementOkeyStudioRepository
                .GetAgreementByGuid(validationProcess.AgreementGUID!.Value)
                    ?? throw new KeyNotFoundException("El convenio no fue encontrado.");

            _context.ChangeUrl = agreementOkeyStudio.ChangeUrl;
            _context.BaseUrlReconoser1 = agreementOkeyStudio.BaseUrlReconoser1;
            _context.BaseUrlReconoser2 = agreementOkeyStudio.BaseUrlReconoser2;
            _context.AgreementGUID = agreementOkeyStudio.AgreementGUID;

            await using Stream streamBiometric = request.Biometric.OpenReadStream();
            string urlBiometric = await _blobStorageService
                .UploadMediaAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, streamBiometric, request.Biometric.FileName);

            await using Stream streamBiometricGesture = request.BiometricGesture.OpenReadStream();
            string urlBiometricGesture = await _blobStorageService
                .UploadMediaAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, streamBiometricGesture, request.BiometricGesture.FileName);

            await CreateValidationProcessDocuments(request, urlBiometric, (int)ServicioSubTipoEnum.SELFIE);
            await CreateValidationProcessDocuments(request, urlBiometricGesture, (int)ServicioSubTipoEnum.GESTURE);

            (object contentMultipartForm, object contentBodyExternalRequest) = CreateObjects(request, urlBiometric, urlBiometricGesture);

            ValidateBiometricReconoserIDResponse response = await _reconoserApiService
                .ValidateBiometricV3ReconoserAsync(contentMultipartForm, contentBodyExternalRequest, request.Biometric, request.BiometricGesture);

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

        private static (object, object) CreateObjects(
            ValidateBiometricV3Command request,
            string urlBiometric,
            string urlBiometricGesture
        )
        {
            object contentMultipartForm = new
            {
                guidCiudadano = request.CitizenGUID,
                subtipo = request.SubType,
                idServicio = request.ServiceId,
                codeParameter = request.CodeParameter
            };

            object contentBodyExternalRequest = new
            {
                biometria = urlBiometric,
                biometriaGesto = urlBiometricGesture,
                guidCiudadano = request.CitizenGUID,
                subtipo = request.SubType,
                idServicio = request.ServiceId,
                codeParameter = request.CodeParameter
            };

            return (contentMultipartForm, contentBodyExternalRequest);
        }

        private async Task CreateValidationProcessDocuments(
            ValidateBiometricV3Command request,
            string urlFace,
            int subType
        )
        {
            ProcessDocumentsRequest processDocumentsRequest = new()
            {
                UrlFile = urlFace,
                ProcessName = nameof(ValidateBiometricV3CommandHandler),
                ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                ServiceSubType = subType,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _validationProcessDocumentsRepository
                .SaveValidationProcessDocumentsAsync(request.ValidationProcessId, processDocumentsRequest);
        }
    }
}
// Fin código generado por GitHub Copilot
