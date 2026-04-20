using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class SaveDocumentBothSidesReconoserIDResponse
    {
        [JsonPropertyName("datos")]
        public SaveDocumentBothSidesDataReconoserIDResponse? Data { get; set; }

        [JsonPropertyName("guidBioAnv")]
        public Guid? FrontalGUID { get; set; }
        
        [JsonPropertyName("guidBioRev")]
        public Guid? ReverseGUID { get; set; }
        
        [JsonPropertyName("anvExitoso")]
        public bool? FrontalSuccessful { get; set; }
        
        [JsonPropertyName("revExitoso")]
        public bool? ReverseSuccessful { get; set; }
        
        [JsonPropertyName("anvMensaje")]
        public string? FrontalMessage { get; set; }
        
        [JsonPropertyName("revMensaje")]
        public string? ReverseMessage { get; set; }
        
        [JsonPropertyName("respuestaTransaccion")]
        public TransactionReconoserIDResponse? RespuestaTransaccion { get; set; }
    }

    public class SaveDocumentBothSidesDataReconoserIDResponse
    {
        [JsonPropertyName("data")]
        public SaveDocumentBothSidesDataPropertiesReconoserIDResponse? Data { get; set; }
    }

    public class SaveDocumentBothSidesDataPropertiesReconoserIDResponse
    {
        [JsonPropertyName("documentTypeDesc")]
        public string? DocumentTypeDescription { get; set; }

        [JsonPropertyName("documentType")]
        public string? DocumentType { get; set; }

        [JsonPropertyName("numeroDocumento")]
        public string? DocumentNumber { get; set; }

        [JsonPropertyName("primerNombre")]
        public string? FirstName { get; set; }

        [JsonPropertyName("segundoNombre")]
        public string? SecondName { get; set; }

        [JsonPropertyName("primerApellido")]
        public string? LastName { get; set; }

        [JsonPropertyName("segundoApellido")]
        public string? SecondLastName { get; set; }

        [JsonPropertyName("sexo")]
        public string? Sex { get; set; }

        [JsonPropertyName("rh")]
        public string? Rh { get; set; }

        [JsonPropertyName("lugarNacimiento")]
        public string? PlaceBirth { get; set; }

        [JsonPropertyName("fechaNacimiento")]
        public DateTime? DateBirth { get; set; }

        [JsonPropertyName("lugarExpedicion")]
        public string? PlaceOfIssue { get; set; }

        [JsonPropertyName("fechaExpedicionDoc")]
        public DateTime? DateOfIssue { get; set; }
    }
}
