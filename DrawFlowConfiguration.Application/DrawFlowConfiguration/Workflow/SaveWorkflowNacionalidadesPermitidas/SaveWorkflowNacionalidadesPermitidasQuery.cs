using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowNacionalidadesPermitidas
{

    public class SaveWorkflowNacionalidadesPermitidasQuery : IRequest<object>
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
