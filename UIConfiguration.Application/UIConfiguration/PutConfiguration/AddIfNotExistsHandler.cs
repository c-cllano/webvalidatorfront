using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public class AddIfNotExistsHandler : IRequestHandler<AddIfNotExistsQuery, object?>
    {
        private readonly IUIConfigurationRepository _repository;

        public AddIfNotExistsHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<object?> Handle(AddIfNotExistsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.AddIfNotExistsAsync(
                request.ProcesoConvenioGuid.ToString("D").ToLower(),
                request.WorkflowID,
                request.FieldsToAdd
            );
        }
    }
}
