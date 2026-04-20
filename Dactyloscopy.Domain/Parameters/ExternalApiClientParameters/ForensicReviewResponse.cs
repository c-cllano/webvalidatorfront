using System.Text.Json.Serialization;

namespace Dactyloscopy.Domain.Parameters.ExternalApiClientParameters
{
    public class ForensicReviewResponse
    {
        [JsonPropertyName("data")]
        public ForensicReviewDataResponse Data { get; set; } = default!;

        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("codeName")]
        public string CodeName { get; set; } = string.Empty;
    }

    public class ForensicReviewDataResponse
    {
        [JsonPropertyName("txGuid")]
        public Guid TxGuid { get; set; }

        [JsonPropertyName("procesoConvenioId")]
        public long? ProcesoConvenioId { get; set; }
    }
}
