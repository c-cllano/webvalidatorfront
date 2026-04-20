using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.GetConfiguration
{
    public class GetGlobalConfigurationHandler : IRequestHandler<GetGlobalConfigurationQuery, object>
    {
        private readonly IUIConfigurationRepository _repository;
        public GetGlobalConfigurationHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }
        public async Task<object> Handle(GetGlobalConfigurationQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetGlobalConfiguration(requets.ProcesoConvenioGuid.ToString("D").ToLower(), requets.WorkflowID) ?? new object();
            return configuration;
        }
    }
}
