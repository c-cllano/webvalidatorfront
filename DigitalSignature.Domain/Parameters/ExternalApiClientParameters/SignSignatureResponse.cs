using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class SignSignatureResponse
    {
        [JsonPropertyName("base64")]
        public string Base64 { get; set; } = string.Empty;
    }
}
