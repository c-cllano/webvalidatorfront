using System.Security.Cryptography;
using System.Text;

namespace UIConfiguration.Infrastructure.Utility
{
    public static class Extensions
    {
        /// <summary>
        /// Cifra un string a AES 256
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public static Tuple<string, string> EncryptToAESNew(this string text, string keyString)
        {
            if (text == null)
            {
                return new Tuple<string, string>("", "");
            }

            byte[] encrypted;
            byte[] encryptedAlg;
            var key = Encoding.UTF8.GetBytes(keyString);


            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using var aes = new AesCryptoServiceProvider();
                var encryptor2 = aes.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(text);
                }

                encrypted = msEncrypt.ToArray();
                encryptedAlg = aesAlg.IV.ToArray();
            }

            return new Tuple<string, string>(Convert.ToBase64String(encrypted), Convert.ToBase64String(encryptedAlg));
        }
    }
}
