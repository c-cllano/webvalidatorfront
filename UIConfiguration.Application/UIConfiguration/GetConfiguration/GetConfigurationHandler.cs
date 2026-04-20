using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.GetConfiguration
{
    public class GetConfigurationHandler : IRequestHandler<GetConfigurationQuery, object>
    {
        private readonly IUIConfigurationRepository _repository;

        public GetConfigurationHandler(IUIConfigurationRepository repository) 
        {
            _repository = repository;
        }

        public async Task<object> Handle(GetConfigurationQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetUIConfiguration(requets.ProcesoConvenioGuid.ToString("D").ToLower(), requets.WorkflowID);
            return configuration;
        }
    }
}
