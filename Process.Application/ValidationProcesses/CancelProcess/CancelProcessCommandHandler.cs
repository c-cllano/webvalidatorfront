using MediatR;
using Process.Application.UseCases.Responses;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

// Inicio código generado por GitHub Copilot
namespace Process.Application.ValidationProcesses.CancelProcess
{
    public class CancelProcessCommandHandler(
        IValidationProcessRepository validationProcessRepository,
        IReconoserApiService reconoserApiService,
        IReasonRepository reasonRepository,
        IStatusValidationRepository statusValidationRepository
    ) : IRequestHandler<CancelProcessCommand, CancelProcessResponse>
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly IReasonRepository _reasonRepository = reasonRepository;
        private readonly IStatusValidationRepository _statusValidationRepository = statusValidationRepository;

        public async Task<CancelProcessResponse> Handle(CancelProcessCommand request, CancellationToken cancellationToken)
        {
            ValidationProcess? validationProcess = await _validationProcessRepository
                .GetValidationProcessByValidationProcessGuid(request.ValidationProcessGuid)
                    ?? throw new KeyNotFoundException("El proceso del convenio no fue encontrado.");

            Reason? reason = await _reasonRepository.GetReasonById(request.ReasonId)
                ?? throw new KeyNotFoundException("El motivo no fue encontrado.");

            object contentBody = new
            {
                procesoConvenioGuid = request.ValidationProcessGuid,
                asesor = request.Adviser,
                motivo = request.Reason,
                motivoId = request.ReasonId
            };

            TransactionReconoserIDResponse response = await _reconoserApiService
                .CancelProcessReconoserAsync(contentBody);

            if (response != null && response.EsExitosa == true)
            {
                validationProcess.RejectionCauseId = reason.ReasonID;
                validationProcess.IsCompleted = true;
                validationProcess.Approved = false;
                validationProcess.UpdatedDate = DateTime.UtcNow.AddHours(-5);

                var statusValidation = await _statusValidationRepository
                    .GetStatusValidationByStatusCodeAsync((int)UpdateStatusCode.Finalized);

                if (statusValidation != null)
                {
                    validationProcess.StatusValidationId = statusValidation.StatusValidationId;
                }

                await _validationProcessRepository.UpdateValidationProcessAsync(validationProcess);
            }

            return new()
            {
                IsHomologation = response!.IsHomologacion!.Value,
                IsSuccessful = response.EsExitosa!.Value,
                TransactionError = response.ErrorEntransaccion?
                    .Select(r => new ErrorTransactionResponse
                    {
                        Code = r.Codigo,
                        Description = r.Descripcion
                    })
                    .ToList() ?? []
            };
        }
    }
}
// Fin código generado por GitHub Copilot
