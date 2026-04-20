using DrawFlowConfiguration.Application.DrawFlowConfiguration.PostWorkflows;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.DuplicateSQLWorkflow
{

    public class DuplicateSQLWorkflowHandler : IRequestHandler<DuplicateSQLWorkflowCommand, bool>
    {
        private readonly IValidationTransaction _transaction;

        public DuplicateSQLWorkflowHandler(IValidationTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task<bool> Handle(DuplicateSQLWorkflowCommand request, CancellationToken cancellationToken)
            => await _transaction.DuplicateSQLWorkflow(request.request);
    }
}
