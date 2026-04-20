using Microsoft.Extensions.Configuration;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class IBetaApiService(
        IExternalApiClientService externalApiClientService,
        IConfiguration config
    ) : IIBetaApiService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;
        private readonly IConfiguration _config = config;

        public async Task<Ibeta1Response> Ibeta1Async(string image, object? contentBodyExternalRequest)
        {
            var bytesImage = Convert.FromBase64String(image);

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlIbeta1,
                ApiName = ApiName.Ibeta1Main,
                BodyExternalRequest = contentBodyExternalRequest,
                Files = new Dictionary<string, byte[]>
                {
                    { "file", bytesImage }
                },
                CustomHeaders = new Dictionary<string, string>
                {
                    { "user-name", _config.GetSection("IbetaService:UserNameIbeta1")?.Value! },
                    { "user-token", _config.GetSection("IbetaService:UserTokenIbeta1")?.Value! }
                }
            };

            Ibeta1Response response = await _externalApiClientService
                .PostAsync<Ibeta1Response>(externalApiClientRequest);

            return response;
        }

        public async Task<Ibeta2Response> Ibeta2Async(
            string imageOne,
            string imageTwo,
            object? contentBodyExternalRequest
        )
        {
            var bytesImageOne = Convert.FromBase64String(imageOne);
            var bytesImageTwo = Convert.FromBase64String(imageTwo);

            ExternalApiClientRequest externalApiClientRequest = new()
            {
                Section = ApiName.ExternalApi,
                BaseUrl = ApiName.BaseUrlIbeta2,
                ApiName = ApiName.Ibeta2Main,
                BodyExternalRequest = contentBodyExternalRequest,
                Files = new Dictionary<string, byte[]>
                {
                    { "near_img", bytesImageOne },
                    { "far_img", bytesImageTwo }
                },
                CustomHeaders = new Dictionary<string, string>
                {
                    { "user-name", _config.GetSection("IbetaService:UserNameIbeta2")?.Value! },
                    { "user-token", _config.GetSection("IbetaService:UserTokenIbeta2")?.Value! }
                }
            };

            Ibeta2Response response = await _externalApiClientService
                .PostAsync<Ibeta2Response>(externalApiClientRequest);

            return response;
        }
    }
}
