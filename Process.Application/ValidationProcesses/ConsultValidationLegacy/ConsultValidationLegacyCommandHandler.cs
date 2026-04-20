using MediatR;
using Process.Domain.Entities;
using Process.Domain.Parameters.Context;
using Process.Domain.Repositories;
using Process.Domain.Services;

namespace Process.Application.ValidationProcesses.ConsultValidationLegacy
{
    public class ConsultValidationLegacyCommandHandler(
        IValidationProcessRepository validationProcessRepository,
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IReconoserApiService reconoserApiService,
        ReconoserContext context
    ) : IRequestHandler<ConsultValidationLegacyCommand, string>
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;
        private readonly ReconoserContext _context = context;

        public async Task<string> Handle(ConsultValidationLegacyCommand request, CancellationToken cancellationToken)
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

            return await _reconoserApiService.ConsultValidationReconoserLegacyAsync(contentBody);
        }
    }
}
