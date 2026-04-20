using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class ValidationDocumentAniResponse
    {
        [JsonPropertyName("Respuesta")]
        public ValidationDocumentAniDataResponse Response { get; set; } = default!;

        [JsonPropertyName("CodigoRespuesta")]
        public string ResponseCode { get; set; } = string.Empty;

        [JsonPropertyName("DescripcionRespuesta")]
        public string ResponseDescription { get; set; } = string.Empty;
    }

    public class ValidationDocumentAniDataResponse
    {
        [JsonPropertyName("AnioResolucion")]
        public string YearResolution { get; set; } = string.Empty;

        [JsonPropertyName("CodigoErrorDatosCedula")]
        public string DocumentDataErrorCode { get; set; } = string.Empty;

        [JsonPropertyName("DepartamentoExpedicion")]
        public string IssuingDepartment { get; set; } = string.Empty;

        [JsonPropertyName("EstadoCedula")]
        public string DocumentStatus { get; set; } = string.Empty;

        [JsonPropertyName("FechaDefuncion")]
        public string DateOfDeath { get; set; } = string.Empty;

        [JsonPropertyName("FechaExpedicion")]
        public string IssueDate { get; set; } = string.Empty;

        [JsonPropertyName("Informante")]
        public string Informant { get; set; } = string.Empty;

        [JsonPropertyName("MunicipioExpedicion")]
        public string IssuingMunicipality { get; set; } = string.Empty;

        [JsonPropertyName("NUIP")]
        public string Niup { get; set; } = string.Empty;

        [JsonPropertyName("NumeroResolucion")]
        public string ResolutionNumber { get; set; } = string.Empty;

        [JsonPropertyName("Particula")]
        public string Particle { get; set; } = string.Empty;

        [JsonPropertyName("PrimerApellido")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("PrimerNombre")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("SegundoApellido")]
        public string SecondLastName { get; set; } = string.Empty;

        [JsonPropertyName("SegundoNombre")]
        public string SecondName { get; set; } = string.Empty;

        [JsonPropertyName("Serial")]
        public string Serial { get; set; } = string.Empty;
    }
}
