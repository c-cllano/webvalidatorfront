using MediatR;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;

namespace Process.Application.AgreementProcess.GetConsultProcess
{
    public class GetConsultProcessHandler(
        IValidationProcessRepository validationProcessRepository,
        IValidationProcessExecutionRepository validationProcessExecutionRepository,
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IReconoserApiService reconoserService
    ) : IRequestHandler<GetConsultProcessQuery, GetConsultProcessResponse>
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IValidationProcessExecutionRepository _validationProcessExecutionRepository = validationProcessExecutionRepository;
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IReconoserApiService _reconoserService = reconoserService;

        public async Task<GetConsultProcessResponse> Handle(GetConsultProcessQuery request, CancellationToken cancellationToken)
        {
            ValidationProcess? validationProcess = await _validationProcessRepository
                .GetValidationProcessByValidationProcessGuid(request.ProcessAgreement)
                    ?? throw new KeyNotFoundException("El proceso convenio no existe");

            GetConsultProcessResponse response = MapToResponseWithValidationProcess(validationProcess);

            ValidationProcessExecution? validationProcessExecution = await _validationProcessExecutionRepository
                .GetValidationProcessExecutionByValidationProcessIdAsync(response.ProcessData.ValidationProcessId)
                    ?? await SaveValidationProcessExecutionAsync(response.ProcessData.ValidationProcessId);

            AgreementOkeyStudio? agreement = await _agreementOkeyStudioRepository.GetAgreementByGuid(response.ProcessData.AgreementGuid)
                ?? throw new KeyNotFoundException("El convenio no existe");

            response.ProcessData.ValidationProcessExecutionId = validationProcessExecution.ValidationProcessExecutionId;
            response.ProcessData.LastStep = validationProcessExecution.LastStep;
            response.ProcessData.UserReconoserId = agreement.UserReconoserId;

            await GetAgreementProcessAsync(response);

            return response;
        }

        private async Task GetAgreementProcessAsync(
            GetConsultProcessResponse response
        )
        {
            ConsultAgreementProcessReconoserIDResponse agreementProcess = await _reconoserService
                .ConsultAgreementProcessReconoserAsync(response.ProcessData.AgreementProcessGuid);

            if (agreementProcess != null)
            {
                response.ProcessData.ClientCode = agreementProcess.Data.ClientCode!;
                response.TransactionResponse = agreementProcess.TransactionResponse;
            }
            else
            {
                response.TransactionResponse = new()
                {
                    IsHomologation = false,
                    IsSucessfull = true,
                    TransactionError = []
                };
            }
        }

        private static GetConsultProcessResponse MapToResponseWithValidationProcess(ValidationProcess validationProcess)
        {
            var processData = new ProcessData
            {
                AgreementGuid = validationProcess.AgreementGUID ?? Guid.Empty,
                CitizenGuid = validationProcess.CitizenGUID ?? Guid.Empty,
                FirstName = validationProcess.FirstName!,
                MiddleName = validationProcess.SecondName!,
                LastName = validationProcess.LastName!,
                SecondLastName = validationProcess.SecondLastName!,
                AgreementProcessGuid = validationProcess.ValidationProcessGUID ?? Guid.Empty,
                Advisor = validationProcess.Advisor,
                BranchCode = validationProcess.OfficeCode,
                BranchName = validationProcess.OfficeName,
                ProcessState = validationProcess.ProcessType,
                ExecuteOnMobile = validationProcess.ExecuteInMobile,
                DocumentType = validationProcess.DocumentTypeId!,
                DocumentNumber = validationProcess.DocumentNumber!,
                Email = validationProcess.Email!,
                Mobile = validationProcess.CellphoneNumber!,
                CandidateInfo = validationProcess.InfCandidate!,
                RegistrationDate = validationProcess.CreatedDate,
                IsCompleted = validationProcess.IsCompleted,
                CompletionDate = validationProcess.CompletionDate,
                ForensicState = validationProcess.ForensicState,
                IssueDate = validationProcess.DocumentIssuingDate,
                IssuePlace = validationProcess.DocumentIssuingPlace!,
                Validation = validationProcess.Validation,
                ValidationProcessId = validationProcess.ValidationProcessId,
                WorkflowId = validationProcess.WorkflowId
            };

            return new GetConsultProcessResponse
            {
                ProcessData = processData
            };
        }

        private async Task<ValidationProcessExecution> SaveValidationProcessExecutionAsync(long validationProcessId)
        {
            ValidationProcessExecution validationProcessExecution = new()
            {
                ValidationProcessId = validationProcessId,
                StartDate = DateTime.UtcNow.AddHours(-5),
                FinishDate = DateTime.UtcNow.AddHours(-5),
                LastStep = null,
                Trazability = null,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                UpdatedDate = null,
                Active = true,
                IsDeleted = false
            };

            return await _validationProcessExecutionRepository
                .SaveValidationProcessExecutionAsync(validationProcessExecution);
        }
    }
}
