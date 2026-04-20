using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowTipoDocumento
{ 
    public record SaveWorkflowTipoDocumentoCommand(SaveWorkflowTipoDocumentoRequest Request) : IRequest<bool>;
}
