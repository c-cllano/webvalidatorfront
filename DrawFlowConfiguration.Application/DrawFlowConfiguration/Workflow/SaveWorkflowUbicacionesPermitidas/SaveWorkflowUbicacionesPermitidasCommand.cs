using DrawFlowConfiguration.Domain.Parameters.DrawFlow.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowUbicacionesPermitidas
{
    public record SaveWorkflowUbicacionesPermitidasCommand(SaveWorkflowUbicacionesPermitidasRequest Request) : IRequest<bool>;
}
