using System.Text.Json.Serialization;
namespace UIConfiguration.Domain.Parameters.Auth
{
    public class TokenResponseOut
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        public string ErrorDescription { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
