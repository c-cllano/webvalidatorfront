using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class GenerateDocumentSignatureResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("documentoBase64")]
        public string documentoBase64 { get; set; } = string.Empty;

        [JsonPropertyName("base64")]
        public string base64 { get; set; } = string.Empty;
    }
}
