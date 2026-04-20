using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface IWhatsappService
    {
        Task<LoginWhatsappResponse> GetTokenWhatsappAsync();
        Task<RequestWhatsappResponse> SendRequestWhatsappAsync(object contentBody);
    }
}
