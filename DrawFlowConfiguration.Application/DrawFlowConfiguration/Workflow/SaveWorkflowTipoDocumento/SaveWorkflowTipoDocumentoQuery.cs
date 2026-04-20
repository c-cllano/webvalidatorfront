using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowTipoDocumento
{

    public class SaveWorkflowTipoDocumentoQuery : IRequest<object>
    {
        public int WorkflowTipoDocumentoId { get; set; }
        public int WorkFlowID { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
