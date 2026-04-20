using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class SaveBiometricReconoserIDResponse
    {
        [JsonPropertyName("guidBio")]
        public Guid? GuidBio { get; set; }
        
        [JsonPropertyName("spoofPrediction")]
        public string? SpoofPrediction { get; set; }
        
        [JsonPropertyName("spoofPredictionGesto")]
        public string? SpoofPredictionGesto { get; set; }
        
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        
        [JsonPropertyName("respuestaTransaccion")]
        public TransactionReconoserIDResponse? RespuestaTransaccion { get; set; }
    }

    public class TransactionReconoserIDResponse
    {
        [JsonPropertyName("isHomologacion")]
        public bool? IsHomologacion { get; set; }
        
        [JsonPropertyName("esExitosa")]
        public bool? EsExitosa { get; set; }

        [JsonPropertyName("errorEntransaccion")]
        public List<ErrorInTransactionReconoserIDResponse>? ErrorEntransaccion { get; set; } = default!;
    }

    public class ErrorInTransactionReconoserIDResponse
    {
        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; } = string.Empty;
        
        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; } = string.Empty;
        
        [JsonPropertyName("descripcionIngles")]
        public string? DescripcionIngles { get; set; } = string.Empty;
    }
}
