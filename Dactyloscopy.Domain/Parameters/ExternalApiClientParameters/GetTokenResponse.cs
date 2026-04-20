using System.Text.Json.Serialization;

namespace Dactyloscopy.Domain.Parameters.ExternalApiClientParameters
{
    public class GetTokenResponse
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refreshToken")]
        public object RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("errorDescription")]
        public object ErrorDescription { get; set; } = default!;

        [JsonPropertyName("expiresIn")]
        public long ExpiresIn { get; set; }
    }
}
