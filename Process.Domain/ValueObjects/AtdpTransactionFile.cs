
namespace Process.Domain.ValueObjects
{
    public class AtdpTransactionFile(
        int transactionId, 
        int documentTypeId, 
        string documentNumber,
        bool isApproved, 
        DateTime date, 
        string fileContent, 
        string sasUrl
    )
    {
        public int TransactionId { get; private set; } = transactionId;
        public int DocumentTypeId { get; private set; } = documentTypeId;
        public string DocumentNumber { get; private set; } = documentNumber;
        public bool IsApproved { get; private set; } = isApproved;
        public DateTime Date { get; private set; } = date;
        public string FileContent { get; private set; } = fileContent;
        public string SasUrl { get; private set; } = sasUrl;
    }

}
