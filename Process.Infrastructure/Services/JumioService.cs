using Microsoft.Extensions.Configuration;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class JumioService(
        IExternalApiClientService externalApiClientService,
        IConfiguration config
    ) : IJumioService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;

        public async Task<TokenJumioResponse> GetTokenJumioAsync()
        {
            object contentBody = new
            {
                client_id = _config.GetSection("JumioService:ClientId")?.Value!,
                client_secret = _config.GetSection("JumioService:ClientSecret")?.Value!,
                grant_type = _config.GetSection("JumioService:GrantType")?.Value!
            };

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlTokenJumio,
                ApiName = ApiName.GetTokenJumio,
                IsFormUrlEncoded = true,
                Body = contentBody
            };

            TokenJumioResponse tokenExternalResponse = await _externalApiClientService
                .PostAsync<TokenJumioResponse>(externalApiClientRequest);

            return tokenExternalResponse;
        }

        public async Task<ClientAccountJumioResponse> CreateClientJumioAsync(string token)
        {
            object contentBody = GetObjectCreateClientJumio();

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlAccountJumio,
                ApiName = ApiName.CreateAccountJumio,
                Body = contentBody,
                Token = token,
                CustomHeaders = new Dictionary<string, string>
                {
                    { "User-Agent", _config.GetSection("JumioService:CustomerInternalReference")?.Value! }
                }
            };

            ClientAccountJumioResponse response = await _externalApiClientService
                .PostAsync<ClientAccountJumioResponse>(externalApiClientRequest);

            return response;
        }

        public async Task ExecuteUrlsFrontBackAsync(string token, string url, string base64)
        {
            var bytesImage = Convert.FromBase64String(base64);

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Url = url,
                Token = token,
                Files = new Dictionary<string, byte[]>
                {
                    { "file", bytesImage }
                },
                CustomHeaders = new Dictionary<string, string>
                {
                    { "User-Agent", _config.GetSection("JumioService:CustomerInternalReference")?.Value! }
                }
            };

            await _externalApiClientService.PostAsync<object>(externalApiClientRequest);
        }

        public async Task<Guid> ExecuteUrlFinalizedProcessAsync(string token, string url)
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Url = url,
                Token = token,
                CustomHeaders = new Dictionary<string, string>
                {
                    { "User-Agent", _config.GetSection("JumioService:CustomerInternalReference")?.Value! }
                }
            };

            FinalizedProcessJumioResponse result = await _externalApiClientService
                .PutAsync<FinalizedProcessJumioResponse>(externalApiClientRequest);

            return result.WorkflowExecution.Id;
        }

        public async Task ExecuteDeleteUserInfoOnJumio(string token, Guid workflowExecutionId)
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Url = $"{_config.GetSection("JumioService:UrlWorkFlow")?.Value!}{workflowExecutionId}",
                Token = token,
                CustomHeaders = new Dictionary<string, string>
                {
                    { "User-Agent", _config.GetSection("JumioService:CustomerInternalReference")?.Value! }
                }
            };

            await _externalApiClientService.DeleteAsync<object>(externalApiClientRequest);
        }

        public async Task<GetProcessJumioResponse> GetResultJumioProcessAsync(string token, Guid workflowExecutionId)
        {
            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Url = $"{_config.GetSection("JumioService:UrlWorkFlow")?.Value!}{workflowExecutionId}",
                Token = token,
                CustomHeaders = new Dictionary<string, string>
                {
                    { "User-Agent", _config.GetSection("JumioService:CustomerInternalReference")?.Value! }
                }
            };

            GetProcessJumioResponse result = await _externalApiClientService
                .GetAsync<GetProcessJumioResponse>(externalApiClientRequest);

            if (result.Workflow.Status != "PROCESSED")
            {
                await Task.Delay(3000);
                return await GetResultJumioProcessAsync(token, workflowExecutionId);
            }

            return result;
        }

        private object GetObjectCreateClientJumio()
        {
            object workflowDefinition = new
            {
                key = _config.GetSection("JumioService:WorkflowDefinitionKey")?.Value!
            };

            object userLocation = new
            {
                country = "COL"
            };

            object consent = new
            {
                obtained = "yes",
                obtainedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            };

            object userConsent = new
            {
                userIp = _config.GetSection("JumioService:UserIp")?.Value!,
                userLocation,
                consent
            };

            object contentBody = new
            {
                customerInternalReference = _config.GetSection("JumioService:CustomerInternalReference")?.Value!,
                workflowDefinition,
                userConsent
            };

            return contentBody;
        }
    }
}
