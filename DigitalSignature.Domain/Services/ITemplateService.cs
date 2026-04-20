using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;

namespace DigitalSignature.Domain.Services
{
    public interface ITemplateService
    {
        Task<ResultResponse<GetTemplateSignatureResponse>> GetTemplateAsync(long clientId, Guid templateSerial);
        Task<ResultResponse<GetTemplateFieldsSignatureResponse>> GetTemplateFieldsAsync(long clientId, Guid templateSerial);
        Task<ResultResponse<CreateTemplateSignatureResponse>> CreateTemplateAsync(long clientId, string templateBase64, string templateName);
        Task<ResultResponse<UpdateTemplateSignatureResponse>> UpdateTemplateAsync(long clientId, Guid templateSerial, string templateBase64, string templateName);
    }
}
