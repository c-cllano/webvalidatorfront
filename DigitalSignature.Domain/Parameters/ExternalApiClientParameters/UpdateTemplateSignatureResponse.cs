using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class UpdateTemplateSignatureResponse
    {
        [JsonPropertyName("ok")]
        public object Ok { get; set; } = default!;
    }
}
