using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class TempKeysReconoserIDResponse
    {
        [JsonPropertyName("llavePublica")]
        public string PublicKey { get; set; } = string.Empty;

        [JsonPropertyName("llavePrivada")]
        public string PrivateKey { get; set; } = string.Empty;

        [JsonPropertyName("algorithmPublicKey")]
        public string AlgorithmPublicKey { get; set; } = string.Empty;

        [JsonPropertyName("algorithmPrivateKey")]
        public string AlgorithmPrivateKey { get; set; } = string.Empty;
    }
}
