using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.PutJsonWorkflow
{

    

    public class PutJsonWorkflowHandler : IRequestHandler<PutJsonWorkflowQuery, object>
    {
        private readonly IJsonWorkflowRepository _repository;

        public PutJsonWorkflowHandler(IJsonWorkflowRepository repository)
        {
            _repository = repository;
        }


        public async Task<object> Handle(PutJsonWorkflowQuery request, CancellationToken cancellationToken)
        {
            var agreementId = request.AgreementID;
            var workFlowId = request.WorkFlowID;
            var drawflow = request.drawflow;

            var result = await _repository.UpdateUIConfiguration(agreementId, workFlowId, drawflow);

            return result;
        }
    }
}
