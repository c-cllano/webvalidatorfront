using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class ConsultAgreementProcessReconoserIDResponse
    {
        [JsonPropertyName("datos")]
        public ConsultAgreementProcessDataReconoserIDResponse Data { get; set; } = default!;

        [JsonPropertyName("RespuestaTransaccion")]
        public TransactionResponse TransactionResponse { get; set; } = default!;
    }

    public class TransactionResponse
    {
        [JsonPropertyName("isHomologacion")]
        public bool IsHomologation { get; set; } = false;

        [JsonPropertyName("EsExitosa")]
        public bool IsSucessfull { get; set; } = true;

        [JsonPropertyName("ErrorEntransaccion")]
        public List<ErrorInTransaction> TransactionError { get; set; } = default!;
    }

    public class ErrorInTransaction
    {
        [JsonPropertyName("Codigo")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("Descripcion")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("DescripcionIngles")]
        public string DescriptionEnglish { get; set; } = string.Empty;
    }

    public class ConsultAgreementProcessDataReconoserIDResponse
    {
        [JsonPropertyName("guidConv")]
        public Guid? AgreementGuid { get; set; }

        [JsonPropertyName("procesoConvenioGuid")]
        public Guid? AgreementProcessGuid { get; set; }

        [JsonPropertyName("guidciu")]
        public Guid? CitizenGuid { get; set; }

        [JsonPropertyName("primerNombre")]
        public string? FirstName { get; set; }

        [JsonPropertyName("segundoNombre")]
        public string? SecondName { get; set; }

        [JsonPropertyName("primerApellido")]
        public string? LastName { get; set; }

        [JsonPropertyName("segundoApellido")]
        public string? SecondLastName { get; set; }

        [JsonPropertyName("infCandidato")]
        public string? CandidateInformation { get; set; }

        [JsonPropertyName("tipoDoc")]
        public string? DocumentType { get; set; }

        [JsonPropertyName("numDoc")]
        public string? DocumentNumber { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("celular")]
        public string? Phone { get; set; }

        [JsonPropertyName("asesor")]
        public string? Adviser { get; set; }

        [JsonPropertyName("sede")]
        public string? BranchCode { get; set; }

        [JsonPropertyName("nombreSede")]
        public string? BranchName { get; set; }

        [JsonPropertyName("codigoCliente")]
        public string? ClientCode { get; set; }

        [JsonPropertyName("ejecutarEnMovil")]
        public bool? RunOnMobile { get; set; }

        [JsonPropertyName("estadoProceso")]
        public int? ProcessStatus { get; set; }

        [JsonPropertyName("finalizado")]
        public bool? Finalized { get; set; }

        [JsonPropertyName("estadoForense")]
        public int? ForensicStatus { get; set; }

        [JsonPropertyName("validacion")]
        public bool? Validation { get; set; }

        [JsonPropertyName("fechaRegistro")]
        public DateTime? RegistryDate { get; set; }

        [JsonPropertyName("fechaFinalizacion")]
        public DateTime? FinalizationDate { get; set; }

        [JsonPropertyName("fechaExpedicion")]
        public DateTime? ExpeditionDate { get; set; }

        [JsonPropertyName("lugarExpedicion")]
        public string? PlaceExpedition { get; set; }

        [JsonPropertyName("biometrias")]
        public string? Biometrics { get; set; }

        [JsonPropertyName("convenioId")]
        public int? AgreementId { get; set; }
    }
}
