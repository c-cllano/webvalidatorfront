using MediatR;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Application.UIConfiguration.PutConfiguration
{
    public class PutDynamicFieldHandler : IRequestHandler<PutDynamicFieldQuery, object?>
    {
        private readonly IUIConfigurationRepository _repository;

        public PutDynamicFieldHandler(IUIConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<object?> Handle(PutDynamicFieldQuery request, CancellationToken cancellationToken)
        {
            return await _repository.UpdateDynamicFieldsAsync(
                clientId: request.ProcesoConvenioGuid.ToString("D").ToLower(),
                workflowID: request.WorkflowID,
                updates: request.FieldsToUpdate
            );
        }
    }
}
