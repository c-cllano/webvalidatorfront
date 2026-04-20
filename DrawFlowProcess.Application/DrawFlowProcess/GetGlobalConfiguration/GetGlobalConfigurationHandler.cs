using DrawFlowProcess.Domain.Repositories;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetGlobalConfiguration
{
    public class GetGlobalConfigurationHandler : IRequestHandler<GetGlobalConfigurationQuery, List<GetGlobalConfigurationResponse>>
    {
        private readonly IGlobalConfigurationRepository _repository;

        public GetGlobalConfigurationHandler(IGlobalConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetGlobalConfigurationResponse>> Handle(GetGlobalConfigurationQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetGlobalConfigurationsAsync(request.AgreementId, request.WorkFlowId, request.CreateDateTask, request.Section);

            return result.Select(x => new GetGlobalConfigurationResponse()
            {
                Page = x.Page,
                Result = x.Result
            }).ToList() ?? new List<GetGlobalConfigurationResponse>();
        }
    }
}
