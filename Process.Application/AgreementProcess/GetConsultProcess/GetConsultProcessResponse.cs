using Process.Domain.Parameters.ExternalApiClientParameters;
using System.Text.Json.Serialization;

namespace Process.Application.AgreementProcess.GetConsultProcess
{
    public class GetConsultProcessResponse
    {
        [JsonPropertyName("datos")]
        public required ProcessData ProcessData { get; set; }

        [JsonPropertyName("RespuestaTransaccion")]
        public TransactionResponse TransactionResponse { get; set; } = default!;
    }

    public class ProcessData
    {
        [JsonPropertyName("guidConv")]
        public Guid AgreementGuid { get; set; }

        [JsonPropertyName("procesoConvenioGuid")]
        public Guid AgreementProcessGuid { get; set; }

        [JsonPropertyName("guidCiu")]
        public Guid CitizenGuid { get; set; }

        [JsonPropertyName("primerNombre")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("segundoNombre")]
        public string MiddleName { get; set; } = string.Empty;

        [JsonPropertyName("primerApellido")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("segundoApellido")]
        public string SecondLastName { get; set; } = string.Empty;

        [JsonPropertyName("infCandidato")]
        public string CandidateInfo { get; set; } = string.Empty;

        [JsonPropertyName("tipoDoc")]
        public string DocumentType { get; set; } = string.Empty;

        [JsonPropertyName("numDoc")]
        public string DocumentNumber { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("celular")]
        public string Mobile { get; set; } = string.Empty;

        [JsonPropertyName("asesor")]
        public string? Advisor { get; set; }

        [JsonPropertyName("sede")]
        public string? BranchCode { get; set; }

        [JsonPropertyName("nombreSede")]
        public string? BranchName { get; set; }

        [JsonPropertyName("codigoCliente")]
        public string ClientCode { get; set; } = string.Empty;

        [JsonPropertyName("ejecutarEnMovil")]
        public bool? ExecuteOnMobile { get; set; }

        [JsonPropertyName("estadoProceso")]
        public int? ProcessState { get; set; }

        [JsonPropertyName("finalizado")]
        public bool? IsCompleted { get; set; }

        [JsonPropertyName("estadoForense")]
        public int? ForensicState { get; set; }

        [JsonPropertyName("validacion")]
        public bool? Validation { get; set; }

        [JsonPropertyName("fechaRegistro")]
        public DateTime? RegistrationDate { get; set; }

        [JsonPropertyName("fechaFinalizacion")]
        public DateTime? CompletionDate { get; set; }

        [JsonPropertyName("fechaExpedicion")]
        public DateTime? IssueDate { get; set; }

        [JsonPropertyName("lugarExpedicion")]
        public string IssuePlace { get; set; } = string.Empty;

        public long ValidationProcessId { get; set; }

        public long ValidationProcessExecutionId { get; set; }

        public string? LastStep { get; set; }

        public long? WorkflowId { get; set; }

        public string? UserReconoserId { get; set; }
    }

}
