using MediatR;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Domain.Entities;
using Process.Domain.Parameters.Context;
using Process.Domain.Repositories;
using Process.Domain.Utilities;

namespace Process.Application.ValidationProcesses.SaveDocumentBothSides
{
    public class SaveDocumentBothSidesCommandHandler(
        IValidationProcessRepository validationProcessRepository,
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IPlatformConnectionStrategyService<ISaveDocumentBothSidesService> platformStrategy,
        ReconoserContext context
    ) : IRequestHandler<SaveDocumentBothSidesCommand, SaveDocumentBothSidesResponse>
    {
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IPlatformConnectionStrategyService<ISaveDocumentBothSidesService> _platformStrategy = platformStrategy;
        private readonly ReconoserContext _context = context;

        public async Task<SaveDocumentBothSidesResponse> Handle(SaveDocumentBothSidesCommand request, CancellationToken cancellationToken)
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

            string platformConnection = string.IsNullOrEmpty(agreementOkeyStudio.PlatformConnection)
                ? Constants.OkeyStudio
                : agreementOkeyStudio.PlatformConnection;

            var strategy = _platformStrategy.Resolve(platformConnection);

            return await strategy.SaveDocumentBothSidesAsync(request, validationProcess);
        }
    }
}
