namespace Process.Domain.Entities
{
    public class BiometricKey
    {
        public long Id { get; set; }
        public Guid? ProcesoConvenioGuid { get; set; }
        public string LlavePublica { get; set; } = string.Empty;
        public string LlavePrivada { get; set; } = string.Empty;
        public string AlgorithmPrivateKey { get; set; } = string.Empty;
        public string AlgorithmPublicKey { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
