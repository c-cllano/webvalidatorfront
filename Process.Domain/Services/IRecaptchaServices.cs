using Process.Domain.Parameters.Recapcha;

namespace Process.Domain.Services
{
    public interface IRecaptchaServices
    {
        Task<RecaptchaResponse> GetValidateRecaptchaAsync(string Response);
    }
}
