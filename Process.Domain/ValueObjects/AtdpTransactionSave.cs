namespace Process.Domain.ValueObjects
{
    public class AtdpTransactionSave(long atdpTransactionID)
    {
        public long AtdpTransactionID { get; set; } = atdpTransactionID;
    }
}
