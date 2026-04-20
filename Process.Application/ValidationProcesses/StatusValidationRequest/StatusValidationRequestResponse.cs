using System.Text.Json.Serialization;

namespace Process.Application.ValidationProcesses.StatusValidationRequest
{
    public class StatusValidationRequestResponse
    {
        public long? ValidationProcessId { get; set; }
        public Guid? ValidationProcessGUID { get; set; }
        public string? Url { get; set; }
        public int? StatusProcess { get; set; }
        public Guid? CitizenGUID { get; set; }

        public string? FullName { get; set; }
    }
}
