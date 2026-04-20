using MediatR;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Domain.Entities;
using Process.Domain.Parameters.Context;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;

namespace Process.Application.ValidationProcesses.StatusValidationRequest
{
    public class StatusValidationRequestCommandHandler(
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IPlatformConnectionStrategyService<IStatusValidationRequestService> platformStrategy,
        IWhatsappService whatsappService,
        ReconoserContext context
    ) : IRequestHandler<StatusValidationRequestCommand, StatusValidationRequestResponse>
    {
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IPlatformConnectionStrategyService<IStatusValidationRequestService> _platformStrategy = platformStrategy;
        private readonly IWhatsappService _whatsappService = whatsappService;
        private readonly ReconoserContext _context = context;

        public async Task<StatusValidationRequestResponse> Handle(StatusValidationRequestCommand request, CancellationToken cancellationToken)
        {
            bool isWhatsapp = request.IsWhatsapp ?? false;

            AgreementOkeyStudio? agreementOkeyStudio = await _agreementOkeyStudioRepository.GetAgreementById(request.AgreementId)
                ?? throw new KeyNotFoundException("El convenio no fue encontrado.");

            if (!isWhatsapp)
            {
                _context.ChangeUrl = agreementOkeyStudio.ChangeUrl;
                _context.BaseUrlReconoser1 = agreementOkeyStudio.BaseUrlReconoser1;
                _context.BaseUrlReconoser2 = agreementOkeyStudio.BaseUrlReconoser2;
                _context.AgreementGUID = agreementOkeyStudio.AgreementGUID;

                string platformConnection = string.IsNullOrEmpty(agreementOkeyStudio.PlatformConnection)
                    ? Constants.OkeyStudio
                    : agreementOkeyStudio.PlatformConnection;

                var strategy = _platformStrategy.Resolve(platformConnection);

                return await strategy.StatusValidationRequestAsync(request, agreementOkeyStudio);
            }
            else
            {
                if (string.IsNullOrEmpty(request.Phone))
                    throw new KeyNotFoundException("Se debe ingresar número de celular");

                object contentBody = new
                {
                    phone = request.Phone,
                    agreementId = agreementOkeyStudio.AgreementId,
                    workflowId = request.WorkflowId,
                    documentType = request.DocumentType,
                    documentNumber = request.DocumentNumber
                };

                await _whatsappService.SendRequestWhatsappAsync(contentBody);

                return new()
                {
                    ValidationProcessId = null,
                    ValidationProcessGUID = null,
                    Url = string.Empty,
                    StatusProcess = null,
                    CitizenGUID = null,
                    FullName = null
                };
            }
        }
    }
}
