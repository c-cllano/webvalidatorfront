using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class StatusValidationRequestReconoserIDResponse
    {
        [JsonPropertyName("code")]
        public int? Code { get; set; }
        
        [JsonPropertyName("codeName")]
        public string? CodeName { get; set; }
        
        [JsonPropertyName("data")]
        public StatusValidationRequestDataReconoserIDResponse? Data { get; set; }
    }

    public class StatusValidationRequestDataReconoserIDResponse
    {
        [JsonPropertyName("procesoConvenioGuid")]
        public Guid? ProcesoConvenioGuid { get; set; }
        
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        
        [JsonPropertyName("estadoProceso")]
        public int? StatusProcess { get; set; }
        
        [JsonPropertyName("guidCiudadano")]
        public Guid? CitizenGUID { get; set; }
    }
}
