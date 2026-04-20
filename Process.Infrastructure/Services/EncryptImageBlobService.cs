using Microsoft.Extensions.Configuration;
using Process.Domain.Services;
using Process.Domain.Utilities;
using System.Text;

namespace Process.Infrastructure.Services
{
    public class EncryptImageBlobService(
        IConfiguration config
    ) : IEncryptImageBlobService
    {
        private readonly IConfiguration _config = config;

        public async Task<string> EncryptImageAsync(
            string fileBase64
        )
        {
            var allowEncryptBlob = _config.GetSection("AllowEncryptBlob").Value;
            var aeskeyBiometric = _config.GetSection("AESKeyBiometric").Value;

            if (string.IsNullOrEmpty(allowEncryptBlob) || Convert.ToBoolean(allowEncryptBlob).Equals(false))
                return fileBase64;

            var (encryptedBase64, iv) = EncryptionHelper.EncryptToAESNew(fileBase64, aeskeyBiometric!);

            string payloadEncrypted = $"{encryptedBase64}::{iv}";

            string payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadEncrypted));

            return payloadBase64;
        }

        public async Task<string> DecryptImageAsync(string payload)
        {
            var allowEncryptBlob = _config.GetSection("AllowEncryptBlob").Value;
            var aesKeyBiometric = _config.GetSection("AESKeyBiometric").Value;

            if (string.IsNullOrEmpty(allowEncryptBlob) || Convert.ToBoolean(allowEncryptBlob).Equals(false))
                return payload;

            try
            {
                byte[] decodedBytes;
                try
                {
                    decodedBytes = Convert.FromBase64String(payload);
                }
                catch (FormatException)
                {
                    return payload;
                }

                string decoded = Encoding.UTF8.GetString(decodedBytes);

                var separatorIndex = decoded.LastIndexOf("::");

                if (separatorIndex < 0)
                    return payload;

                string encryptedBase64 = decoded[..separatorIndex];
                string iv = decoded[(separatorIndex + 2)..];

                return EncryptionHelper.DecryptFromAESNew(encryptedBase64, aesKeyBiometric!, iv);
            }
            catch
            {
                return payload;
            }
        }
    }
}
