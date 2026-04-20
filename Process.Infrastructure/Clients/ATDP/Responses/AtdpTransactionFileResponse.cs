using System.Text.Json.Serialization;

namespace Process.Infrastructure.Clients.ATDP.Responses
{
    public class AtdpTransactionFileResponse
    {
        [JsonPropertyName("atdpVersionID")]
        public int AtdpVersionID { get; set; }

        [JsonPropertyName("atdpTransactionID")]
        public int AtdpTransactionID { get; set; }

        [JsonPropertyName("documentTypeID")]
        public int DocumentTypeID { get; set; }

        [JsonPropertyName("documentNumber")]
        public string? DocumentNumber { get; set; }

        [JsonPropertyName("isApproved")]
        public bool IsApproved { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("file")]
        public string? File { get; set; }

        [JsonPropertyName("sas")]
        public string? Sas { get; set; }
    }

}
