using DrawFlowProcess.Domain.Domain;
using DrawFlowProcess.Domain.Repositories;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetJsonConvert
{
    public class GetJsonConvertHandler : IRequestHandler<GetJsonConvertQuery, ExportJson>
    {
        private readonly IDrwaFlowProcessRepository _repository;

        public GetJsonConvertHandler(IDrwaFlowProcessRepository repository)
        {
            _repository = repository;
        }

        public Task<ExportJson> Handle(GetJsonConvertQuery query, CancellationToken cancellationToken)
        {
            var result = _repository.GetJsonConvert(query.JsonDocument);
            return Task.FromResult(result);
        }
    }
}
