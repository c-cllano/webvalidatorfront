using DigitalSignature.Domain.Parameters.ExternalApiClientParameters;

namespace DigitalSignature.Domain.Exceptions
{
    public class ExternalApiException(ExternalApiError error) : Exception
    {
        public int? Code { get; } = error.Code;
        public string? CodeName { get; } = error.CodeName;
        public string? MessageError { get; } = error.Data?.Mesage;
    }
}
