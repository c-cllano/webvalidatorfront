using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Process.Domain.Exceptions;
using Process.Domain.Services;
using Process.Domain.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace Process.Infrastructure.Services
{
    public class EncryptionKeyProviderService(
        IConfiguration config
    ) : IEncryptionKeyProviderService
    {
        private readonly IConfiguration _config = config;
        private const string CURVE_NAME = "prime256v1";

        public async Task<(string publicKey, (Tuple<string, string>, Tuple<string, string>) encripted)> GenerateKeysAsync()
        {
            var aeskeyBiometric = _config.GetSection("AESKeyBiometric").Value;

            X9ECParameters ecParams = ECNamedCurveTable.GetByName(CURVE_NAME);
            var n = new ECDomainParameters(ecParams.Curve, ecParams.G, ecParams.N, ecParams.H, ecParams.GetSeed());
            ECKeyGenerationParameters keygenParams = new(n, new SecureRandom());
            ECKeyPairGenerator generator = new();

            generator.Init(keygenParams);
            var keyPair = generator.GenerateKeyPair();

            var privateKey = (ECPrivateKeyParameters)keyPair.Private;
            var publicKey = (ECPublicKeyParameters)keyPair.Public;

            ECPublicKeyParameters publicKeyParams = publicKey;
            var rtaX1 = publicKeyParams.Q.AffineXCoord.ToString();
            var rtaY1 = publicKeyParams.Q.AffineYCoord.ToString();

            rtaX1 = ValidateLength(rtaX1);
            rtaY1 = ValidateLength(rtaY1);
            string sharePublicKey = $"04{rtaX1}{rtaY1}";

            string privateKeyhex = ConvertIntoHex(privateKey);
            Tuple<string, string> encriptedPrivate = EncryptionHelper.EncryptToAESNew(privateKeyhex, aeskeyBiometric!);
            Tuple<string, string> encriptedPublic = EncryptionHelper.EncryptToAESNew(sharePublicKey, aeskeyBiometric!);

            return await Task.FromResult((sharePublicKey, (encriptedPublic, encriptedPrivate)));
        }

        public async Task<byte[]> GenerateDerivateAsync(
            string privatekey,
            string publicKeyextern
        )
        {
            try
            {
                var localPrivatekey = ConvertToECPrivateKey(privatekey);
                var Privatekey = ConvertToECPublicKey(publicKeyextern);
                var exch = new ECDHBasicAgreement();

                exch.Init(localPrivatekey);

                var secretAlice = exch.CalculateAgreement(Privatekey).ToByteArray();

                if (secretAlice.Length > 32)
                {
                    var rta = secretAlice.ToList();
                    rta.RemoveAt(0);
                    secretAlice = [.. rta];
                }

                return await Task.FromResult(secretAlice);
            }
            catch
            {
                throw new BusinessException($"No fue posible procesar la imagen enviada. Por favor, intenta nuevamente o contacta a soporte si el problema persiste.", 500);
            }
        }

        public async Task<string> DecrypImgAsync(string img, byte[] derivateKey, string tags, string IVs, string? sha = null)
        {
            try
            {
                byte[] secretBytes = derivateKey;
                byte[] nonce = Convert.FromBase64String(IVs);
                byte[] tag = Convert.FromBase64String(tags);
                string decryptedText = string.Empty;

                using (AesGcm aesGcm = new(secretBytes, tag.Length))
                {
                    var base64EncodedBytes = Convert.FromBase64String(img);
                    aesGcm.Decrypt(nonce, base64EncodedBytes, tag, base64EncodedBytes, null);
                    decryptedText = Encoding.UTF8.GetString(base64EncodedBytes);
                }

                if (string.IsNullOrEmpty(sha) || (SHA(decryptedText).ToLower().Equals(sha.ToString().ToLower())))
                    return await Task.FromResult(decryptedText);
                else
                    return string.Empty;
            }
            catch
            {
                throw new BusinessException($"No fue posible procesar la imagen enviada. Por favor, intenta nuevamente o contacta a soporte si el problema persiste.", 500);
            }
        }

        private static string SHA(string img)
        {
            byte[] hashValue;
            hashValue = SHA256.HashData(ByteConvert(img));

            return Convert.ToHexString(hashValue);
        }

        private static byte[] ByteConvert(string img)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(img);

            return byteArray;
        }

        private static string ValidateLength(string key)
        {
            if (key.Length < 64)
            {
                int missed = 64 - key.Length;
                for (int i = 0; i < missed; i++)
                {
                    key = $"0{key}";
                }
            }

            return key;
        }

        private static string ConvertIntoHex(ECPrivateKeyParameters obj)
        {
            byte[] privateKeyBytes = obj.D.ToByteArrayUnsigned();

            if (privateKeyBytes.Length > obj.Parameters.Curve.FieldSize / 8)
            {
                byte[] trimmedKeyBytes = new byte[obj.Parameters.Curve.FieldSize / 8];
                Array.Copy(privateKeyBytes, privateKeyBytes.Length - trimmedKeyBytes.Length, trimmedKeyBytes, 0, trimmedKeyBytes.Length);
                privateKeyBytes = trimmedKeyBytes;
            }

            return Convert.ToHexString(privateKeyBytes);
        }

        private static ECPrivateKeyParameters ConvertToECPrivateKey(string hexPrivateKey)
        {
            X9ECParameters ecParams = ECNamedCurveTable.GetByName(CURVE_NAME);
            BigInteger privateKeyBigInt = new(hexPrivateKey, 16);
            ECPrivateKeyParameters privateKey = new(privateKeyBigInt, new ECDomainParameters(ecParams.Curve, ecParams.G, ecParams.N, ecParams.H));

            return privateKey;
        }

        private static ECPublicKeyParameters ConvertToECPublicKey(string hexPublicKey)
        {
            X9ECParameters ecParams = ECNamedCurveTable.GetByName(CURVE_NAME);
            byte[] publicKeyBytes = HexStringToByteArray(hexPublicKey);

            ECDomainParameters domainParams = new(ecParams.Curve, ecParams.G, ecParams.N);
            var ecPoint = domainParams.Curve.DecodePoint(publicKeyBytes);

            return new ECPublicKeyParameters(ecPoint, domainParams);
        }

        private static byte[] HexStringToByteArray(string hexString)
        {
            int numberChars = hexString.Length;
            byte[] bytes = new byte[numberChars / 2];

            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
