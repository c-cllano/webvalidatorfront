using MediatR;
using Process.Application.UseCases.InterfacesUseCases;
using Process.Domain.Entities;
using Process.Domain.Parameters.Context;
using Process.Domain.Repositories;
using Process.Domain.Utilities;

namespace Process.Application.ValidationProcesses.CompareFaces
{
    public class CompareFacesCommandHandler(
        IAgreementOkeyStudioRepository agreementOkeyStudioRepository,
        IValidationProcessRepository validationProcessRepository,
        IPlatformConnectionStrategyService<ICompareFacesService> platformStrategy,
        ReconoserContext context
    ) : IRequestHandler<CompareFacesCommand, CompareFacesResponse>
    {
        private readonly IAgreementOkeyStudioRepository _agreementOkeyStudioRepository = agreementOkeyStudioRepository;
        private readonly IValidationProcessRepository _validationProcessRepository = validationProcessRepository;
        private readonly IPlatformConnectionStrategyService<ICompareFacesService> _platformStrategy = platformStrategy;
        private readonly ReconoserContext _context = context;

        public async Task<CompareFacesResponse> Handle(CompareFacesCommand request, CancellationToken cancellationToken)
        {
            ValidationProcess? validationProcess = await _validationProcessRepository
                .GetValidationProcessById(request.ValidationProcessId!.Value)
                    ?? throw new KeyNotFoundException("El proceso del convenio no fue encontrado.");

            AgreementOkeyStudio? agreementOkeyStudio = await _agreementOkeyStudioRepository.GetAgreementByGuid(validationProcess.AgreementGUID!.Value)
                ?? throw new KeyNotFoundException("El convenio no fue encontrado.");

            _context.ChangeUrl = agreementOkeyStudio.ChangeUrl;
            _context.BaseUrlReconoser1 = agreementOkeyStudio.BaseUrlReconoser1;
            _context.BaseUrlReconoser2 = agreementOkeyStudio.BaseUrlReconoser2;
            _context.AgreementGUID = agreementOkeyStudio.AgreementGUID;

            string platformConnection = string.IsNullOrEmpty(agreementOkeyStudio.PlatformConnection)
                ? Constants.OkeyStudio
                : agreementOkeyStudio.PlatformConnection;

            var strategy = _platformStrategy.Resolve(platformConnection);

            return await strategy.CompareFacesAsync(request, validationProcess.AgreementGUID!.Value);
        }
    }
}
