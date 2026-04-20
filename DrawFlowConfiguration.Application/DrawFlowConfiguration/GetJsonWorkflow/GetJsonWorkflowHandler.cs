using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetJsonWorkflow
{

    public class GetJsonWorkflowHandler : IRequestHandler<GetJsonWorkflowQuery, object>
    {
        private readonly IJsonWorkflowRepository _repository;

        public GetJsonWorkflowHandler(IJsonWorkflowRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(GetJsonWorkflowQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetUIConfiguration(requets.AgreementID, requets.WorkFlowID);
            return configuration;
        }

    }
}
