using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class FaceCompareMegviResponse
    {
        [JsonPropertyName("reqid")]
        public string Reqid { get; set; } = string.Empty;

        [JsonPropertyName("time_used")]
        public int TimeUsed { get; set; }

        [JsonPropertyName("confidence")]
        public decimal Confidence { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;
    }
}
