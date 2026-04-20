using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.PostConfiguration
{
    public class SaveConfigurationHandler: IRequestHandler<SaveConfigurationQuery, object>
    {
        private readonly IUIConfigurationRepository _repository;

        public SaveConfigurationHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(SaveConfigurationQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.SaveUIConfiguration(request);
            return result;
        }
    }
}
