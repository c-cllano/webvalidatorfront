using Microsoft.Extensions.Configuration;
using Process.Application.Interfaces;
using Process.Domain.Parameters.Atdpt;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using Process.Domain.ValueObjects;
using Process.Infrastructure.Clients.ATDP.Responses;
using Process.Infrastructure.Extensions;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class RefitAtdpApiClient(
        IExternalApiClientService externalApiClientService,
        IConfiguration config
    ) : IAtdpApiClient
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;

        public async Task<AtdpVersionFile> GetFileVersionByIDAsync(int atdpID, string tokenAdtp)
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlAdtp,
                ApiName = ApiName.ATDPGetFileByID,
                Token = tokenAdtp,
                QueryParams = new Dictionary<string, string> { { "atdpID", $"{atdpID}" } }
            };

            AtdpFileVersionByIDResponse response = await _externalApiClientService
                .GetAsync<AtdpFileVersionByIDResponse>(externalApiClientRequest);

            return new AtdpVersionFile(
                response.AtdpVersionID,
                response.Version!,
                response.File!.ToHtmlString(),
                response.SAS
            );
        }

        public async Task<AtdpTransactionFile> GetFileTransactionByIdAsync(int atdpTransactionID, string tokenAdtp)
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlAdtp,
                ApiName = ApiName.ATDPTransactionGetFileByID,
                Token = tokenAdtp,
                QueryParams = new Dictionary<string, string> { { "atdpTransactionID", $"{atdpTransactionID}" } }
            };

            AtdpTransactionFileResponse response = await _externalApiClientService
                .GetAsync<AtdpTransactionFileResponse>(externalApiClientRequest);

            return new AtdpTransactionFile(
                response.AtdpTransactionID,
                response.DocumentTypeID,
                response.DocumentNumber!,
                response.IsApproved,
                response.Date,
                response.File!,
                response.Sas!
            );
        }

        public async Task<AtdpTransactionSave> SaveTransactionAsync(SaveTransactionRequest request, string tokenAdtp)
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlAdtp,
                ApiName = ApiName.ATDPTransactionSave,
                Body = request,
                Token = tokenAdtp
            };

            AtdpTransactionSaveResponse response = await _externalApiClientService
                .PostAsync<AtdpTransactionSaveResponse>(externalApiClientRequest);

            return new AtdpTransactionSave(response.AtdpTransactionID);
        }

    }
}
