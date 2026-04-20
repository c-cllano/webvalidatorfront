using Process.Application.UseCases.InterfacesUseCases;
using Process.Domain.Entities;
using Process.Domain.Exceptions;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;

namespace Process.Application.UseCases.ServicesUseCases
{
    public class ExtractImageService(
        IParametersAgreementRepository parametersAgreementRepository,
        ITempProcessKeysRepository tempProcessKeysRepository,
        IEncryptionKeyProviderService encryptionKeyProviderService,
        IConfigurationService configuration
    ) : IExtractImageService
    {
        private readonly IParametersAgreementRepository _parametersAgreementRepository = parametersAgreementRepository;
        private readonly ITempProcessKeysRepository _tempProcessKeysRepository = tempProcessKeysRepository;
        private readonly IEncryptionKeyProviderService _encryptionKeyProviderService = encryptionKeyProviderService;
        private readonly IConfigurationService _configuration = configuration;

        public async Task<string> ExtractImageAsync(
            Guid agreementGuid,
            Guid validationProcessGuid,
            string image
        )
        {
            var parameters = await _parametersAgreementRepository
                .GetParametersAgreementByAgreementGuidAsync(agreementGuid);

            if (parameters is null || !parameters.Any())
            {
                return image;
            }

            var parameterSecureChannel = parameters
                .FirstOrDefault(p => p.ParameterName.Equals(Constants.SecureChannelEn) || p.ParameterName.Equals(Constants.SecureChannelEs));

            if (parameterSecureChannel == null || Convert.ToBoolean(parameterSecureChannel.ParameterValue).Equals(false))
            {
                return image;
            }

            TempProcessKeys? tempProcessKeys = await _tempProcessKeysRepository
                .GetTempProcessKeysByValidationProcessGuidAsync(validationProcessGuid)
                    ?? throw new BusinessException($"No fue posible procesar la imagen enviada. Por favor, intenta nuevamente o contacta a soporte si el problema persiste.", 500);

            var responseImage = ImageHelper.GetImage(image)
                ?? throw new BusinessException("Error validando imagen (dsc).", 500);

            string aeskeyBiometric = _configuration.GetConfiguration("AESKeyBiometric");
            string privateKey = EncryptionHelper.DecryptFromAESNew(tempProcessKeys.PrivateKey, aeskeyBiometric, tempProcessKeys.AlgorithmPrivateKey);

            byte[] derivate = await _encryptionKeyProviderService.GenerateDerivateAsync(privateKey, responseImage[0]);

            string decrypImage = await _encryptionKeyProviderService
                .DecrypImgAsync(responseImage[4], derivate, responseImage[2], responseImage[3], responseImage[1]);

            return decrypImage;
        }
    }
}
