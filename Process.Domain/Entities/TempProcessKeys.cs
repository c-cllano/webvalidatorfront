namespace Process.Domain.Entities
{
    public class TempProcessKeys
    {
        public long TempProcessKeysId { get; set; }
        public Guid ValidationProcessGuid { get; set; }
        public string PublicKey { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public string AlgorithmPublicKey { get; set; } = string.Empty;
        public string AlgorithmPrivateKey { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
