using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class LoginSignatureResponse
    {
        [JsonPropertyName("loginOK")]
        public bool LoginOK { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }
}
