using DrawFlowProcess.Domain.Domain;
using DrawFlowProcess.Domain.Repositories;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GteJsonFilter
{
    public class GetJsonFilterHandler : IRequestHandler<GetJsonFilterQuery, ExportJson>
    {
        private readonly IDrwaFlowProcessRepository _repository;

        public GetJsonFilterHandler(IDrwaFlowProcessRepository repository)
        {
            _repository = repository;
        }

        public async Task<ExportJson> Handle(GetJsonFilterQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetJsonByIds(query.AgreementId, query.WorkflowId);
            return result;
        }
    }
}
