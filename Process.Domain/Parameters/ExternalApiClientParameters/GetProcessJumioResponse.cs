using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class GetProcessJumioResponse
    {
        [JsonPropertyName("workflow")]
        public WorkflowJumioResponse Workflow { get; set; } = default!;

        [JsonPropertyName("credentials")]
        public List<CredentialJumioResponse> Credentials { get; set; } = default!;

        [JsonPropertyName("decision")]
        public DecisionJumioResponse Decision { get; set; } = default!;

        [JsonPropertyName("capabilities")]
        public CapabilitiesJumioResponse Capabilities { get; set; } = default!;
    }

    public class WorkflowJumioResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }

    public class CredentialJumioResponse
    {
        [JsonPropertyName("parts")]
        public List<PartJumioResponse> Parts { get; set; } = default!;
    }

    public class PartJumioResponse
    {
        [JsonPropertyName("classifier")]
        public string Classifier { get; set; } = string.Empty;

        [JsonPropertyName("href")]
        public string Href { get; set; } = string.Empty;
    }

    public class DecisionJumioResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("risk")]
        public RiskJumioResponse Risk { get; set; } = default!;
    }

    public class RiskJumioResponse
    {
        [JsonPropertyName("score")]
        public float Score { get; set; }
    }

    public class CapabilitiesJumioResponse
    {
        [JsonPropertyName("extraction")]
        public List<ExtractionJumioResponse> Extraction { get; set; } = default!;

        [JsonPropertyName("dataChecks")]
        public List<DataChecksJumioResponse> DataChecks { get; set; } = default!;

        [JsonPropertyName("imageChecks")]
        public List<ImageChecksJumioResponse> ImageChecks { get; set; } = default!;

        [JsonPropertyName("usability")]
        public List<UsabilityJumioResponse> Usability { get; set; } = default!;
    }

    public class UsabilityJumioResponse
    {
        [JsonPropertyName("decision")]
        public Decision2JumioResponse Decision { get; set; } = default!;
    }

    public class ImageChecksJumioResponse
    {
        [JsonPropertyName("decision")]
        public Decision2JumioResponse Decision { get; set; } = default!;
    }

    public class DataChecksJumioResponse
    {
        [JsonPropertyName("decision")]
        public Decision2JumioResponse Decision { get; set; } = default!;
    }

    public class ExtractionJumioResponse
    {
        [JsonPropertyName("decision")]
        public Decision2JumioResponse Decision { get; set; } = default!;

        [JsonPropertyName("data")]
        public DataJumioResponse Data { get; set; } = default!;
    }

    public class Decision2JumioResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public DetailsJumioResponse Details { get; set; } = default!;
    }

    public class ClassificationJumioResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("credentials")]
        public List<ClassificationCredentialJumioResponse> Credentials { get; set; } = default!;

        [JsonPropertyName("decision")]
        public ClassificationDecisionJumioResponse Decision { get; set; } = default!;

        [JsonPropertyName("data")]
        public DataJumioResponse Data { get; set; } = default!;
    }

    public class ClassificationCredentialJumioResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;
    }

    public class ClassificationDecisionJumioResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("details")]
        public DetailsJumioResponse? Details { get; set; } = default!;
    }

    public class DetailsJumioResponse
    {
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;
    }

    public class DataJumioResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("subType")]
        public string SubType { get; set; } = string.Empty;

        [JsonPropertyName("issuingCountry")]
        public string IssuingCountry { get; set; } = string.Empty;

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("dateOfBirth")]
        public string DateOfBirth { get; set; } = string.Empty;

        [JsonPropertyName("expiryDate")]
        public string ExpiryDate { get; set; } = string.Empty;

        [JsonPropertyName("issuingDate")]
        public string IssuingDate { get; set; } = string.Empty;

        [JsonPropertyName("documentNumber")]
        public string DocumentNumber { get; set; } = string.Empty;

        [JsonPropertyName("optionalMrzField1")]
        public string? OptionalMrzField1 { get; set; }

        [JsonPropertyName("optionalMrzField2")]
        public string? OptionalMrzField2 { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; } = string.Empty;

        [JsonPropertyName("placeOfBirth")]
        public string PlaceOfBirth { get; set; } = string.Empty;

        [JsonPropertyName("currentAge")]
        public string CurrentAge { get; set; } = string.Empty;

        [JsonPropertyName("mrz")]
        public MrzJumioResponse Mrz { get; set; } = default!;
    }

    public class MrzJumioResponse
    {
        [JsonPropertyName("line1")]
        public string Line1 { get; set; } = string.Empty;

        [JsonPropertyName("line2")]
        public string Line2 { get; set; } = string.Empty;

        [JsonPropertyName("line3")]
        public string Line3 { get; set; } = string.Empty;
    }
}
