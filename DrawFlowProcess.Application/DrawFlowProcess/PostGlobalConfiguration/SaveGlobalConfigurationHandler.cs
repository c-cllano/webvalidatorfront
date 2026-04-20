using DrawFlowProcess.Domain.Repositories;
using MediatR;

namespace DrawFlowProcess.Application.DrawFlowProcess.PostGlobalConfiguration
{
    public class SaveGlobalConfigurationHandler : IRequestHandler<SaveGlobalConfigurationCommand, bool>
    {
        private readonly IGlobalConfigurationRepository _repository;
        public SaveGlobalConfigurationHandler(IGlobalConfigurationRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(SaveGlobalConfigurationCommand request, CancellationToken cancellationToken)
        {
            if (request.Document == null)
                return false;
            var result = await _repository.SaveGlobalConfigurationAsync(request.Document);
            return result;
        }
    }
}