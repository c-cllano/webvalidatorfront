using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Domain.Parameters.Transaction
{
    public class SaveTransactionRequest
    {
        public long ValidationProcessId { get; set; }
        public Guid ProcessGuid { get; set; }
        public int Status { get; set; }
        public bool Approved { get; set; }
        public int RejectionReaseon { get; set; }
        public string? RejectionDetail { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
