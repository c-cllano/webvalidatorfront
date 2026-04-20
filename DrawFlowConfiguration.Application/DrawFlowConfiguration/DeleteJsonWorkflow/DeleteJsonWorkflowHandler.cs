using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.DeleteJsonWorkflow
{
    public class DeleteJsonWorkflowHandler : IRequestHandler<DeleteJsonWorkflowCommand, object>
    {
        private readonly IJsonWorkflowRepository _repository;

        public DeleteJsonWorkflowHandler(IJsonWorkflowRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(DeleteJsonWorkflowCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _repository.DeleteWorkflow(request.AgreementID, request.WorkFlowID);
            return new { deleted }; 
        }
    }
}
