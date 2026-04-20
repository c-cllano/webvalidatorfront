using Microsoft.Extensions.Configuration;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class VerIdService(
        IExternalApiClientService externalApiClientService,
        IConfiguration config
    ) : IVerIdService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;

        public async Task<VerIdResponse> VerIdAsync(string imageOne, string imageTwo, object? contentBodyExternalRequest)
        {
            var bytesImageOne = Convert.FromBase64String(imageOne);
            var bytesImageTwo = Convert.FromBase64String(imageTwo);

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlVerId,
                ApiName = ApiName.VerIdMain,
                BodyExternalRequest = contentBodyExternalRequest,
                Files = new Dictionary<string, byte[]>
                {
                    { "front_img", bytesImageOne },
                    { "back_img", bytesImageTwo }
                },
                CustomHeaders = new Dictionary<string, string>
                {
                    { "user-name", _config.GetSection("VerIdService:UserNameVerId")?.Value! },
                    { "user-token", _config.GetSection("VerIdService:UserTokenVerId")?.Value! }
                }
            };

            VerIdResponse response = await _externalApiClientService
                .PostAsync<VerIdResponse>(externalApiClientRequest);

            return response;
        }
    }
}
