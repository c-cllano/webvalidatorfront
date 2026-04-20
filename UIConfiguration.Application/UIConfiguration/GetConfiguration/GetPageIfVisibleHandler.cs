using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.GetConfiguration
{
    public class GetPageIfVisibleHandler : IRequestHandler<GetPageIfVisibleQuery, object>
    {
        private readonly IUIConfigurationRepository _repository;
        public GetPageIfVisibleHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }
        public async Task<object> Handle(GetPageIfVisibleQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetPageIfVisible(requets.ProcesoConvenioGuid.ToString("D").ToLower(), requets.PageName, requets.WorkflowID) ?? new object();
            return configuration;
        }
    }
}
