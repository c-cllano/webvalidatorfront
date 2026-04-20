using MediatR;
using Microsoft.Extensions.Configuration;
using Process.Application.Interfaces;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.ValueObjects;

namespace Process.Application.Atdpt.Transactions.Querys
{
    public class GetFileTransactionByIdHandler(IAtdpApiClient apiClient,
        IParametersAgreementRepository parametersAgreementRepository,
        IConfigurationService configuration
        ) : IRequestHandler<GetFileTransactionByIdQuery, AtdpTransactionFile>
    {
        private readonly IAtdpApiClient _apiClient = apiClient; 
        private readonly IParametersAgreementRepository _parametersAgreementRepository = parametersAgreementRepository;
        private readonly IConfigurationService _configuration = configuration;

        // Inicio refactorización/optimización por GitHub Copilot
        public async Task<AtdpTransactionFile> Handle(GetFileTransactionByIdQuery request, CancellationToken cancellationToken)
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
            var response = await _apiClient.GetFileTransactionByIdAsync(request.AtdpTransactionId, tokenAdtp);
            return response;
        }
        // Fin refactorización/optimización por GitHub Copilot
    }
}
