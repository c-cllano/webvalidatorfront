using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;

namespace DigitalSignature.Domain.Services
{
    public interface IDocumentService
    {
        Task<ResultResponse<GenerateDocumentSignatureResponse>> GenerateDocumentAsync(long clientId, Guid templateSerial, IEnumerable<GenerateDocumentDataRequest> data);
    }
}
