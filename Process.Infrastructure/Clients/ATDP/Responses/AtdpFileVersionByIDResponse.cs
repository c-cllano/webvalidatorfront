namespace Process.Infrastructure.Clients.ATDP.Responses
{
    using System.Text.Json.Serialization;

    public class AtdpFileVersionByIDResponse
    {
        [JsonPropertyName("atdpVersionID")]
        public int AtdpVersionID { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("file")]
        public string? File { get; set; }

        [JsonPropertyName("sas")]
        public string? SAS { get; set; }
    }
}
