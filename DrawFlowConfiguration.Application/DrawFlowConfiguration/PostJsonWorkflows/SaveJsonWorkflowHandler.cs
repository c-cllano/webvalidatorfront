using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PostJsonWorkflows
{


    public class SaveJsonWorkflowHandler : IRequestHandler<SaveJsonWorkflowQuery, object>
    {

        private readonly IJsonWorkflowRepository _repository;

        public SaveJsonWorkflowHandler(IJsonWorkflowRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(SaveJsonWorkflowQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.SaveUIConfiguration(request);
            return result;
        }
    }

  
  
}
