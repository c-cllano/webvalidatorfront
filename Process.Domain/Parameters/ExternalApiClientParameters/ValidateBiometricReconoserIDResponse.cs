using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class ValidateBiometricReconoserIDResponse
    {
        [JsonPropertyName("faceRiskDetected")]
        public bool? FaceRiskDetected { get; set; }
        
        [JsonPropertyName("spoofDetected")]
        public bool? SpoofDetected { get; set; }
        
        [JsonPropertyName("esValido")]
        public bool? IsValid { get; set; }
        
        [JsonPropertyName("resultado")]
        public object? Result { get; set; }
        
        [JsonPropertyName("score")]
        public decimal? Score { get; set; }
        
        [JsonPropertyName("scoreRostroDocumento")]
        public decimal? ScoreFaceDocument { get; set; }
                
        [JsonPropertyName("comparacionRostroDocumento")]
        public bool? CompareFaceDocument { get; set; }
        
        [JsonPropertyName("intentos")]
        public int? Attempts { get; set; }
        
        [JsonPropertyName("codigoProceso")]
        public string? ProcessCode { get; set; }
        
        [JsonPropertyName("spoofPrediction")]
        public string? SpoofPrediction { get; set; }
        
        [JsonPropertyName("spoofPredictionGesto")]
        public string? SpoofPredictionGesto { get; set; }
        
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("respuestaTransaccion")]
        public TransactionReconoserIDResponse? RespuestaTransaccion { get; set; }
    }
}
