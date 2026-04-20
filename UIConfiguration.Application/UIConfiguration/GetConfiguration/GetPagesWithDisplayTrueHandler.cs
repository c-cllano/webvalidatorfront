using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.GetConfiguration
{
    public class GetPagesWithDisplayTrueHandler : IRequestHandler<GetPagesWithDisplayTrueQuery, object>
    {
        private readonly IUIConfigurationRepository _repository;
        public GetPagesWithDisplayTrueHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }
        public async Task<object> Handle(GetPagesWithDisplayTrueQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetPagesWithDisplayTrue(requets.ProcesoConvenioGuid.ToString("D").ToLower(), requets.WorkflowID);
            return configuration;
        }
    }
}
