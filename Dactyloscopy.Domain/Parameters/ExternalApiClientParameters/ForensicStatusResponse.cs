using System.Text.Json.Serialization;

namespace Dactyloscopy.Domain.Parameters.ExternalApiClientParameters
{
    public class ForensicStatusResponse
    {
        [JsonPropertyName("data")]
        public ForensicStatusDataResponse Data { get; set; } = default!;

        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("codeName")]
        public string CodeName { get; set; } = string.Empty;
    }

    public partial class ForensicStatusDataResponse
    {
        [JsonPropertyName("txGuid")]
        public Guid TxGuid { get; set; }

        [JsonPropertyName("revisada")]
        public bool Revisada { get; set; }

        [JsonPropertyName("fechaRevision")]
        public DateTime FechaRevision { get; set; }

        [JsonPropertyName("aprobada")]
        public bool Aprobada { get; set; }

        [JsonPropertyName("score")]
        public long Score { get; set; }

        [JsonPropertyName("motivoPrincipal")]
        public string MotivoPrincipal { get; set; } = string.Empty;

        [JsonPropertyName("motivoOpcional")]
        public object MotivoOpcional { get; set; } = default!;

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [JsonPropertyName("observacion")]
        public object Observacion { get; set; } = default!;
    }
}
