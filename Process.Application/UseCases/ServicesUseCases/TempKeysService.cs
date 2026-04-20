using Process.Application.UseCases.InterfacesUseCases;
using Process.Domain.Entities;
using Process.Domain.Parameters.ExternalApiClientParameters;
using Process.Domain.Repositories;
using Process.Domain.Services;

namespace Process.Application.UseCases.ServicesUseCases
{
    public class TempKeysService(
        ITempProcessKeysRepository tempProcessKeysRepository,
        IReconoserApiService reconoserApiService
    ) : ITempKeysService
    {
        private readonly ITempProcessKeysRepository _tempProcessKeysRepository = tempProcessKeysRepository;
        private readonly IReconoserApiService _reconoserApiService = reconoserApiService;

        public async Task<TempProcessKeys> ValidateKeysAsync(Guid validationProcessGuid)
        {
            TempProcessKeys? tempProcessKeys = await _tempProcessKeysRepository
                .GetTempProcessKeysByValidationProcessGuidAsync(validationProcessGuid);

            if (tempProcessKeys == null)
            {
                TempKeysReconoserIDResponse tempKeysReconoser = await _reconoserApiService.GetTempKeysAsync(validationProcessGuid);

                if (tempKeysReconoser != null && tempKeysReconoser.PrivateKey != null && tempKeysReconoser.AlgorithmPrivateKey != null)
                {
                    tempProcessKeys = new()
                    {
                        ValidationProcessGuid = validationProcessGuid,
                        PublicKey = tempKeysReconoser.PublicKey,
                        PrivateKey = tempKeysReconoser.PrivateKey,
                        AlgorithmPrivateKey = tempKeysReconoser.AlgorithmPrivateKey,
                        AlgorithmPublicKey = tempKeysReconoser.AlgorithmPublicKey,
                        CreatedAt = DateTime.UtcNow.AddHours(-5)
                    };

                    await _tempProcessKeysRepository.SaveTempProcessKeysAsync(tempProcessKeys);
                }
            }

            return tempProcessKeys!;
        }
    }
}
