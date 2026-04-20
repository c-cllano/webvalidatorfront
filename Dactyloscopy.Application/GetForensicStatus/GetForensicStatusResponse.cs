namespace Dactyloscopy.Application.GetForensicStatus
{
    public class GetForensicStatusResponse
    {
        public Guid TxGuid { get; set; }
        public bool Reviewed { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Approved { get; set; }
        public long Score { get; set; }
        public string MainReason { get; set; } = string.Empty;
        public object OptionalReason { get; set; } = default!;
        public string Description { get; set; } = string.Empty;
        public object Observation { get; set; } = default!;
    }
}
