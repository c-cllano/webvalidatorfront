namespace Process.Domain.Parameters.Atdpt
{
    public class SaveTransactionRequest
    {
        public int ATDPVersionID { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string TransactionID { get; set; } = string.Empty;
        public string Commerce { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? SecondName { get; set; }
        public string FirstLastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? File { get; set; }
        public bool Signature { get; set; }
        public DateTime Date { get; set; }
        public bool IsApproved { get; set; }
       
    }
}
