namespace Process.Domain.Services
{
    public interface IEncryptionKeyProviderService
    {
        Task<(string publicKey, (Tuple<string, string>, Tuple<string, string>) encripted)> GenerateKeysAsync();
        Task<byte[]> GenerateDerivateAsync(string privatekey, string publicKeyextern);
        Task<string> DecrypImgAsync(string img, byte[] derivateKey, string tags, string IVs, string? sha = null);
    }
}
