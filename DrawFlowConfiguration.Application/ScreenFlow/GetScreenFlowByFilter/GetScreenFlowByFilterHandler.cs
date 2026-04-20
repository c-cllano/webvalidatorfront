using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.Workflow.GetWorkflowsByFilter;
using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.ScreenFlow.GetScreenFlowByFilter
{

    public class GetScreenFlowByFilterHandler : IRequestHandler<GetScreenFlowByFilterQuery, object>
    {
        private readonly IScreenFlowRepository _transaction;

        public GetScreenFlowByFilterHandler(IScreenFlowRepository transaction)
        {
            _transaction = transaction;
        }

        public async Task<object> Handle(GetScreenFlowByFilterQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _transaction.GetScreenFlowByFilter(requets.ScreenFlowID, requets.AgreementId, requets.SelectedIdWorkflow);
            return configuration;
        }

    }
}
