using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class CompareFacesReconoserIDResponse
    {
        [JsonPropertyName("data")]
        public CompareFacesDataReconoserIDResponse Data { get; set; } = default!;

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("codeName")]
        public string CodeName { get; set; } = string.Empty;
    }

    public class CompareFacesDataReconoserIDResponse
    {
        [JsonPropertyName("esValido")]
        public bool IsValid { get; set; }

        [JsonPropertyName("resultado")]
        public string Result { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("provider")]
        public int Provider { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("mensaje")]
        public string Message { get; set; } = string.Empty;
    }
}
