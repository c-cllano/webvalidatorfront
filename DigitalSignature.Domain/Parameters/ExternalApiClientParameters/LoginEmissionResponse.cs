using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class LoginEmissionResponse
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }
        
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
