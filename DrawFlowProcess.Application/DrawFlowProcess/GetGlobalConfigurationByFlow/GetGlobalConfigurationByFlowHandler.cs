using DrawFlowProcess.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetGlobalConfigurationByFlow
{
    public class GetGlobalConfigurationByFlowHandler : IRequestHandler<GetGlobalConfigurationByFlowQuery, JsonDocument>
    {
        private readonly IGlobalConfigurationRepository _repository;

        public GetGlobalConfigurationByFlowHandler(IGlobalConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<JsonDocument> Handle(GetGlobalConfigurationByFlowQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetGlobalConfigurationByFlow(query.AgreementId, query.WorkFlowId);
            return result;
        }
    }
}
