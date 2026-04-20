using Microsoft.Extensions.Configuration;
using Process.Domain.Services;
using Process.Domain.Utilities;
using Process.Infrastructure.Utility;

namespace Process.Infrastructure.Services
{
    public class SigningCredentialService(IConfiguration config) : ISigningCredentialService
    {
        private readonly string keyBiometric = config.GetSection("AESKeyBiometric")?.Value ?? string.Empty;

        public async Task<string> ConsultAgreementProcess(string process)
        {
            string processToken = await GetCertificadoSigningCredentials.GetProcess(process);

            string procesoConvenioGuid = string.Empty;

            if (!string.IsNullOrEmpty(process))
            {
                string[] strings = processToken.Split('|');
                procesoConvenioGuid = strings[0].DecryptFromAESNew(keyBiometric, strings[1]);
            }

            return procesoConvenioGuid;
        }
    }
}
