using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Services;
using static Domain.Enums.Enumerations;

namespace Process.Infrastructure.Services
{
    public class CompareFacesMegviApiService(
        IExternalApiClientService externalApiClientService
    ) : ICompareFacesMegviApiService
    {
        private readonly IExternalApiClientService _externalApiClientService = externalApiClientService;

        public async Task<FaceCompareMegviResponse> CompareFacesMegviApiAsync(
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
                BaseUrl = ApiName.BaseUrlMegvi,
                ApiName = ApiName.CompareFaceMegvi,
                BodyExternalRequest = contentBodyExternalRequest,
                Files = new Dictionary<string, byte[]>
                {
                    { "image_best", bytesImageOne },
                    { "image_idcard", bytesImageTwo }
                }
            };

            FaceCompareMegviResponse response = await _externalApiClientService
                .PostAsync<FaceCompareMegviResponse>(externalApiClientRequest);

            return response;
        }
    }
}
