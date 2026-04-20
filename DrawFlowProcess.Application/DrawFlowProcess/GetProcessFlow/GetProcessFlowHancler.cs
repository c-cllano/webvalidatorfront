using DrawFlowProcess.Domain.Repositories;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.GetProcessFlow
{
    public class GetProcessFlowHancler : IRequestHandler<GetProcessFlowQuery, GetProcessFlowResponse>
    {
        private readonly IDrwaFlowProcessRepository _repository;

        public GetProcessFlowHancler(IDrwaFlowProcessRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProcessFlowResponse> Handle(GetProcessFlowQuery processFlowQuery, CancellationToken cancellationToken)
        {
            var result = await _repository.GetProcessFlow(processFlowQuery.AgreeentId, processFlowQuery.WorkFlowId, processFlowQuery.NameType!, processFlowQuery.Conditional, processFlowQuery.TypeProcess, processFlowQuery.Back);

            return new GetProcessFlowResponse()
            {
                CountPages = result.CountPages,
                DataConfiguration = result.DataConfiguration,
                BackStep = result.TypeBack,
                CurrentStep = result.TypeFrom,
                NextStep = result.TypeFront,
                Conditional = result.Conditional
            };
        }
    }
}
