namespace Process.Domain.Services
{
    public interface IEncryptImageBlobService
    {
        Task<string> EncryptImageAsync(string fileBase64);
        Task<string> DecryptImageAsync(string payload);
    }
}
