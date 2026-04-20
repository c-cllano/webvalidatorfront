using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class ClientAccountJumioResponse
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("account")]
        public AccountJumio Account { get; set; } = default!;

        [JsonPropertyName("web")]
        public WebJumio Web { get; set; } = default!;

        [JsonPropertyName("sdk")]
        public SdkJumio Sdk { get; set; } = default!;

        [JsonPropertyName("workflowExecution")]
        public WorkflowExecutionJumio WorkflowExecution { get; set; } = default!;
    }

    public class AccountJumio
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }

    public class SdkJumio
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }

    public class WebJumio
    {
        [JsonPropertyName("href")]
        public string Href { get; set; } = string.Empty;
    }

    public class WorkflowExecutionJumio
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("credentials")]
        public List<CredentialJumio> Credentials { get; set; } = default!;
    }

    public class CredentialJumio
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("allowedChannels")]
        public List<string> AllowedChannels { get; set; } = default!;

        [JsonPropertyName("api")]
        public ApiJumio Api { get; set; } = default!;
    }

    public class ApiJumio
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("parts")]
        public PartsJumio Parts { get; set; } = default!;

        [JsonPropertyName("workflowExecution")]
        public string WorkflowExecution { get; set; } = string.Empty;
    }

    public class PartsJumio
    {
        [JsonPropertyName("front")]
        public string Front { get; set; } = string.Empty;

        [JsonPropertyName("back")]
        public string Back { get; set; } = string.Empty;
    }
}
