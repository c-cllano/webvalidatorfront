using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.PutWorkflow;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowNacionalidadesPermitidas
{

    public class SaveWorkflowNacionalidadesPermitidasHandler : IRequestHandler<SaveWorkflowNacionalidadesPermitidasCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public SaveWorkflowNacionalidadesPermitidasHandler (IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(SaveWorkflowNacionalidadesPermitidasCommand request, CancellationToken cancellationToken)
            => await _transaction.SaveWorkflowNacionalidadesPermitidas(request.Request);
    }
}
