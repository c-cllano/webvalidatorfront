using Process.Application.UseCases.Attributes;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Application.ValidationProcesses.StatusValidationRequest;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using static Domain.Enums.Enumerations;

namespace Process.Application.UseCases.ServicesUseCases
{
    [PlatformConnection(Constants.ReconoserID)]
    public class StatusValidationRequestReconoserService(
        IReconoserApiService reconoserApiService,
        IValidationProcessRepository validationProcessRepository,
        IConfigurationService configurationService,
        IStatusValidationRepository statusValidationRepository,
        ITempKeysService tempKeysService,
        ICitizenRepository citizenRepository,
        IDocumentTypeRepository documentTypeRepository,
        ITokenService tokenService
    ) : IStatusValidationRequestService
    {
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IConfigurationService _configurationService = configurationService;
        private readonly IStatusValidationRepository _statusValidationRepository = statusValidationRepository;
        private readonly ITempKeysService _tempKeysService = tempKeysService;
        private readonly ICitizenRepository _citizenRepository = citizenRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository = documentTypeRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<StatusValidationRequestResponse> StatusValidationRequestAsync(
            StatusValidationRequestCommand request,
            AgreementOkeyStudio agreementOkeyStudio
        )
        {
            object? citizenData = null;
            string adviser = _tokenService.GetClaim("user_name");

            if (request.CitizenData != null && !string.IsNullOrEmpty(request.CitizenData.FirstName))
            {
                citizenData = new
                {
                    primerNombre = request.CitizenData.FirstName,
                    segundoNombre = request.CitizenData.SecondName,
                    primerApellido = request.CitizenData.LastName,
                    segundoApellido = request.CitizenData.SecondLastName
                };
            }

            object contentBody = new
            {
                guidConv = agreementOkeyStudio.AgreementGUID,
                tipoValidacion = request.ValidationType,
                tipoDoc = request.DocumentType,
                numDoc = request.DocumentNumber,
                usuario = agreementOkeyStudio.UserReconoserId,
                email = request.Email,
                celular = request.Phone,
                prefCelular = request.Indicative,
                ciudadanoData = citizenData,
                procesoWhatsapp = request.RequestChannel == 1,
                asesor = adviser
            };

            StatusValidationRequestReconoserIDResponse response = await _reconoserApiService
                .StatusValidationRequestReconoserAsync(contentBody);

            if (response == null || (response != null && response.Code != 200))
            {
                throw new KeyNotFoundException("Ha ocurrido un error creando el proceso convenio en ReconoserID");
            }

            var (validationProcessId, fullName) =
                await SaveValidationProcessAsync(response!, agreementOkeyStudio, request);

            if (GetAllowKey())
            {
                await _tempKeysService.ValidateKeysAsync(response!.Data!.ProcesoConvenioGuid!.Value);
            }

            return new()
            {
                ValidationProcessId = validationProcessId,
                ValidationProcessGUID = response!.Data!.ProcesoConvenioGuid!.Value,
                Url = $"{_configurationService.GetConfiguration(RedirectUrl.RedirectValidationUrl.ToString())}/{response!.Data!.ProcesoConvenioGuid!.Value}",
                StatusProcess = response.Data.StatusProcess,
                CitizenGUID = response.Data.CitizenGUID,
                FullName = fullName
            };
        }

        private bool GetAllowKey()
        {
            var allowKeysString = _configurationService.GetConfiguration("AllowKeys");

            bool allowKeys = false;

            if (!string.IsNullOrWhiteSpace(allowKeysString) &&
                bool.TryParse(allowKeysString, out var result))
            {
                allowKeys = result;
            }

            return allowKeys;
        }

        private async Task<(long validationProcessId, string fullName)> SaveValidationProcessAsync(
            StatusValidationRequestReconoserIDResponse response,
            AgreementOkeyStudio agreementOkeyStudio,
            StatusValidationRequestCommand request
        )
        {
            ConsultAgreementProcessReconoserIDResponse agreementProcessReconoserId =
                await _reconoserApiService.ConsultAgreementProcessReconoserAsync(
                    response!.Data!.ProcesoConvenioGuid!.Value
                );

            StatusValidation? statusValidation = await _statusValidationRepository
               .GetStatusValidationByStatusCodeAsync((int)UpdateStatusCode.Process)
                   ?? throw new KeyNotFoundException($"No existe un estado de validación con StatusCode: {1}");

            await SaveOrUpdateCitizenAsync(request, agreementOkeyStudio, agreementProcessReconoserId);

            ValidationProcess validationProcess = new()
            {
                ValidationProcessGUID = response!.Data!.ProcesoConvenioGuid,
                AgreementGUID = agreementOkeyStudio.AgreementGUID,
                WorkflowId = request.WorkflowId,
                CitizenGUID = response!.Data!.CitizenGUID,
                FirstName = agreementProcessReconoserId.Data.FirstName,
                SecondName = agreementProcessReconoserId.Data.SecondName,
                LastName = agreementProcessReconoserId.Data.LastName,
                SecondLastName = agreementProcessReconoserId.Data.SecondLastName,
                InfCandidate = agreementProcessReconoserId.Data.CandidateInformation,
                DocumentTypeId = request.DocumentType,
                DocumentNumber = request.DocumentNumber,
                Email = request.Email ?? agreementProcessReconoserId.Data.Email,
                CellphoneNumber = request.Phone ?? agreementProcessReconoserId.Data.Phone,
                ProcessType = response!.Data!.StatusProcess,
                Advisor = null,
                DocumentIssuingDate = agreementProcessReconoserId.Data.ExpeditionDate >= new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    ? agreementProcessReconoserId.Data.ExpeditionDate
                    : null,
                DocumentIssuingPlace = agreementProcessReconoserId.Data.PlaceExpedition,
                OfficeCode = agreementProcessReconoserId.Data.BranchCode,
                OfficeName = agreementProcessReconoserId.Data.BranchName,
                ExecuteInMobile = agreementProcessReconoserId.Data.RunOnMobile,
                CreatorUserId = null,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                UpdatedDate = null,
                Active = true,
                IsDeleted = false,
                IsCompleted = false,
                CompletionDate = null,
                ForensicState = null,
                Validation = true,
                RequestChannel = request.RequestChannel ?? 0,
                Approved = false,
                StatusValidationId = statusValidation.StatusValidationId
            };

            validationProcess =
                await _validationProcessRepository.SaveValidationProcessAsync(validationProcess);

            string fullName = string.Join(" ",
                validationProcess.FirstName,
                validationProcess.SecondName,
                validationProcess.LastName,
                validationProcess.SecondLastName
            ).Trim();

            return (validationProcess.ValidationProcessId, fullName);
        }

        private async Task SaveOrUpdateCitizenAsync(
            StatusValidationRequestCommand request,
            AgreementOkeyStudio agreementOkeyStudio,
            ConsultAgreementProcessReconoserIDResponse agreementProcessReconoserId
        )
        {
            DocumentType? documentType = await _documentTypeRepository.GetDocumentTypeByCode(request.DocumentType)
                ?? throw new KeyNotFoundException("El tipo de documento no existe");

            Citizen? citizen = await _citizenRepository
                .GetCitizenByDocumentNumberAndAgreementIdAsync(request.DocumentNumber, agreementOkeyStudio.AgreementId);

            _ = citizen is null
                ? await SaveCitizenAsync(request, agreementOkeyStudio, documentType, agreementProcessReconoserId)
                : await UpdateCitizenAsync(citizen, request, documentType, agreementProcessReconoserId);
        }

        private async Task<Citizen> SaveCitizenAsync(
            StatusValidationRequestCommand request,
            AgreementOkeyStudio agreementOkeyStudio,
            DocumentType documentType,
            ConsultAgreementProcessReconoserIDResponse agreementProcessReconoserId
        )
        {
            var citizen = new Citizen
            {
                CitizenGuid = Guid.NewGuid(),
                AgreementId = agreementOkeyStudio.AgreementId,
                DocumentTypeId = documentType.DocumentTypeId,
                DocumentNumber = request.DocumentNumber,
                Email = request.Email ?? agreementProcessReconoserId.Data.Email,
                CountryId = request.CountryId,
                Phone = request.Phone ?? agreementProcessReconoserId.Data.Phone,
                FirstName = agreementProcessReconoserId.Data.FirstName,
                SecondName = agreementProcessReconoserId.Data.SecondName,
                LastName = agreementProcessReconoserId.Data.LastName,
                SecondLastName = agreementProcessReconoserId.Data.SecondLastName,
                DocumentIssueDate = agreementProcessReconoserId.Data.ExpeditionDate >= new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    ? agreementProcessReconoserId.Data.ExpeditionDate
                    : null,
                BirthDate = null,
                Location = agreementProcessReconoserId.Data.PlaceExpedition,
                AdviserId = null,
                BranchId = null,
                Status = true,
                AutoGeneratedValidation = false,
                QualificationProcesses = null,
                IssueDepartment = agreementProcessReconoserId.Data.PlaceExpedition,
                AccountId = null,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                UpdatedDate = null,
                Active = true
            };

            return await _citizenRepository.SaveCitizenAsync(citizen);
        }

        private async Task<Citizen> UpdateCitizenAsync(
            Citizen citizen,
            StatusValidationRequestCommand request,
            DocumentType documentType,
            ConsultAgreementProcessReconoserIDResponse agreementProcessReconoserId
        )
        {
            citizen.DocumentTypeId = documentType.DocumentTypeId;
            citizen.Email = request.Email ?? agreementProcessReconoserId.Data.Email;
            citizen.CountryId = request.CountryId;
            citizen.Phone = request.Phone ?? agreementProcessReconoserId.Data.Phone;
            citizen.FirstName = agreementProcessReconoserId.Data.FirstName;
            citizen.SecondName = agreementProcessReconoserId.Data.SecondName;
            citizen.LastName = agreementProcessReconoserId.Data.LastName;
            citizen.SecondLastName = agreementProcessReconoserId.Data.SecondLastName;
            citizen.DocumentIssueDate = agreementProcessReconoserId.Data.ExpeditionDate >= new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ? agreementProcessReconoserId.Data.ExpeditionDate
                : null;
            citizen.Location = agreementProcessReconoserId.Data.PlaceExpedition;
            citizen.UpdatedDate = DateTime.UtcNow.AddHours(-5);
            citizen.IssueDepartment = agreementProcessReconoserId.Data.PlaceExpedition;

            return await _citizenRepository.UpdateCitizenAsync(citizen);
        }
    }
}
