using MediatR;
using Process.Application.Interfaces;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.ValueObjects;

namespace Process.Application.Atdpt.Version
{
    public class GetFileVersionByIdHandler(IAtdpApiClient apiClient,
        IParametersAgreementRepository parametersAgreementRepository,
        IConfigurationService configuration
        ) : IRequestHandler<GetFileVersionByIdQuery, AtdpVersionFile>
    {
        private readonly IAtdpApiClient _apiClient = apiClient;
        private readonly IParametersAgreementRepository _parametersAgreementRepository = parametersAgreementRepository;
        private readonly IConfigurationService _configuration = configuration;

        // Inicio refactorización/optimización por GitHub Copilot
        public async Task<AtdpVersionFile> Handle(GetFileVersionByIdQuery request, CancellationToken cancellationToken)
        {
            List<string>? parameterCode = ["TOKEN_ADTP"];
            string tokenAdtp;
            var parameters = await _parametersAgreementRepository
                .GetParametersAgreementByAgreementGuidAsync(request.agreementGuid, parameterCode);
            if (parameters is null || !parameters.Any())
            {
                tokenAdtp = _configuration.GetConfiguration("ExternalApi:TokenAdtp");
            }
            else
            {
                tokenAdtp = parameters.FirstOrDefault()?.ParameterValue ?? _configuration.GetConfiguration("ExternalApi:TokenAdtp");
            }
            var atdpVersionFile = await _apiClient.GetFileVersionByIDAsync(request.AtdpId, tokenAdtp);
            return atdpVersionFile;
        }
        // Fin refactorización/optimización por GitHub Copilot

    }

}
