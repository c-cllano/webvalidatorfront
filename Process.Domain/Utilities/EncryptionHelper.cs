using System.Security.Cryptography;
using System.Text;

namespace Process.Domain.Utilities
{
    public static class EncryptionHelper
    {
        public static Tuple<string, string> EncryptToAESNew(string text, string keyString)
        {
            if (text == null)
            {
                return new Tuple<string, string>(null!, null!);
            }

            byte[] encrypted;
            byte[] encryptedAlg;
            var key = Encoding.UTF8.GetBytes(keyString);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(text);
                }

                encrypted = msEncrypt.ToArray();
                encryptedAlg = [.. aesAlg.IV];
            }

            return new Tuple<string, string>(Convert.ToBase64String(encrypted), Convert.ToBase64String(encryptedAlg));
        }

        public static string DecryptFromAESNew(string cipherText, string keyString, string algorit)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            var key = Encoding.UTF8.GetBytes(keyString);
            byte[] IV = Convert.FromBase64String(algorit);

            string? plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new(Convert.FromBase64String(cipherText));
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);

                try
                {
                    plaintext = srDecrypt.ReadToEnd();
                }
                catch (Exception)
                {
                    return cipherText;
                }
            }

            return plaintext;
        }
    }
}
