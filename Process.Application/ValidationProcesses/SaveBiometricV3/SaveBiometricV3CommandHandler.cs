using MediatR;
using Process.Application.UseCases.Responses;
using Process.Application.ValidationProcesses.SaveBiometric;
using Process.Domain.Entities;
using Process.Domain.Parameters.Context;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Parameters.ProcessDocuments;
using Process.Domain.Repositories;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

// Inicio código generado por GitHub Copilot
namespace Process.Application.ValidationProcesses.SaveBiometricV3
{
    public class SaveBiometricV3CommandHandler(
        IValidationProcessRepository validationProcessRepository,
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IReconoserApiService reconoserApiService,
        IBlobStorageService blobStorageService,
        IValidationProcessDocumentsRepository validationProcessDocumentsRepository,
        ReconoserContext context
    ) : IRequestHandler<SaveBiometricV3Command, SaveBiometricResponse>
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IValidationProcessDocumentsRepository _validationProcessDocumentsRepository = validationProcessDocumentsRepository;
        private readonly ReconoserContext _context = context;

        public async Task<SaveBiometricResponse> Handle(SaveBiometricV3Command request, CancellationToken cancellationToken)
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

            await using Stream streamBiometric = request.Value.OpenReadStream();
            string urlBiometric = await _blobStorageService
                .UploadMediaAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, streamBiometric, request.Value.FileName);

            await using Stream streamBiometricGesture = request.BiometricGesture.OpenReadStream();
            string urlBiometricGesture = await _blobStorageService
                .UploadMediaAsync(validationProcess.AgreementGUID.ToString()!, AzureBlobStorageFolders.ValidationFiles, streamBiometricGesture, request.BiometricGesture.FileName);

            await CreateValidationProcessDocuments(request, urlBiometric);

            (object contentMultipartForm, object contentBodyExternalRequest) = CreateObjects(request, urlBiometric, urlBiometricGesture);

            SaveBiometricReconoserIDResponse response = await _reconoserApiService
                .SaveBiometricV3ReconoserAsync(contentMultipartForm, contentBodyExternalRequest, request.Value, request.BiometricGesture);

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
            SaveBiometricV3Command request,
            string urlFace
        )
        {
            ProcessDocumentsRequest processDocumentsRequest = new()
            {
                UrlFile = urlFace,
                ProcessName = nameof(SaveBiometricV3CommandHandler),
                ServiceType = (int)ServiciosEnum.BIOMETRIA_FACIAL,
                Date = DateTime.UtcNow.AddHours(-5)
            };

            await _validationProcessDocumentsRepository
                .SaveValidationProcessDocumentsAsync(request.ValidationProcessId, processDocumentsRequest);
        }

        private static (object, object) CreateObjects(
            SaveBiometricV3Command request,
            string urlBiometric,
            string urlBiometricGesture
        )
        {
            object contentMultipartForm = new
            {
                guidCiu = request.CitizenGUID,
                idServicio = request.ServiceId,
                subtipo = request.SubType,
                datosAdi = request.AditionalData,
                usuario = request.User,
                actualizar = request.Update,
                codeParameter = request.CodeParameter
            };

            object contentBodyExternalRequest = new
            {
                biometria = urlBiometric,
                biometriaGesto = urlBiometricGesture,
                guidCiu = request.CitizenGUID,
                idServicio = request.ServiceId,
                subtipo = request.SubType,
                datosAdi = request.AditionalData,
                usuario = request.User,
                actualizar = request.Update,
                codeParameter = request.CodeParameter
            };

            return (contentMultipartForm, contentBodyExternalRequest);
        }
    }
}
// Fin código generado por GitHub Copilot
