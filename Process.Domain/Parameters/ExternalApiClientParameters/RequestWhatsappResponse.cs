using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class RequestWhatsappResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("session_initialized")]
        public bool SessionInitialized { get; set; }

        [JsonPropertyName("session_id")]
        public Guid SessionId { get; set; }

        [JsonPropertyName("id_session_unique")]
        public string IdSessionUnique { get; set; } = string.Empty;

        [JsonPropertyName("current_step")]
        public string CurrentStep { get; set; } = string.Empty;

        [JsonPropertyName("next_step")]
        public string NextStep { get; set; } = string.Empty;

        [JsonPropertyName("previous_session_deactivated")]
        public bool PreviousSessionDeactivated { get; set; }
    }
}
