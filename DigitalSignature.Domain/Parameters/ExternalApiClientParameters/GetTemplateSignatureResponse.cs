using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class GetTemplateSignatureResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("documentoBase64")]
        public string DocumentBase64 { get; set; } = string.Empty;
    }
}
