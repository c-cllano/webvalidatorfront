using System.Text.Json.Serialization;

namespace Process.Infrastructure.Clients.ATDP.Responses
{
    public class AtdpTransactionSaveResponse
    {
        [JsonPropertyName("atdpTransactionID")]
        public long AtdpTransactionID { get; set; }
    }
}
