using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request
{
    public class PublicarWorkflowRequest
    {
        public int WorkFlowID { get; set; }
        public Guid AgreementID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
