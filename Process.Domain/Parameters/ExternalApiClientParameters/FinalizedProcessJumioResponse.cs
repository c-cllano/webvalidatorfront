using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class FinalizedProcessJumioResponse
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("account")]
        public AccountJumioResponse Account { get; set; } = default!;

        [JsonPropertyName("workflowExecution")]
        public WorkflowExecutionJumioResponse WorkflowExecution { get; set; } = default!;
    }

    public class AccountJumioResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }

    public class WorkflowExecutionJumioResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }

}
