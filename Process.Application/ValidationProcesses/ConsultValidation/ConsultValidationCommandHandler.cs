using MediatR;
using Process.Domain.Entities;
using Process.Domain.Parameters.Context;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Application.ValidationProcesses.ConsultValidation
{
    public class ConsultValidationCommandHandler(
        IValidationProcessRepository validationProcessRepository,
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IReconoserApiService reconoserApiService,
        IReasonRepository reasonRepository,
        IForensicReviewProcessRepository forensicReviewProcessRepository,
        IStatusForensicRepository statusForensicRepository,
        ReconoserContext context,
        IStatusValidationRepository statusValidationRepository
    ) : IRequestHandler<ConsultValidationCommand, ConsultValidationResponse>
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IReasonRepository _reasonRepository = reasonRepository;
        private readonly IForensicReviewProcessRepository _forensicReviewProcessRepository = forensicReviewProcessRepository;
        private readonly IStatusForensicRepository _statusForensicRepository = statusForensicRepository;
        private readonly ReconoserContext _context = context;
        private readonly IStatusValidationRepository _statusValidationRepository = statusValidationRepository;

        private readonly Dictionary<int, string> ForensicStatesValues = new()
        {
            {1, "En revisión"},
            {2, "Revisado"}
        };

        public async Task<ConsultValidationResponse> Handle(ConsultValidationCommand request, CancellationToken cancellationToken)
        {
            ValidationProcess? validationProcess = await _validationProcessRepository
                .GetValidationProcessByValidationProcessGuid(request.ValidationProcessGUID)
                    ?? throw new KeyNotFoundException("El proceso del convenio no fue encontrado.");

            AgreementOkeyStudio? agreementOkeyStudio = await _agreementOkeyStudioRepository.GetAgreementByGuid(validationProcess.AgreementGUID!.Value)
                ?? throw new KeyNotFoundException("El convenio no fue encontrado.");

            if (string.IsNullOrEmpty(agreementOkeyStudio.UserReconoserId) || string.IsNullOrEmpty(agreementOkeyStudio.PasswordReconoserId))
            {
                throw new KeyNotFoundException("El convenio no tiene asignado user o password de ReconoserID");
            }

            object contentBody = new
            {
                guidConv = agreementOkeyStudio.AgreementGUID,
                procesoConvenioGuid = validationProcess.ValidationProcessGUID,
                usuario = agreementOkeyStudio.UserReconoserId,
                clave = agreementOkeyStudio.PasswordReconoserId
            };

            _context.ChangeUrl = agreementOkeyStudio.ChangeUrl;
            _context.BaseUrlReconoser1 = agreementOkeyStudio.BaseUrlReconoser1;
            _context.BaseUrlReconoser2 = agreementOkeyStudio.BaseUrlReconoser2;
            _context.AgreementGUID = agreementOkeyStudio.AgreementGUID;

            ConsultValidationReconoserIDResponse consultValidationReconoser = await _reconoserApiService.ConsultValidationReconoserAsync(contentBody);

            if (consultValidationReconoser != null)
            {
                validationProcess.Approved = consultValidationReconoser.Data!.Approved;
                validationProcess.ProcessType = consultValidationReconoser.Data!.ProcessStatus;
                validationProcess.UpdatedDate = DateTime.UtcNow.AddHours(-5);
                validationProcess.IsCompleted = consultValidationReconoser.Data!.Finalized;
                validationProcess.CompletionDate = consultValidationReconoser.Data!.FinalizedDate;
                validationProcess.ForensicState = consultValidationReconoser.Data!.ForensicState;
                validationProcess.ForensicReason = consultValidationReconoser.Data!.ForensicReason;
                validationProcess.ForensicOptionalReason = consultValidationReconoser.Data!.ForensicOptionalReason;
                validationProcess.ForensicObservations = consultValidationReconoser.Data!.ForensicObservations;

                if (consultValidationReconoser.Data!.ReasonId != null && consultValidationReconoser.Data!.ReasonId > 0)
                {
                    var reason = await _reasonRepository.GetReasonById(consultValidationReconoser.Data!.ReasonId.Value);

                    if (reason != null)
                    {
                        validationProcess.RejectionCauseId = reason.ReasonID;
                    }
                }

                await SetStatusAsync(consultValidationReconoser, validationProcess);

                await _validationProcessRepository.UpdateValidationProcessAsync(validationProcess);

                await ValidateForensicReviewProcessAsync(consultValidationReconoser, validationProcess);
            }

            return new()
            {
                AgreementGUID = agreementOkeyStudio.AgreementGUID,
                ValidationProcessGUID = validationProcess.ValidationProcessGUID!.Value,
                CitizenGUID = validationProcess.CitizenGUID!.Value,
                FirstName = validationProcess.FirstName,
                SecondName = validationProcess.SecondName,
                LastName = validationProcess.LastName,
                SecondLastName = validationProcess.SecondLastName,
                DocumentType = validationProcess.DocumentTypeId,
                DocumentNumber = validationProcess.DocumentNumber,
                Email = validationProcess.Email,
                DateOfIssue = validationProcess.DocumentIssuingDate,
                PlaceOfIssue = validationProcess.DocumentIssuingPlace,
                ProcessStatus = validationProcess.ProcessType,
                Approved = validationProcess.Approved,
                ForensicState = validationProcess.ForensicState,
                ForensicReason = validationProcess.ForensicReason,
                ForensicOptionalReason = validationProcess.ForensicOptionalReason,
                ForensicObservations = validationProcess.ForensicObservations,
                IsCompleted = validationProcess.IsCompleted,
                CompletionDate = validationProcess.CompletionDate,
                CreatedDate = validationProcess.CreatedDate,
                RejectionCauseId = validationProcess.RejectionCauseId,
                RejectionCauseDescription = consultValidationReconoser?.Data?.StatusDescription
            };
        }


        private async Task SetStatusAsync(
    ConsultValidationReconoserIDResponse consultValidationReconoser,
    ValidationProcess validationProcess
)
        {
            if (validationProcess == null)
                return;

            int statusCode = DetermineStatusCode(consultValidationReconoser, validationProcess);

            var statusValidation = await _statusValidationRepository
                .GetStatusValidationByStatusCodeAsync(statusCode);

            if (statusValidation != null)
            {
                validationProcess.StatusValidationId = statusValidation.StatusValidationId;
            }
        }

        private static int DetermineStatusCode(
            ConsultValidationReconoserIDResponse consultValidationReconoser,
            ValidationProcess validationProcess
        )
        {
            // Caso: estado forense presente
            if (consultValidationReconoser?.Data?.ForensicState != null)
            {
                validationProcess.Active = true;
                return (int)UpdateStatusCode.Validation;
            }
            if (validationProcess?.IsCompleted == true)
            {
                // Caso: proceso rechazado y aprobado
                if (validationProcess.RejectionCauseId != null)
                {
                    validationProcess.Active = true;
                    validationProcess.Approved = validationProcess.RejectionCauseId == 1;
                    return (int)UpdateStatusCode.Finalized;
                }
            }
            return (int)UpdateStatusCode.None;
        }







        private async Task ValidateForensicReviewProcessAsync(
            ConsultValidationReconoserIDResponse consultValidationReconoser,
            ValidationProcess validationProcess
        )
        {
            if (consultValidationReconoser.Data!.ForensicState == null)
            {
                return;
            }

            if (!ForensicStatesValues.TryGetValue(consultValidationReconoser.Data!.ForensicState!.Value, out string? stateName))
            {
                throw new KeyNotFoundException($"No existe clave {consultValidationReconoser.Data!.ForensicState} para obtener el nombre del estado");
            }

            StatusForensic? statusForensic = await _statusForensicRepository
                .GetStatusByDescriptionAsync(stateName)
                    ?? throw new KeyNotFoundException($"El estado forense con descripción {stateName} no fue encontrado");

            ForensicReviewProcess? forensicReviewProcess = await _forensicReviewProcessRepository
                .GetForensicReviewProcessByValidationProcessIdAsync(validationProcess.ValidationProcessId);

            if (forensicReviewProcess == null)
            {
                forensicReviewProcess = new()
                {
                    ValidationProcessId = validationProcess.ValidationProcessId,
                    StatusForensicId = statusForensic.StatusForensicId,
                    MainReason = consultValidationReconoser.Data!.ForensicReason,
                    OptionalReason = consultValidationReconoser.Data!.ForensicOptionalReason,
                    Observation = consultValidationReconoser.Data!.ForensicObservations,
                    CreatedDate = DateTime.UtcNow.AddHours(-5),
                    Active = true,
                    IsDeleted = false
                };

                await _forensicReviewProcessRepository.SaveForensicReviewProcessAsync(forensicReviewProcess);
            }
            else
            {
                forensicReviewProcess.StatusForensicId = statusForensic.StatusForensicId;
                forensicReviewProcess.MainReason = consultValidationReconoser.Data!.ForensicReason;
                forensicReviewProcess.OptionalReason = consultValidationReconoser.Data!.ForensicOptionalReason;
                forensicReviewProcess.Observation = consultValidationReconoser.Data!.ForensicObservations;
                forensicReviewProcess.UpdatedDate = DateTime.UtcNow.AddHours(-5);

                await _forensicReviewProcessRepository.UpdateForensicReviewProcessAsync(forensicReviewProcess);
            }
        }
    }
}
