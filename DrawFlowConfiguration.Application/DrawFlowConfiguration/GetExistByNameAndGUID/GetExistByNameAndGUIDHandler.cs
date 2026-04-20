using DrawFlowConfiguration.Domain.Repositories;
using MediatR;

namespace DrawFlowConfiguration.Application.DrawFlowConfiguration.GetExistByNameAndGUID
{

    public class GetExistByNameAndGUIDHandler : IRequestHandler<GetExistByNameAndGUIDQuery, object>
    {
        private readonly IValidationTransaction _repository;

        public GetExistByNameAndGUIDHandler(IValidationTransaction repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(GetExistByNameAndGUIDQuery requets, CancellationToken cancellationToken)
        {
            var configuration = await _repository.GetExistByNameAndGUID((Guid)requets.AgreementID!, requets.Name!, requets.WorkflowID);
            return configuration;
        }
    }
}
