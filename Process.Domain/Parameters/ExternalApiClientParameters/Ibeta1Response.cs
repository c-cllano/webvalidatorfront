using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class Ibeta1Response
    {
        [JsonPropertyName("api_hash")]
        public string ApiHash { get; set; } = string.Empty;

        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; } = string.Empty;

        [JsonPropertyName("user_name")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("service")]
        public string Service { get; set; } = string.Empty;

        [JsonPropertyName("api_gateway_id")]
        public string ApiGatewayId { get; set; } = string.Empty;

        [JsonPropertyName("file_token")]
        public string FileToken { get; set; } = string.Empty;

        [JsonPropertyName("with_iqa")]
        public bool WithIqa { get; set; }

        [JsonPropertyName("smiling")]
        public bool Smiling { get; set; }

        [JsonPropertyName("save_azure")]
        public bool SaveAzure { get; set; }

        [JsonPropertyName("save_mongo")]
        public bool SaveMongo { get; set; }

        [JsonPropertyName("ts")]
        public double Ts { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("summary")]
        public Summary Summary { get; set; } = default!;

        [JsonPropertyName("metadata")]
        public MetadataIbetaOne Metadata { get; set; } = default!;

        [JsonPropertyName("user_data")]
        public object UserData { get; set; } = default!;

        [JsonPropertyName("http_code")]
        public long HttpCode { get; set; }

        [JsonPropertyName("total_time_ms")]
        public long TotalTimeMs { get; set; }

        [JsonPropertyName("db_manager")]
        public string DbManager { get; set; } = string.Empty;
    }

    public class MetadataIbetaOne
    {
        [JsonPropertyName("integrity_dims")]
        public IntegrityDims IntegrityDims { get; set; } = default!;

        [JsonPropertyName("accessory_detector")]
        public AccessoryDetector AccessoryDetector { get; set; } = default!;

        [JsonPropertyName("face_detector")]
        public FaceDetector FaceDetector { get; set; } = default!;

        [JsonPropertyName("classic_metrics")]
        public ClassicMetrics ClassicMetrics { get; set; } = default!;

        [JsonPropertyName("iqa_heuristics")]
        public IqaHeuristics IqaHeuristics { get; set; } = default!;

        [JsonPropertyName("as_heuristics")]
        public AsHeuristics AsHeuristics { get; set; } = default!;
    }
}
