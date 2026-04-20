using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class LoginWhatsappResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("appInfo")]
        public AppInfo AppInfo { get; set; } = default!;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class AppInfo
    {
        [JsonPropertyName("appId")]
        public long AppId { get; set; }

        [JsonPropertyName("codigo")]
        public Guid Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public bool Estado { get; set; }

        [JsonPropertyName("aplicationBlock")]
        public bool AplicationBlock { get; set; }

        [JsonPropertyName("ultimoAcceso")]
        public object UltimoAcceso { get; set; } = default!;
    }
}
