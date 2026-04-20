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
    [PlatformConnection(Constants.OkeyStudio)]
    public class StatusValidationRequestOkeyService(
        IValidationProcessRepository validationProcessRepository,
        ICitizenRepository citizenRepository,
        IConfigurationService configurationService,
        IAniApiService aniApiService,
        IDocumentTypeRepository documentTypeRepository,
        ICountryRepository countryRepository,
        ICitizenBiometricsDocumentsRepository biometricsRepository,
        IStatusValidationRepository statusValidationRepository
    ) : IStatusValidationRequestService
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly ICitizenRepository _citizenRepository = citizenRepository;
        private readonly IConfigurationService _configurationService = configurationService;
        private readonly IAniApiService _aniApiService = aniApiService;
        private readonly IDocumentTypeRepository _documentTypeRepository = documentTypeRepository;
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly ICitizenBiometricsDocumentsRepository _biometricsRepository = biometricsRepository;
        private readonly IStatusValidationRepository _statusValidationRepository = statusValidationRepository;

        public async Task<StatusValidationRequestResponse> StatusValidationRequestAsync(
            StatusValidationRequestCommand request,
            AgreementOkeyStudio agreementOkeyStudio
        )
        {
            DocumentType? documentType = await _documentTypeRepository.GetDocumentTypeByCode(request.DocumentType)
                ?? throw new KeyNotFoundException("El tipo de documento no existe");

            if (request.CountryId is not null)
            {
                _ = await _countryRepository.GetCountryById(request.CountryId.Value)
                    ?? throw new KeyNotFoundException("El país no existe");
            }            

            Citizen? citizen = await _citizenRepository
                .GetCitizenByDocumentNumberAndAgreementIdAsync(request.DocumentNumber, request.AgreementId);

            ValidationDocumentAniResponse? validationDocumentAni = request.DocumentType.Equals(Constants.CC)
                ? await _aniApiService.ValidationDocumentAniAsync(request.DocumentNumber, "1")
                : null;

            CitizenBiometricsDocuments? biometrics = null;

            if (citizen != null)
            {
                biometrics = await _biometricsRepository
                    .GetBiometricsByCitizenIdAndServiceTypeAsync(citizen.CitizenId, (int)ServiciosEnum.BIOMETRIA_FACIAL);
            }

            int processStatus = citizen is null || biometrics is null
                ? (int)EstadoProceso.Enrolamiento
                : (int)EstadoProceso.Validacion;

            citizen = citizen is null
                ? await SaveCitizenAsync(documentType, request, validationDocumentAni!)
                : await UpdateCitizenAsync(citizen, documentType, request, validationDocumentAni!);

            ValidationProcess validationProcess = await SaveValidationProcessAsync(citizen!, agreementOkeyStudio, request, processStatus);

            return new()
            {
                ValidationProcessId = validationProcess.ValidationProcessId,
                ValidationProcessGUID = validationProcess.ValidationProcessGUID,
                Url = $"{_configurationService.GetConfiguration(RedirectUrl.RedirectValidationUrl.ToString())}/{validationProcess.ValidationProcessGUID}",
                StatusProcess = validationProcess.ProcessType,
                CitizenGUID = citizen.CitizenGuid
            };
        }

        private async Task<Citizen> SaveCitizenAsync(
            DocumentType documentType,
            StatusValidationRequestCommand request,
            ValidationDocumentAniResponse validationDocumentAni
        )
        {
            var response = validationDocumentAni?.Response;

            int documentStatus = 0;
            if (!string.IsNullOrEmpty(response?.DocumentStatus))
                _ = int.TryParse(response.DocumentStatus, out documentStatus);

            DateTime? issueDate = null;
            if (!string.IsNullOrEmpty(response?.IssueDate) && DateTime.TryParse(response.IssueDate, out var parsedDate))
                issueDate = parsedDate;

            var citizen = new Citizen
            {
                CitizenGuid = Guid.NewGuid(),
                AgreementId = request.AgreementId,
                DocumentTypeId = documentType.DocumentTypeId,
                DocumentNumber = request.DocumentNumber,
                Email = request.Email,
                CountryId = request.CountryId,
                Phone = request.Phone,
                FirstName = response?.FirstName ?? (request.CitizenData is not null
                    ? request.CitizenData.FirstName
                    : (string.IsNullOrEmpty(request.Name) ? null : request.Name)
                ),
                SecondName = response?.SecondName ?? request.CitizenData?.SecondName,
                LastName = response?.LastName ?? request.CitizenData?.LastName,
                SecondLastName = response?.SecondLastName ?? request.CitizenData?.SecondLastName,
                Alive = string.IsNullOrEmpty(response?.DateOfDeath),
                DocumentStatus = documentStatus,
                DocumentIssueDate = issueDate,
                BirthDate = null,
                Location = response?.IssuingMunicipality,
                AdviserId = null,
                BranchId = null,
                Status = true,
                AutoGeneratedValidation = false,
                QualificationProcesses = null,
                ResolutionNumber = response?.ResolutionNumber,
                Particle = response?.Particle,
                IssueDepartment = response?.IssuingDepartment,
                AccountId = null,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                UpdatedDate = null,
                Active = true
            };

            return await _citizenRepository.SaveCitizenAsync(citizen);
        }

        private async Task<Citizen> UpdateCitizenAsync(
            Citizen citizen,
            DocumentType documentType,
            StatusValidationRequestCommand request,
            ValidationDocumentAniResponse validationDocumentAni
        )
        {
            var response = validationDocumentAni?.Response;

            int documentStatus = 0;
            if (!string.IsNullOrEmpty(response?.DocumentStatus))
                _ = int.TryParse(response.DocumentStatus, out documentStatus);

            DateTime? issueDate = null;
            if (!string.IsNullOrEmpty(response?.IssueDate) && DateTime.TryParse(response.IssueDate, out var parsedDate))
                issueDate = parsedDate;

            citizen.DocumentTypeId = documentType.DocumentTypeId;
            citizen.Email = request.Email;
            citizen.CountryId = request.CountryId;
            citizen.Phone = request.Phone;
            citizen.FirstName = response?.FirstName ?? (request.CitizenData is not null
                ? request.CitizenData.FirstName
                : (string.IsNullOrEmpty(request.Name) ? null : request.Name)
            );
            citizen.SecondName = response?.SecondName ?? request.CitizenData?.SecondName;
            citizen.LastName = response?.LastName ?? request.CitizenData?.LastName;
            citizen.SecondLastName = response?.SecondLastName ?? request.CitizenData?.SecondLastName;
            citizen.Alive = string.IsNullOrEmpty(response?.DateOfDeath);
            citizen.DocumentStatus = documentStatus;
            citizen.DocumentIssueDate = issueDate;
            citizen.Location = response?.IssuingMunicipality;
            citizen.UpdatedDate = DateTime.UtcNow.AddHours(-5);
            citizen.ResolutionNumber = response?.ResolutionNumber;
            citizen.Particle = response?.Particle;
            citizen.IssueDepartment = response?.IssuingDepartment;

            return await _citizenRepository.UpdateCitizenAsync(citizen);
        }

        private async Task<ValidationProcess> SaveValidationProcessAsync(
            Citizen citizen,
            AgreementOkeyStudio agreementOkeyStudio,
            StatusValidationRequestCommand request,
            int processStatus
        )
        {
            StatusValidation? statusValidation = await _statusValidationRepository
                .GetStatusValidationByStatusCodeAsync((int)UpdateStatusCode.Process)
                    ?? throw new KeyNotFoundException($"No existe un estado de validación con StatusCode: {1}");

            ValidationProcess validationProcess = new()
            {
                ValidationProcessGUID = Guid.NewGuid(),
                AgreementGUID = agreementOkeyStudio.AgreementGUID,
                WorkflowId = request.WorkflowId,
                CitizenGUID = citizen.CitizenGuid,
                FirstName = citizen.FirstName,
                SecondName = citizen.SecondName,
                LastName = citizen.LastName,
                SecondLastName = citizen.SecondLastName,
                InfCandidate = null,
                DocumentTypeId = request.DocumentType,
                DocumentNumber = request.DocumentNumber,
                Email = citizen.Email,
                CellphoneNumber = citizen.Phone,
                ProcessType = processStatus,
                Advisor = null,
                DocumentIssuingDate = citizen.DocumentIssueDate,
                DocumentIssuingPlace = citizen.IssueDepartment,
                OfficeCode = null,
                OfficeName = null,
                ExecuteInMobile = null,
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

            return await _validationProcessRepository.SaveValidationProcessAsync(validationProcess);
        }
    }
}
