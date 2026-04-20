using DrawFlowProcess.Domain.Repositories;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetJsonPages
{
    public class GetJsonPagesHandler : IRequestHandler<GetJsonPagesQuery, GetJsonPagesResponse>
    {
        public IDrwaFlowProcessRepository _repository;

        public GetJsonPagesHandler(IDrwaFlowProcessRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetJsonPagesResponse> Handle(GetJsonPagesQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetJsonPages(query.AgreementId, query.WorkFlowId);

            return new GetJsonPagesResponse()
            {
                CountPages = result.CountPages,
                Pages = result.Pages
            };
        }
    }
}
