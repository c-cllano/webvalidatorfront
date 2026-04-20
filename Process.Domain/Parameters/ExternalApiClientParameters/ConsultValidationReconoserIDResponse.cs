using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class ConsultValidationReconoserIDResponse
    {
        [JsonPropertyName("code")]
        public int? Code { get; set; }

        [JsonPropertyName("codeName")]
        public string? CodeName { get; set; }

        [JsonPropertyName("data")]
        public ConsultValidationDataReconoserIDResponse? Data { get; set; }
    }

    public class ConsultValidationDataReconoserIDResponse
    {
        [JsonPropertyName("guidConv")]
        public Guid? AgreementGUID { get; set; }

        [JsonPropertyName("procesoConvenioGuid")]
        public Guid? AgreementProcessGUID { get; set; }

        [JsonPropertyName("primerNombre")]
        public string? FirstName { get; set; }

        [JsonPropertyName("segundoNombre")]
        public string? SecondName { get; set; }

        [JsonPropertyName("primerApellido")]
        public string? FirstLastName { get; set; }

        [JsonPropertyName("segundoApellido")]
        public string? SecondLastName { get; set; }

        [JsonPropertyName("sexo")]
        public string? Sex { get; set; }

        [JsonPropertyName("rh")]
        public string? Rh { get; set; }

        [JsonPropertyName("fechaNacimiento")]
        public DateTime? BirthDate { get; set; }

        [JsonPropertyName("lugarNacimiento")]
        public string? PlaceBirth { get; set; }

        [JsonPropertyName("lugarExpedicion")]
        public string? PlaceExpedition { get; set; }

        [JsonPropertyName("fechaExpedicion")]
        public DateTime? ExpeditionDate { get; set; }

        [JsonPropertyName("tipoDoc")]
        public string? DocumentType { get; set; }

        [JsonPropertyName("numDoc")]
        public string? DocumentNumber { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("estadoProceso")]
        public int? ProcessStatus { get; set; }

        [JsonPropertyName("scoreProceso")]
        public int? ProcessScore { get; set; }

        [JsonPropertyName("aprobado")]
        public bool? Approved { get; set; }

        [JsonPropertyName("cancelado")]
        public bool? Cancel { get; set; }

        [JsonPropertyName("estadoForense")]
        public int? ForensicState { get; set; }

        [JsonPropertyName("motivoForense")]
        public string? ForensicReason { get; set; }

        [JsonPropertyName("motivoOpcionalForense")]
        public string? ForensicOptionalReason { get; set; }

        [JsonPropertyName("observacionesForense")]
        public string? ForensicObservations { get; set; }

        [JsonPropertyName("motivoCancelacion")]
        public string? CancelationReason { get; set; }

        [JsonPropertyName("estadoDescripcion")]
        public string? StatusDescription { get; set; }

        [JsonPropertyName("motivoId")]
        public int? ReasonId { get; set; }

        [JsonPropertyName("finalizado")]
        public bool? Finalized { get; set; }

        [JsonPropertyName("fechaRegistro")]
        public DateTime? RegistrationDate { get; set; }

        [JsonPropertyName("fechaFinalizacion")]
        public DateTime? FinalizedDate { get; set; }
    }
}
