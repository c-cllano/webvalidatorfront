using MediatR;
using Process.Application.Interfaces;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.ValueObjects;

namespace Process.Application.Atdpt.Transactions
{
    public class SaveTransactionHandler(IAtdpApiClient apiClient,
        IParametersAgreementRepository parametersAgreementRepository,
        IConfigurationService configuration
        ) : IRequestHandler<SaveTransactionCommand, AtdpTransactionSave>
    {
        private readonly IAtdpApiClient _apiClient = apiClient;
        private readonly IParametersAgreementRepository _parametersAgreementRepository = parametersAgreementRepository;
        private readonly IConfigurationService _configuration = configuration;

        // Inicio refactorización/optimización por GitHub Copilot
        public async Task<AtdpTransactionSave> Handle(SaveTransactionCommand request, CancellationToken cancellationToken)
        {

            List<string>? parameterCode = ["TOKEN_ADTP"];
            string tokenAdtp;
            var parameters = await _parametersAgreementRepository
                .GetParametersAgreementByAgreementGuidAsync(request.AgreementGuid, parameterCode);

            if (parameters is null || !parameters.Any())
            {
                tokenAdtp = _configuration.GetConfiguration("ExternalApi:TokenAdtp");
            }
            else
            {
                tokenAdtp = parameters.FirstOrDefault()?.ParameterValue ?? _configuration.GetConfiguration("ExternalApi:TokenAdtp");
            }
            var response = await _apiClient.SaveTransactionAsync(request.Request, tokenAdtp);
            return response;
        }
        // Fin refactorización/optimización por GitHub Copilot
    }

}
