using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public class AddDynamicFieldsHandler : IRequestHandler<AddDynamicFieldsQuery, object?>
    {
        private readonly IUIConfigurationRepository _repository;

        public AddDynamicFieldsHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<object?> Handle(AddDynamicFieldsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.AddDynamicFieldsAsync(
                request.ProcesoConvenioGuid.ToString("D").ToLower(),
                request.WorkflowID,
                request.FieldsToAdd
            );
        }
    }

}
