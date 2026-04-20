using MediatR;
using Nuget.LogService.Models;
using Nuget.LogService.Services;

namespace ApiGateways.Aplication.LogServices.CreateError
{
    public class CreateErrorHandler(ILogServices service) : IRequestHandler<CreateErrorCommand, bool>
    {
        private readonly ILogServices _service = service;

        public async Task<bool> Handle(CreateErrorCommand command, CancellationToken cancellationToken)
        {
            bool createErrorResult = false;
            createErrorResult = await _service.CreateErrorAsync(new CreateErrorIn
            {
                SeverityID = command.SeverityID,
                Description = command.Description,
                UserID = command.UserID,
                TransactionID = command.TransactionID,
                Code = command.Code,
                Component = command.Component,
                Machine = command.Machine,
                Date = command.Date
            });
            return createErrorResult;
        }
    }
}
