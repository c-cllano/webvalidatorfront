using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class GetTemplateFieldsSignatureResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("anotaciones")]
        public IEnumerable<string> Annotations { get; set; } = default!;
    }
}
