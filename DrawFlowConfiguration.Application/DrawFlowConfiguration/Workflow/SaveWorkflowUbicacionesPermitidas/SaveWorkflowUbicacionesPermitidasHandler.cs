using DrawFlowConfiguration.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowUbicacionesPermitidas
{



    public class SaveWorkflowUbicacionesPermitidasHandler : IRequestHandler<SaveWorkflowUbicacionesPermitidasCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public SaveWorkflowUbicacionesPermitidasHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(SaveWorkflowUbicacionesPermitidasCommand request, CancellationToken cancellationToken)
            => await _transaction.SaveWorkflowUbicacionesPermitidas(request.Request);
    }
}
