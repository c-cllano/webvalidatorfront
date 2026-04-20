using DrawFlowConfiguration.Domain.Parameters.DrawFlow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.DuplicateSQLWorkflow
{

    public record DuplicateSQLWorkflowCommand(WorkflowsEntry request) : IRequest<bool>;
}
