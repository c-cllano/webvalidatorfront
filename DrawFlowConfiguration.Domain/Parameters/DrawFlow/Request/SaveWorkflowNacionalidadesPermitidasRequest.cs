using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request
{

    public class SaveWorkflowNacionalidadesPermitidasRequest
    {
        public int WorkflowNacionalidadesPermitidasId { get; set; }
        public int WorkFlowID { get; set; }
        public int CountryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
