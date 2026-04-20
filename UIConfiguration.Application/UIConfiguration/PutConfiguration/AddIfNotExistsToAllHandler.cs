using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public class AddIfNotExistsToAllHandler : IRequestHandler<AddIfNotExistsToAllQuery, object>
    {
        private readonly IUIConfigurationRepository _repository;
        public AddIfNotExistsToAllHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }
        public async Task<object> Handle(AddIfNotExistsToAllQuery request, CancellationToken cancellationToken)
        {
            return await _repository.AddIfNotExistsToAllAsync(
                request.FieldsToAdd
            );
        }
    }
}
