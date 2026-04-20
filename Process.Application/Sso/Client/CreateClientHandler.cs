using MediatR;
using Process.Domain.Entities;
using Process.Domain.Services;

namespace Process.Application.Sso.Client
{
    public class CreateClientHandler(ISsoService ssoService) : IRequestHandler<CreateClientQuery, object>
    {
        private readonly ISsoService _ssoService = ssoService;
        public async Task<object> Handle(CreateClientQuery request, CancellationToken cancellationToken)
        {
            CreateClient createClient = new()
            {
                ClientId = request.ClientId,
                DisplayName = request.DisplayName,
                Description = request.Description,
                ClientType = request.ClientType,
                ClientSecret = request.ClientSecret
            };
            var result = await _ssoService.CreateClient(createClient);
            return result!;
        }
    }
}
