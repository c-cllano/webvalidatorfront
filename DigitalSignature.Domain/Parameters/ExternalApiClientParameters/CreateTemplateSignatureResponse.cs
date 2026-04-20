using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class CreateTemplateSignatureResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("plantillaSerial")]
        public Guid TemplateSerial { get; set; }
    }
}
