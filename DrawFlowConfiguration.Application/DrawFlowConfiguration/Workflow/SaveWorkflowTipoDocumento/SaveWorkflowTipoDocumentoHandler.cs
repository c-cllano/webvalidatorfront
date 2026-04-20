using DrawFlowConfiguration.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.SaveWorkflowTipoDocumento
{


    public class SaveWorkflowTipoDocumentoHandler : IRequestHandler<SaveWorkflowTipoDocumentoCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public SaveWorkflowTipoDocumentoHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(SaveWorkflowTipoDocumentoCommand request, CancellationToken cancellationToken)
            => await _transaction.SaveWorkflowTipoDocumento(request.Request);
    }
}
