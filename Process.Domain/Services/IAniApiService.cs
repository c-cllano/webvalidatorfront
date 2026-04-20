using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface IAniApiService
    {
        Task<string> GetTokenAniAsync();
        Task<ValidationDocumentAniResponse> ValidationDocumentAniAsync(string documentNumber, string documentType);
    }
}
