using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public class PutConfigurationHandler : IRequestHandler<PutConfigurationQuery, object>
    {
        private readonly IUIConfigurationRepository _repository;

        public PutConfigurationHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(PutConfigurationQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.UpdateIUConfiguration(request.UIConfigurationJson, request.ProcesoConvenioGuid.ToString("D").ToLower(), request.WorkflowID);
            return result;
        }
    }
}
