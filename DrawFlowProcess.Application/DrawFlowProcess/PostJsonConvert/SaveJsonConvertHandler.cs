using DrawFlowProcess.Domain.Repositories;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.PostJsonConvert
{
    public class SaveJsonConvertHandler : IRequestHandler<SaveJsonConvertQuery, bool>
    {
        private readonly IDrwaFlowProcessRepository _repository;

        public SaveJsonConvertHandler(IDrwaFlowProcessRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(SaveJsonConvertQuery query, CancellationToken cancellationToken)
        {
            var resultGet = _repository.GetJsonConvert(query.Document!);
            var result = await _repository.SaveJsonConvert(resultGet);
            return result;
        }
    }
}
