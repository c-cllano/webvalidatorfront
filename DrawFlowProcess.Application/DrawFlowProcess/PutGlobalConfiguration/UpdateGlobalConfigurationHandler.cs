using DrawFlowProcess.Domain.Repositories;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.PutGlobalConfiguration
{
    public class UpdateGlobalConfigurationHandler : IRequestHandler<UpdateGlobalConfigurationCommand, bool>
    {
        private readonly IGlobalConfigurationRepository _repository;

        public UpdateGlobalConfigurationHandler(IGlobalConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateGlobalConfigurationCommand request, CancellationToken cancellationToken)
        {
            if (request.Document == null)
                return false;

            return await _repository.UpdateGlobalConfigurationAsync(request.AgreementId, request.WorkFlowId, request.Document);
        }
    }
}
