using System.Text.Json.Serialization;

namespace DigitalSignature.Domain.Parameters.ExternalApiClientParameters
{
    public class ResultResponse<T>
    {
        [JsonPropertyName("Mensaje")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("Respuesta")]
        public bool Response { get; set; }

        [JsonPropertyName("Codigo")]
        public long Code { get; set; }

        [JsonPropertyName("Data")]
        public T Data { get; set; } = default!;
    }
}
